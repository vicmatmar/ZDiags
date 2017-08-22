using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Text.RegularExpressions;

namespace ZDiags
{

    class Diags : IDisposable
    {
        enum Relays : uint { DUT = 0, BUTTON, BLE, USB2, USB1 };
        enum Sensors : uint { BUZZER_AUDIO = 0, GREEN_LIGHT, RED_LIGHT, YELLOW_LIGHT };

        string _dutport_name, _bleport_name;
        SerialCOM _dutport, _bleport;

        string _smt_serial;

        public enum Customer { IRIS, Amazone };
        Customer _custumer;

        string _serialize_model = "IH200";
        char _serialize_hw_version = '4';

        bool _program_radios = true;
        public bool Program_Radios { get { return _program_radios; } set { _program_radios = value; } }

        int _program_radios_timeout_sec = 140;
        public int Program_Radios_Timeout_Sec { get { return _program_radios_timeout_sec; } set { _program_radios_timeout_sec = value; } }

        public delegate void StatusHandler(object sender, string status_txt);
        public event StatusHandler Status_Event;


        public Diags(string dut_port_name, string ble_port_name, string smt_serial, Customer customer, char hw_version)
        {
            _dutport_name = dut_port_name;
            _bleport_name = ble_port_name;

            _smt_serial = smt_serial;
            _custumer = customer;
            _serialize_hw_version = hw_version;
        }

        void fire_status(string msg)
        {
            if (Status_Event != null)
            {
                Status_Event(this, msg);
            }
        }

        public void Run()
        {
            DateTime start_time = DateTime.Now;

            set_all_relays(false);

            using (SerialCOM dutport = getDUTPort())
            using (SerialCOM bleport = getBLEPort())
            {
                // Trun BLE board so 
                write_SingleDIO(Relays.BLE, true);
                bleport.WaitForStr("U-Boot", 3);

                // Trun DUT on
                write_SingleDIO(Relays.DUT, true);
                dutport.WaitForStr("U-Boot", 3);

                // Login
                fire_status("Login to DUT");
                login(dutport);

                // Program_Radios
                if (Program_Radios)
                {
                    fire_status("Program Radios");
                    DateTime t1 = DateTime.Now;
                    dutport.WriteLine("program_radios");
                    dutport.WaitForStr("Radio programming is complete.", Program_Radios_Timeout_Sec);
                    TimeSpan ts1 = DateTime.Now - t1;
                    fire_status("Radio programming is complete after " + ts1.TotalSeconds.ToString() + "sec");
                }

                // Diags
                fire_status("Start diagnostics");
                dutport.WriteLine("diagnostics");
                dutport.WaitForStr("Press the reset button...", 3);

                fire_status("Press the reset button...");
                write_SingleDIO(Relays.BUTTON, true);
                dutport.WaitForStr(@"Insert both USB drives and attach increased load to usb0. Press <enter> when ready...", 10);
                fire_status("USB0 Test");
                write_SingleDIO(Relays.USB1, true);
                Thread.Sleep(500);
                dutport.WriteLine();

                dutport.WaitForStr(@"Remove increased load from usb0 and attach load to usb1. Press <enter> when ready...", 5);
                fire_status("USB1 Test");
                write_SingleDIO(Relays.USB1, false);
                write_SingleDIO(Relays.USB2, true);
                Thread.Sleep(500);
                dutport.WriteLine();

                fire_status("Buzzer Test");
                dutport.WaitForStr("Buzzer on?", 3);
                double val = -1.0;
                for (int i = 0; i < 5; i++)
                {
                    val = read_SingelAi(Sensors.BUZZER_AUDIO);
                    fire_status(string.Format("Buzzer Voltage: {0}", val.ToString("f2")));
                    if (val > 3.0)
                        break;
                    Thread.Sleep(100);
                }
                if (val > 3.0)
                {
                    dutport.WriteLine("y");
                }
                else
                {
                    dutport.WriteLine("n");
                    string emsg = string.Format("Unable to detect buzzer. Volatgae was: {0}", val.ToString("f2"));
                    throw new Exception(emsg);
                }


                fire_status("LED Tests");
                led_test(Sensors.GREEN_LIGHT, 0.8, 1.2, "green");
                led_test(Sensors.RED_LIGHT, 3.8, 4.5, "red");
                led_test(Sensors.YELLOW_LIGHT, 2.8, 3.8, "yellow");


                // Other tests
                fire_status("Other Built-in Tests...");
                dutport.WaitForStr("All Tests Passed", 20);
                fire_status("All Tests Passed");

                // BLE test
                fire_status("BLE Test");
                Random rand = new Random(DateTime.Now.Second);
                int channel = rand.Next(0, 27);
                fire_status("Login to BLE");
                login(bleport);
                bleport.WriteLine("ble_rx " + channel.ToString() + " 3000");
                dutport.WriteLine("ble_tx " + channel.ToString());
                bleport.WaitForStr("Packets received:", 5, clear_data: false);
                string data = bleport.Data;
                Match match = Regex.Match(data, @"Packets received: (\d*)", RegexOptions.Singleline);
                if (!match.Success || match.Groups.Count < 2)
                {
                    string emsg = string.Format("Unable to get packets received from BLE master.\r\nData was: {0}", data);
                    throw new Exception(emsg);
                }
                int packets_received = Convert.ToInt32(match.Groups[1].Value);
                if (packets_received < 1500)
                {
                    string emsg = string.Format("BLE packets received < 1500.\r\nPackets received were: {0}", packets_received);
                    throw new Exception(emsg);
                }
                string rmsg = string.Format("BLE packets received: {0}", packets_received);


            }

            fire_status("Serialize...");
            Serialize();

            TimeSpan ts = DateTime.Now - start_time;
            string tmsg = string.Format("ETime: {0}s.", ts.TotalSeconds);
            fire_status(tmsg);
            set_all_relays(false);


        }

