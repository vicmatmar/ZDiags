using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZDiags
{
    class Program
    {
        enum Relays : uint { DUT = 0, BUTTON, BLE, USB2, USB1 };
        enum Sensors : uint { BUZZER_AUDIO = 0, GREEN_LIGHT, RED_LIGHT, YELLOW_LIGHT };

        static int Main(string[] args)
        {
            var options = new Options();
            var isValid = CommandLine.Parser.Default.ParseArguments(args, options);

            if (!isValid)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return -1;
            }

            if (options.Com_dut != null)
            {
                Properties.Settings.Default.COM_DUT = options.Com_dut;
                Properties.Settings.Default.Save();
            }

            string com_dut = Properties.Settings.Default.COM_DUT;
            if (com_dut == null || com_dut == string.Empty)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                Console.WriteLine("DUT Com port not specified");
                return -1;
            }
            Console.WriteLine("DUT COM Port: " + com_dut);

            SerialUtils dutport = new SerialUtils(com_dut);

            set_all_relays(false);
            write_SingleDIO(Relays.DUT, true);

            DateTime start = DateTime.Now;
            while(true)
            {
                string data = dutport.Data;
                if (data != null && data.Contains("beaglebone login:"))
                    break;
                Thread.Sleep(5);

                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalSeconds > 20)
                {
                    dutport.Dispose();
                    throw new Exception("Timeout");
                }
            }

            return 0;
        }

        static void set_all_relays(bool value)
        {
            Array relays= Enum.GetValues( typeof(Relays) );
            foreach ( uint relay in relays)
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
