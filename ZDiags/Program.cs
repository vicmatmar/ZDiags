using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;
using System.Text.RegularExpressions;
using ZCommon;

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

            Console.WriteLine("Parameters used:");

            string propname = "com_dut";
            save_property(propname, options.Com_DUT);
            string valuestr = Properties.Settings.Default[propname].ToString();
            if (valuestr == null || valuestr == string.Empty)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                Console.WriteLine(propname.ToUpper() + " not specified");
                return -1;
            }
            Console.WriteLine(propname.ToUpper() + ": " + valuestr);
            string com_dut = valuestr;

            propname = "com_ble";
            save_property(propname, options.com_ble);
            valuestr = Properties.Settings.Default[propname].ToString();
            if (valuestr == null || valuestr == string.Empty)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                Console.WriteLine(propname.ToUpper() + " not specified");
                return -1;
            }
            Console.WriteLine(propname.ToUpper() + ": " + valuestr);
            string com_ble = valuestr;

            Console.WriteLine("SMT Serial: " + options.SMT_Serial);
            Diags.Customer customer = Diags.Customer.IRIS; ;
            if (options.Custumer_Amazone)
                customer = Diags.Customer.Amazone;
            Console.WriteLine("Custumer: " + customer.ToString());
            Console.WriteLine("HW Version: " + options.HW_Version);

            string tester = DataUtils.OperatorName(options.Tester);
            int tester_id = DataUtils.OperatorId(tester);
            Console.WriteLine("Tester: " + tester);
            Console.WriteLine();

            // For debug
            //Console.WriteLine("Press Enter to continue");
            //Console.ReadLine();

            Console.WriteLine("Run Tests...");
            try
            {
                using 
                (
                    Diags diags = new Diags
                    (
                        dut_port_name: com_dut, ble_port_name: com_ble,
                        smt_serial: options.SMT_Serial, 
                        customer: customer, 
                        hw_version: options.HW_Version,
                        tester: tester,
                        hub_ip_addr: options.Hub_IP
                    )
                )
                {

                    
                    diags.LogFolder = Properties.Settings.Default.Log_Folder;
                    Directory.CreateDirectory(diags.LogFolder);

                    diags.Status_Event += Diags_Status_Event;
                    diags.Program_Radios = options.Program_Radios;

                    //diags.Serialize();

                    diags.Run();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

                if(ex is System.Data.Entity.Validation.DbEntityValidationException)
                {
                    var exd = (System.Data.Entity.Validation.DbEntityValidationException)ex;
                    foreach (var eves in exd.EntityValidationErrors)
                    {
                        Console.WriteLine(eves.Entry.Entity.ToString());
                        foreach(var eve in eves.ValidationErrors)
                            Console.WriteLine(eve.ErrorMessage);
                    }
                }
                else if(ex is System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    var exd = (System.Data.Entity.Infrastructure.DbUpdateException)ex;
                    Console.WriteLine(exd.InnerException.InnerException.Message);
                }




                return -1;
            }
            finally
            {
                Diags.Set_all_relays(false);
            }
            Console.WriteLine("All Tests Passed");

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