        public void Serialize()
        {
            

            long lowes_serial = LowesSerial.GetSerial(
                    model: LowesSerial.Model.IH200, 
                    hw_version: (byte)_serialize_hw_version, 
                    datetime: DateTime.Now, 
                    factory: 7, 
                    test_station: 1, 
                    tester: 2);

            using (CLStoreEntities cx = new CLStoreEntities())
            using (SerialCOM port = getDUTPort())
            {
                //port.WriteLine();
                //port.WaitForStr("#", 3);

                int customer_id = cx.LowesCustomers.Where(c => c.Name == _custumer.ToString()).Single().Id;

                // See if this board already had a mac assigned
                long mac = MACAddrUtils.INVALID_MAC;
                var hubs = cx.LowesHubs.Where(h => h.smt_serial == _smt_serial).OrderByDescending(h => h.date);
                if (hubs.Any())
                    mac = hubs.ToArray()[0].MacAddress.MAC;
                if (mac == MACAddrUtils.INVALID_MAC)
                {
                    MACAddrUtils macutil = new MACAddrUtils();
                    mac = macutil.GetNewMac();
                }
                int macid = MACAddrUtils.GetMacId(mac);

                // Inser the 
                LowesHub lh = new LowesHub();
                lh.customer_id = customer_id;
                lh.hw_ver = _serialize_hw_version;
                lh.mac_id = macid;
                lh.smt_serial = _smt_serial.ToString();

                cx.LowesHubs.Add(lh);
                cx.SaveChanges();

                string macstr = MACAddrUtils.LongToStr(mac);




                int batch_no = 1234567890;

                string cmd = string.Format("serialize {0} model {1} customer {2} hw_version {3} batch_no {4}",
                    macstr, _serialize_model, _custumer.ToString(), _serialize_hw_version, batch_no);
                port.WriteLine(cmd);

                port.WaitForStr("Device serialization is complete - please reboot", 5);
            }

        }

        void login(SerialCOM port)
        {
            fire_status("Wait for login...");
            port.WaitForStr("login:", 20);
            port.WriteLine();
            port.WaitForStr("login:", 3);
            fire_status("Login");
            port.WriteLine("root");
            port.WaitForStr("IRIS MFG Shell.*#", 3, isRegx: true);

        }

        void led_test(Sensors sensor, double min_val, double max_val, string color)
        {

            _dutport.WaitForStr(color + " led on?", 3);
            double val = -1.0;
            for (int i = 0; i < 5; i++)
            {
                val = read_SingelAi(sensor);

                string rmsg = string.Format(color + " led Voltage: {0}", val.ToString("f2"));
                rmsg += string.Format(" ({0}-{1})", min_val.ToString("f1"), max_val.ToString("f1"));
                fire_status(rmsg);

                if (val >= min_val && val < max_val)
                    break;

                Thread.Sleep(250);
            }

            if (val >= min_val && val < max_val)
            {
                _dutport.WriteLine("y");
            }
            else
            {
                _dutport.WriteLine("n");
                string emsg = string.Format("Unable to detect {0} led. Volatgae was: {1}", color, val.ToString("f2"));
                throw new Exception(emsg);
            }

        }

        static void set_all_relays(bool value)
        {
            Array relays = Enum.GetValues(typeof(Relays));
            foreach (uint relay in relays)
            {
                NIUtils.Write_SingleDIO(relay, value);

            }
        }

        static void write_SingleDIO(Relays relay, bool value)
        {
            NIUtils.Write_SingleDIO((uint)relay, value);
        }

        static double read_SingelAi(Sensors sensor)
        {
            return NIUtils.Read_SingelAi((uint)sensor);
        }

        SerialCOM getDUTPort()
        {
            if (_dutport == null || _dutport.IsDisposed)
                _dutport = new SerialCOM(_dutport_name);
            return _dutport;
        }
        SerialCOM getBLEPort()
        {
            if (_bleport == null || _bleport.IsDisposed)
                _bleport = new SerialCOM(_bleport_name);
            return _bleport;
        }


        public void Dispose()
        {
            if (_dutport != null)
            {
                _dutport.Dispose();
                _dutport = null;
            }

            if (_bleport != null)
            {
                _bleport.Dispose();
                _bleport = null;
            }

        }
    }
}
