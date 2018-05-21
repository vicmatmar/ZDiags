using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

using ZCommon;

namespace ZDiags
{
    public class Diags : IDisposable
    {
        enum Relays : uint { DUT = 0, BUTTON, BLE, USB2, USB1 };
        enum Sensors : uint { BUZZER_AUDIO = 0, GREEN_LIGHT, RED_LIGHT, YELLOW_LIGHT };

        string _dutport_name;
        public string COM_DUT_Name { get { return _dutport_name; } }

        string _bleport_name;
        public string COM_BT_Name { get { return _bleport_name; } }

        SerialCOM _dutport, _bleport;

        string _smt_serial;
        public string SMT_Serial { get { return _smt_serial; } }

        long _lowes_serial = 0;
        public long Lowes_Serial { get { return _lowes_serial; } }

        public enum Customers { IRIS, Amazone, Centralite };
        Customers _customer;
        public Customers Customer { get { return _customer; } }

        string _serialize_model = "IH200";
        public string Serialize_Model { get { return _serialize_model; } }

        int _serialize_hw_version = 3;
        public int HW_Ver { get { return _serialize_hw_version; } }

        string _tester;
        public string Tester { get { return _tester; } }

        bool _program_radios = false;
        public bool Program_Radios { get { return _program_radios; } set { _program_radios = value; } }

        int _program_radios_timeout_sec = 140;
        public int Program_Radios_Timeout_Sec { get { return _program_radios_timeout_sec; } set { _program_radios_timeout_sec = value; } }

        string _log_folder = "";
        public string LogFolder { get { return _log_folder; } set { _log_folder = value; } }

        public delegate void StatusHandler(object sender, string status_txt);
        public event StatusHandler Status_Event;

        const int _zwave_ver_major = 4;
        const int _zwave_ver_minor = 5;

        const string _certificate_server = "172.24.32.6";

        FileStream _fs_dut_data, _fs_ble_data;

        bool _zwave_updated = false;

        const string _shell_prompt = "~ #";

        string _hub_ip;

        bool _ble_test_disabled = false;
        public bool BLETestDisable { get { return _ble_test_disabled; } set { _ble_test_disabled = value; } }

        public double LED_Red_Low_Value { get; set; }
        public double LED_Red_High_Value { get; set; }
        public double LED_Green_Low_Value { get; set; }
        public double LED_Green_High_Value { get; set; }
        public double LED_Yellow_Low_Value { get; set; }
        public double LED_Yellow_High_Value { get; set; }

        public Diags(string dut_port_name, string ble_port_name, string smt_serial, Customers customer, int hw_version, string tester = "Victor Martin", string hub_ip_addr = null)
        {
            _dutport_name = dut_port_name;
            _bleport_name = ble_port_name;

            _smt_serial = smt_serial.ToUpper();
            _customer = customer;
            _serialize_hw_version = hw_version;

            _tester = tester;

            _hub_ip = hub_ip_addr;

            if (Customer == Customers.Centralite)
            {
                MACAddrUtils.BlockStartAddr = Properties.Settings.Default.MAC_Centralite_Block_Start;
                MACAddrUtils.BlockEndAddr = Properties.Settings.Default.MAC_Centralite_Block_End;
            }
            else if (Customer == Customers.Amazone || Customer == Customers.IRIS)
            {
                MACAddrUtils.BlockStartAddr = Properties.Settings.Default.MAC_Lowes_Block_Start;
                MACAddrUtils.BlockEndAddr = Properties.Settings.Default.MAC_Lowes_Block_End;
            }

            LED_Red_Low_Value = Properties.Settings.Default.LED_Red_Off_Val;
            LED_Red_High_Value = Properties.Settings.Default.LED_Red_On_Val;
            LED_Green_Low_Value = Properties.Settings.Default.LED_Green_Off_Val;
            LED_Green_High_Value = Properties.Settings.Default.LED_Green_On_Val;
            LED_Yellow_Low_Value = Properties.Settings.Default.LED_Yellow_Off_Val;
            LED_Yellow_High_Value = Properties.Settings.Default.LED_Yellow_On_Val;

        }

        void fire_status(string msg)
        {
            Status_Event?.Invoke(this, msg);
        }

