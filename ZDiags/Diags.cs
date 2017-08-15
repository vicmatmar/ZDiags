using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace ZDiags
{
    class Diags
    {
        enum Relays : uint { DUT = 0, BUTTON, BLE, USB2, USB1 };
        enum Sensors : uint { BUZZER_AUDIO = 0, GREEN_LIGHT, RED_LIGHT, YELLOW_LIGHT };

        SerialUtils _dutport;

        bool _program_radios = true;
        public bool Program_Radios { get { return _program_radios; } set { _program_radios = value; } }

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

            write_SingleDIO(Relays.DUT, true);

            fire_status("Wait for login...");
            _dutport.WaitForStr("beaglebone login:", 20);
            _dutport.WriteLine();
            _dutport.WaitForStr("beaglebone login:", 3);
            fire_status("Login");
            _dutport.WriteLine("root");
            _dutport.WaitForStr("Welcome root.*#", 30, isRegx:true);

            TimeSpan ts = DateTime.Now - start_time;
            string msg = string.Format("ETime: {0}s.", ts.TotalSeconds);
            fire_status(msg);

            _dutport.Dispose();

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

    }
}
