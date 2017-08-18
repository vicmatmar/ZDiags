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

        const long INVALID_MAC = 0;

        static int Main(string[] args)
        {
            var options = new Options();
            var parser_options = new CommandLine.ParserSettings { MutuallyExclusive = true };
            var parser = new CommandLine.Parser(parser_options);

            var isValid = parser.ParseArguments(args, options);
            if (!isValid)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return -1;
            }

            string propname = "com_dut";
            save_property(propname, options.Com_DUT);
            string valuestr = Properties.Settings.Default[propname].ToString();
            if (valuestr == null || valuestr == string.Empty)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                Console.WriteLine(propname + " not specified");
                return -1;
            }
            Console.WriteLine(propname + ": " + valuestr);
            string com_dut = valuestr;

            propname = "com_ble";
            save_property(propname, options.com_ble);
            valuestr = Properties.Settings.Default[propname].ToString();
            if (valuestr == null || valuestr == string.Empty)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                Console.WriteLine(propname + " not specified");
                return -1;
            }
            Console.WriteLine(propname + ": " + valuestr);
            string com_ble = valuestr;


            Diags.Customer custumer = Diags.Customer.IRIS; ;
            if (options.Custumer_Amazone)
                custumer = Diags.Customer.Amazone;
            Console.WriteLine("Custumer: " + custumer.ToString());


            Console.WriteLine("Run Diags...");
            try
            {
                using (Diags diags = new Diags(com_dut, com_ble, custumer))
                {
                    diags.Status_Event += Diags_Status_Event;
                    diags.Program_Radios = options.Program_Radios;

                    //diags.Serialize();

                    diags.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return -1;
            }
            Console.WriteLine("Diags Passed");

            return 0;
        }

        static void save_property(string propertyName, object value)
        {
            if (value != null)
            {
                Properties.Settings.Default[propertyName] = value;
                Properties.Settings.Default.Save();
            }
        }

        static void Diags_Status_Event(object sender, string status_txt)
        {
            string msg = string.Format("{0:G}: {1}", DateTime.Now, status_txt);
            Console.WriteLine(msg);
        }

    }
}