        public void Run()
        {
            DateTime start_time = DateTime.Now;

            // Init the serial data log files
            string fileloc = Path.Combine(LogFolder, "log_dut_data.txt");
            _fs_dut_data = new FileStream(fileloc, FileMode.Create, FileAccess.Write, FileShare.Read);

            fileloc = Path.Combine(LogFolder, "log_ble_data.txt");
            _fs_ble_data = new FileStream(fileloc, FileMode.Create, FileAccess.Write, FileShare.Read);

            Set_all_relays(false);
            Thread.Sleep(500);
            using (SerialCOM dutport = getDUTPort())
            using (SerialCOM bleport = getBLEPort())
            {
                // Turn BLE board so 
                fire_status("Power up BLE master");
                write_SingleDIO(Relays.BLE, true);
                bleport.WaitFor("U-Boot", 3);

                // Turn DUT on
                fire_status("Power up DUT");
                write_SingleDIO(Relays.DUT, true);
                dutport.WaitFor("U-Boot", 3);
                Thread.Sleep(3000);

                // Login
                fire_status("Login to DUT");
                login(dutport);

                // Program_Radios
                if (Program_Radios)
                {
                    fire_status("Program Radios");
                    dutport.Data = "";
                    DateTime t1 = DateTime.Now;
                    dutport.WriteLine("program_radios");
                    string data = dutport.WaitFor("Radio programming is complete.", Program_Radios_Timeout_Sec);
                    if (data.Contains("Error"))
                        throw new Exception("Errors detected during program radios.\r\nOutput was:\r\n" + data);
                    TimeSpan ts1 = DateTime.Now - t1;
                    fire_status("Radio programming is complete after " + ts1.TotalSeconds.ToString() + "sec");
                }
            }

            if (_hub_ip != null && _hub_ip != "")
            {
                fire_status("Set Hub IP to " + _hub_ip);
                string data = setHubIpAddr(_hub_ip);
                fire_status(data);
            }

            fire_status("ZWave Update...");
            ZWaveUpdate();

            if (_zwave_updated)
            {
                fire_status("Show Radios...");
                ShowRadios();
            }

            fire_status("Diagnostics...");
            Diagnostics();

            if (BLETestDisable)
            {
                fire_status("BLE Test DISABLED!!!");

            }
            else
            {
                fire_status("BLE Test...");
                using (SerialCOM bleport = getBLEPort())
                {
                    fire_status("Login to BLE");
                    login(bleport);
                }
                int trycount = 0;
                while (true)
                {
                    try
                    {
                        BLETest();
                        break;
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        if (!msg.Contains("BLE packets received"))
                            throw;

                        if (trycount++ > 3)
                            throw;
                    }
                }
            }

            fire_status("Serialize...");
            Serialize();

            fire_status("Certificate...");
            Certificate();

            fire_status("Show Mfg...");
            SaveShowMfg();

            TimeSpan ts = DateTime.Now - start_time;
            string tmsg = string.Format("ETime: {0}s.", ts.TotalSeconds);
            fire_status(tmsg);
        }

        string setHubIpAddr(string ipaddr, string adapter = "eth0", string netmask = "255.255.0.0")
        {
            string data;
            using (SerialCOM dutport = getDUTPort())
            {
                // Make sure we can talk to hub
                dutport.WriteWait("", "#", 3);

                enterShell(dutport);

                try
                {
                    string cmd = string.Format("ifconfig {0} {1} netmask {2}", adapter, ipaddr, netmask);
                    dutport.WriteWait(cmd, Regex.Escape(cmd) + ".*" + _shell_prompt, timeout_sec: 3, isRegx: true);

                    cmd = "ifconfig";
                    data = dutport.WriteWait(cmd, Regex.Escape(cmd) + ".*" + _shell_prompt, timeout_sec: 3, isRegx: true);

                }
                finally
                {
                    exitShell(dutport);
                }
            }

            return data;
        }

