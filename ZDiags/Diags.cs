using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace ZDiags
{

    class Diags : IDisposable
    {
        enum Relays : uint { DUT = 0, BUTTON, BLE, USB2, USB1 };
        enum Sensors : uint { BUZZER_AUDIO = 0, GREEN_LIGHT, RED_LIGHT, YELLOW_LIGHT };

        SerialUtils _dutport;

        bool _program_radios = true;
        public bool Program_Radios { get { return _program_radios; } set { _program_radios = value; } }

        int _program_radios_timeout_sec = 140;
        public int Program_Radios_Timeout_Sec { get { return _program_radios_timeout_sec; } set { _program_radios_timeout_sec = value; } }

        public delegate void StatusHandler(object sender, string status_txt);
        public event StatusHandler Status_Event;


        public Diags(string dut_port)
        {
            _dutport = new SerialUtils(dut_port);
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

            try
            {
                write_SingleDIO(Relays.BLE, true);
                write_SingleDIO(Relays.DUT, true);

                // Login
                fire_status("Wait for login...");
                _dutport.WaitForStr("beaglebone login:", 20);
                _dutport.WriteLine();
                _dutport.WaitForStr("beaglebone login:", 3);
                fire_status("Login");
                _dutport.WriteLine("root");
                _dutport.WaitForStr("Welcome root.*#", 3, isRegx: true);

                // Program_Radios
                if(Program_Radios)
                {
                    fire_status("Program Radios");
                    DateTime t1 = DateTime.Now;
                    _dutport.WriteLine("program_radios");
                    _dutport.WaitForStr("Radio programming is complete.", Program_Radios_Timeout_Sec);
                    TimeSpan ts1 = DateTime.Now - t1;
                    fire_status("Radio programming is complete after " + ts1.TotalSeconds.ToString() + "sec");
                }

                // Diags
                fire_status("Start diagnostics");
                _dutport.WriteLine("diagnostics");
                _dutport.WaitForStr("Press the reset button...", 3);

                fire_status("Press the reset button...");
                write_SingleDIO(Relays.BUTTON, true);
                _dutport.WaitForStr(@"Insert both USB drives and attach increased load to usb0. Press <enter> when ready...", 10);
                fire_status("USB0 Test");
                write_SingleDIO(Relays.USB1, true);
                Thread.Sleep(500);
                _dutport.WriteLine();

                _dutport.WaitForStr(@"Remove increased load from usb0 and attach load to usb1. Press <enter> when ready...", 5);
                fire_status("USB1 Test");
                write_SingleDIO(Relays.USB1, false);
                write_SingleDIO(Relays.USB2, true);
                Thread.Sleep(500);
                _dutport.WriteLine();

                fire_status("Buzzer Test");
                _dutport.WaitForStr("Buzzer on?", 3);
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
                    _dutport.WriteLine("y");
                }
                else
                {
                    _dutport.WriteLine("n");
                    string emsg = string.Format("Unable to detect buzzer. Volatgae was: {0}", val.ToString("f2"));
                    throw new Exception(emsg);
                }


                fire_status("LED Tests");

                _dutport.WaitForStr("green led on?", 3);
                val = -1.0;
                double min_val = 0.8;
                for (int i = 0; i < 5; i++)
                {
                    val = read_SingelAi(Sensors.GREEN_LIGHT);
                    fire_status(string.Format("GREEN LED Voltage: {0}", val.ToString("f2")));
                    if (val > min_val)
                        break;
                    Thread.Sleep(250);
                }
                if (val > min_val)
                {
                    _dutport.WriteLine("y");
                }
                else
                {
                    _dutport.WriteLine("n");
                    string emsg = string.Format("Unable to detect GREEN LED. Volatgae was: {0}", val.ToString("f2"));
                    throw new Exception(emsg);
                }

                _dutport.WaitForStr("red led on?", 3);
                val = -1.0;
                for (int i = 0; i < 5; i++)
                {
                    val = read_SingelAi(Sensors.RED_LIGHT);
                    fire_status(string.Format("RED LED Voltage: {0}", val.ToString("f2")));
                    if (val > min_val)
                        break;
                    Thread.Sleep(250);
                }
                if (val > min_val)
                {
                    _dutport.WriteLine("y");
                }
                else
                {
                    _dutport.WriteLine("n");
                    string emsg = string.Format("Unable to detect RED LED. Volatgae was: {0}", val.ToString("f2"));
                    throw new Exception(emsg);
                }

                _dutport.WaitForStr("yellow led on?", 3);
                val = -1.0;
                for (int i = 0; i < 5; i++)
                {
                    val = read_SingelAi(Sensors.YELLOW_LIGHT);
                    fire_status(string.Format("YELLOW LED Voltage: {0}", val.ToString("f2")));
                    if (val > min_val)
                        break;
                    Thread.Sleep(250);
                }
                if (val > min_val)
                {
                    _dutport.WriteLine("y");
                }
                else
                {
                    _dutport.WriteLine("n");
                    string emsg = string.Format("Unable to detect YELLOW LED. Volatgae was: {0}", val.ToString("f2"));
                    throw new Exception(emsg);
                }

                // Other tests
                _dutport.WaitForStr("All Tests Passed", 20);


                TimeSpan ts = DateTime.Now - start_time;
                string tmsg = string.Format("ETime: {0}s.", ts.TotalSeconds);
                fire_status(tmsg);
            }
            finally
            {
                set_all_relays(false);
                _dutport.Dispose();
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

        public void Dispose()
        {
            if (_dutport != null)
            {
                _dutport.Dispose();
                _dutport = null;
            }
        }
    }
}
