﻿using System;
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

            if (options.COM_DUT != null)
            {
                Properties.Settings.Default.COM_DUT = options.COM_DUT;
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

            try
            {
                Diags diags = new Diags(com_dut);
                diags.Status_Event += Diags_Status_Event;
                diags.Program_Radios = options.Program_Radios;
                diags.Run();
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                return -1;
            }


            return 0;
        }

        private static void Diags_Status_Event(object sender, string status_txt)
        {
            string msg = string.Format("{0:G}: {1}", DateTime.Now, status_txt);
            Console.WriteLine(msg);
        }

    }
}