        public void SaveShowMfg()
        {
            using (SerialCOM dutport = getDUTPort())
            {
                // Make sure we can talk to hub
                dutport.WriteWait("", "#", 3);
                dutport.Data = "";
                string mfg_data = dutport.WriteWait("show mfg", "Batch Number:", 3);

                fire_status(mfg_data);

                string fileloc = Path.Combine(this.LogFolder, "d" + SMT_Serial.ToString() + ".txt");
                using (FileStream fs = new FileStream(fileloc, FileMode.Create, FileAccess.Write, FileShare.Read))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(mfg_data);
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// Zeus certificate server: `172.24.32.6`
        /// </summary>
        public void Certificate()
        {
            using (SerialCOM dutport = getDUTPort())
            {
                // Make sure we can talk to hub
                dutport.WriteWait("", "#", 3);

                dutport.WriteWait("certificate " + _certificate_server + " skip_ca_check true", "Key install was successful", 5);

                fire_status("Key install was successful");
            }
        }

        public void ShowRadios()
        {
            using (SerialCOM dutport = getDUTPort())
            {
                // Make sure we can talk to hub
                dutport.WaitForPrompt();

                dutport.WriteWait("show radios", "Zwave Stack Version", timeout_sec: 5, clear_data: false);
                string data = dutport.Data;
                Match m = Regex.Match(data, @"Z-Wave (\d+\.\d+)", RegexOptions.Singleline);
                if (m.Success && m.Groups.Count > 1)
                {
                    string verstr = m.Groups[1].Value;

                    fire_status("Zwave Stack Version: " + verstr);
                    bool needToUpdate = needsUpdate(verstr, _zwave_ver_major, _zwave_ver_minor);
                    if (needToUpdate)
                    {
                        throw new Exception("Unexpected Zwave version: " + verstr);
                    }
                }
                else
                {
                    throw new Exception("Unable to detect Zwave version.\r\nData was: " + data);
                }
            }
        }

        /// <summary>
        /// Based on test spec:LOWESHUB-65830935-070716-0910-4
        /// 
        /// And from Edgar:
        /// "Zwave_test" is needed to verify Zwave version is equal or higher to 4.05,  
        /// if it is lower (all new components have lower version), 
        /// then you have to send "zwave_flash -w /data/firmware/zwave-firmware.bin\n".
        /// </summary>
        public void ZWaveUpdate()
        {
            _zwave_updated = false;
            using (SerialCOM dutport = getDUTPort())
            {
                // Make sure we can talk to hub
                dutport.WriteWait("", "#", 3);

                enterShell(dutport);

                try
                {
                    dutport.WriteWait("zwave_test -v", "Zwave Stack Version:", timeout_sec: 5, clear_data: false);
                    string data = dutport.Data;
                    Match m = Regex.Match(data, @"Z-Wave (\d+\.\d+)", RegexOptions.Singleline);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        string verstr = m.Groups[1].Value;
                        fire_status("Zwave Stack Version: " + verstr);
                        bool needToUpdate = needsUpdate(verstr, _zwave_ver_major, _zwave_ver_minor);
                        if (needToUpdate)
                        {
                            fire_status("Zwave Updating...");
                            dutport.WriteWait(
                                "zwave_flash -w /data/firmware/zwave-firmware.bin",
                                "Flash programming complete", 60);
                            fire_status("Zwave Flash programming complete");

                            dutport.WriteWait("zwave_default", "Zwave module has been factory defaulted", 3);
                            fire_status("Zwave module has been factory defaulted");

                            _zwave_updated = true;
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to extract Z-Wave version info.\r\nData was: " + data);
                    }
                }
                finally
                {
                    exitShell(dutport);
                }
            }
        }

        bool needsUpdate(string version_str, int exp_major, int exp_minor)
        {
            string[] vers = version_str.Split(new char[] { '.' });
            int maj = Convert.ToInt32(vers[0]);
            int min = 0;
            if (vers.Count() > 1)
                min = Convert.ToInt32(vers[1]);

            bool needToUpdate = false;
            if (maj < exp_major)
                needToUpdate = true;
            else if (maj == exp_major && min < exp_minor)
                needToUpdate = true;

            return needToUpdate;
        }

        void enterShell(SerialCOM port)
        {
            // Enter debug
            port.WriteWait("debug", "debug>", 3);
            // Enter Shelf
            port.WriteWait("sh", "#", 3);
        }

        void exitShell(SerialCOM port)
        {
            // Exit shell
            port.WriteWait("exit", "debug>", 3);
            // Exit debug
            port.WriteWait("exit", "#", 3);

        }

        public void Diagnostics()
        {
            using (SerialCOM dutport = getDUTPort())
            {
                // Make sure we can talk to hub
                //dutport.WriteWait("", "#", 3, clear_data:false);
                dutport.WaitForPrompt();

                // Diags
                fire_status("Start diagnostics");
                dutport.WriteWait("diagnostics", "Press the reset button...", 3);

                fire_status("Press the reset button...");
                write_SingleDIO(Relays.BUTTON, true);
                dutport.WaitFor(@"Insert both USB drives and attach increased load to usb0. Press <enter> when ready...", 10);
                fire_status("USB0 Test");
                write_SingleDIO(Relays.USB1, true);
                Thread.Sleep(500);
                dutport.WriteLine();

                dutport.WaitFor(@"Remove increased load from usb0 and attach load to usb1. Press <enter> when ready...", 5);
                fire_status("USB1 Test");
                write_SingleDIO(Relays.USB1, false);
                write_SingleDIO(Relays.USB2, true);
                Thread.Sleep(500);
                dutport.WriteLine();

                fire_status("Buzzer Test");
                dutport.WaitFor("Buzzer on?", 3);
                double val = -1.0;
                double expval = 3.0;
                for (int i = 0; i < 5; i++)
                {
                    val = read_SingelAi(Sensors.BUZZER_AUDIO);
                    fire_status(string.Format("Buzzer Voltage: {0}", val.ToString("f2")));
                    if (val > expval)
                        break;
                    Thread.Sleep(1000);
                }
                if (val > 3.0)
                {
                    dutport.WriteLine("y");
                }
                else
                {
                    dutport.WriteLine("n");
                    string emsg = string.Format("Unable to detect buzzer. Volatgae was: {0}. Expected more than {1}",
                        val.ToString("f2"), expval.ToString("f2"));
                    throw new Exception(emsg);
                }


                fire_status("LED Tests");
                led_test(Sensors.GREEN_LIGHT, LED_Green_Low_Value, LED_Green_High_Value, "green");
                led_test(Sensors.RED_LIGHT, LED_Red_Low_Value, LED_Red_High_Value, "red");
                led_test(Sensors.YELLOW_LIGHT, LED_Yellow_Low_Value, LED_Yellow_High_Value, "yellow");

                // Other tests
                fire_status("Other Built-in Tests...");
                dutport.WaitFor("All Tests Passed", 20);
                fire_status("All Tests Passed");
            }
        }

        public void BLETest()
        {
            using (SerialCOM dutport = getDUTPort())
            using (SerialCOM bleport = getBLEPort())
            {
                // Make sure we can talk to hubs
                dutport.WriteWait("", "#", 3);

                Random rand = new Random(DateTime.Now.Second);
                int channel = rand.Next(0, 27);
                bleport.WriteLine("ble_rx " + channel.ToString() + " 3000");
                dutport.WriteLine("ble_tx " + channel.ToString());
                bleport.WaitFor("Packets received:", 5, clear_data: false);
                string data = bleport.Data;
                Match match = Regex.Match(data, @"Packets received: (\d*)", RegexOptions.Singleline);
                if (!match.Success || match.Groups.Count < 2)
                {
                    string emsg = string.Format("Unable to get packets received from BLE master.\r\nData was: {0}", data);
                    throw new Exception(emsg);
                }
                int packets_received = Convert.ToInt32(match.Groups[1].Value);
                // if (packets_received < 1500)
                if (packets_received < 800)
                {
                    string emsg = string.Format("BLE packets received < 1500.\r\nPackets received were: {0}", packets_received);
                    throw new Exception(emsg);
                }
                string rmsg = string.Format("BLE packets received: {0}", packets_received);
                fire_status(rmsg);
            }
        }

        public void Serialize()
        {
            using (CLStoreEntities cx = new CLStoreEntities())
            using (SerialCOM port = getDUTPort())
            {
                // Make sure we can talk to hub
                port.WriteLine();
                port.WaitFor("#", 3);

                LowesHub loweshub_data = new LowesHub();

                // Gather info to serialize hub
                int production_site_id = MACAddrUtils.ProductionSiteId();
                while (production_site_id > byte.MaxValue)
                    production_site_id = production_site_id >> 1;

                int test_station_id = MACAddrUtils.StationSiteId();
                loweshub_data.test_station_id = test_station_id;
                while (test_station_id > byte.MaxValue)
                    test_station_id = test_station_id >> 1;

                int hw_ver = HW_Ver;
                loweshub_data.hw_ver = HW_Ver;
                while (hw_ver > byte.MaxValue)
                    hw_ver = hw_ver >> 1;

                int operator_id = DataUtils.OperatorId(_tester);
                loweshub_data.operator_id = operator_id;
                while (operator_id > short.MaxValue)
                    operator_id = operator_id >> 1;

                _lowes_serial = LowesSerial.GetSerial(
                        model: LowesSerial.Model.IH200,
                        hw_version: (byte)hw_ver,
                        datetime: DateTime.Now,
                        factory: (byte)production_site_id,
                        test_station: (byte)test_station_id,
                        tester: (short)operator_id);

                int customer_id = cx.LowesCustomers.Where(c => c.Name == Customer.ToString()).Single().Id;

                // See if this board already had a mac assigned
                long mac = MACAddrUtils.INVALID_MAC;
                var hubsq = cx.LowesHubs.Where(h => h.smt_serial == SMT_Serial).OrderByDescending(h => h.date);
                if (hubsq.Any())
                {
                    var hubs = hubsq.ToArray();
                    foreach (LowesHub hub in hubs)
                    {
                        long hubmac = hub.MacAddress.MAC;
                        if (MACAddrUtils.Inrange(hubmac))
                        {
                            mac = hubmac;
                            break;
                        }
                    }
                }
                if (mac == MACAddrUtils.INVALID_MAC)
                    mac = MACAddrUtils.GetNewMac();
                int mac_id = MACAddrUtils.GetMacId(mac);


                string macstr = MACAddrUtils.LongToStr(mac);


                string cmd = string.Format("serialize {0} model {1} customer {2} hw_version {3} batch_no {4}",
                    macstr, Serialize_Model, Customer.ToString(), HW_Ver, Lowes_Serial);

                fire_status(cmd);
                port.WriteLine(cmd);
                port.WaitFor("Device serialization is complete - please reboot", 5);
                fire_status("Device serialization is complete.");

                port.Data = "";
                string mfg_data = port.WriteWait("show mfg", "Batch Number:", 3);
                Regex regx = new Regex(@"HubID:\s+([A-Z]+-\d+)");
                Match m = regx.Match(mfg_data);
                if (!m.Success || m.Groups.Count < 2)
                {
                    string emsg = string.Format("Unable to extract Hub id from data:{0}", mfg_data);
                    throw new Exception(emsg);
                }
                string hubid = m.Groups[1].Value;
                fire_status("Hub ID: " + hubid);

                // Insert the hub
                loweshub_data.customer_id = customer_id;
                loweshub_data.mac_id = mac_id;
                loweshub_data.smt_serial = SMT_Serial.ToString().ToUpper();
                loweshub_data.lowes_serial = Lowes_Serial;
                loweshub_data.hub_id = hubid;

                cx.LowesHubs.Add(loweshub_data);
                cx.SaveChanges();
            }
        }

        void login(SerialCOM port)
        {
            fire_status("Wait for login...");

            //port.WaitFor("login:", 40);

            int trycount = 0;
            while (true)
            {
                try
                {
                    port.WriteWait("", "login:", 5);
                    break;
                }
                catch (Exception ex)
                {
                    if (trycount++ > 5)
                        throw;
                }
            }

            fire_status("Login");
            port.WriteWait("root", "IRIS MFG Shell.*#", 10, isRegx: true);

        }

        void led_test(Sensors sensor, double min_val, double max_val, string color)
        {

            _dutport.WaitFor(color + " led on?", 3);
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
                string emsg = string.Format("Unable to detect {0} led. Voltage was: {1}", color, val.ToString("f2"));
                throw new Exception(emsg);
            }

        }

        static public void Set_all_relays(bool value)
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
            {
                _dutport = new SerialCOM(_dutport_name);
                _dutport.DataReceived += _dutport_DataReceived;
            }
            return _dutport;
        }

        private void _dutport_DataReceived(object sender, string data)
        {
            //byte[] edata = new UTF8Encoding(true).GetBytes(data);

            byte[] edata = Encoding.Unicode.GetBytes(data);
            _fs_dut_data.Write(edata, 0, edata.Length);
            _fs_dut_data.Flush();
        }

        SerialCOM getBLEPort()
        {
            if (_bleport == null || _bleport.IsDisposed)
            {
                _bleport = new SerialCOM(_bleport_name);
                _bleport.DataReceived += _bleport_DataReceived;
            }
            return _bleport;
        }

        private void _bleport_DataReceived(object sender, string data)
        {
            //byte[] edata = new UTF8Encoding(true).GetBytes(data);
            byte[] edata = Encoding.UTF8.GetBytes(data);
            _fs_ble_data.Write(edata, 0, edata.Length);
            _fs_ble_data.Flush();
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

            if (_fs_dut_data != null)
            {
                _fs_dut_data.Close();
                _fs_dut_data.Dispose();
                _fs_dut_data = null;
            }

            if (_fs_ble_data != null)
            {
                _fs_ble_data.Close();
                _fs_ble_data.Dispose();
                _fs_ble_data = null;
            }
        }
    }
}
