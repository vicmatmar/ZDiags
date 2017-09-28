using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;
using Renci.SshNet;
using ZCommon;


// #complete flash
// cd /lowesI
// dd if=iris_image_v2.0.1.055.bin of=/dev/mmcblk0 

//debug
//sh
//find /data/agent/ -type f -exec md5sum { } \; | sort -k 34 | md5sum
//cd /data/agent
//chown -R agent .
//chgrp -R agent .
//touch factory_reset
//rm -f /data/log/messages*
//rm -f /data/log/dmesg*
//rm -f /data/zwave_*
//rm -f /data/mfg_test_report.json
//sync


// exit;exit
//# boot invalidate
//Current partition has been invalidated!

//#To revert it
//debug
//sh
//mount /dev/mmcblk0p5 /mnt
//vi /mnt/bootindex
//# Change to 5
//sync
//umount /mnt
//# Remove jumper
//reboot


namespace ZBatt
{
    class Program
    {
        enum Relays : uint { BATT = 0, DUT };

        enum Sensors : uint { RED_LIGHT = 0, GREEN_LIGHT, YELLOW_LIGHT };

        static SSHUtil _ssh;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static int Main(string[] args)
        {
            //Console.WriteLine("Press Enter to continue");
            //Console.Read();

            _log.Info("App started");

            var options = new Options();
            var parser_options = new CommandLine.ParserSettings { MutuallyExclusive = true };
            var parser = new CommandLine.Parser(parser_options);
            var isValid = parser.ParseArguments(args, options);
            if (!isValid)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return -1;
            }

            Console.WriteLine("SMT Serial: " + options.SMT_Serial);
            Console.WriteLine("Host: " + options.Host);
            Console.WriteLine();


            if (options.CalibrateLEDs)
            {

                BatteryTest batcal = new BatteryTest(options.Host, options.SMT_Serial);

                char save_option = 'n';
                while (true)
                {
                    Console.WriteLine("Getting LEDs values...\r\n");
                    double[] values = batcal.GetLEDsValues();
                    int i = 0;

                    Console.WriteLine("Red On   : {0}", values[i++].ToString("G2"));
                    Console.WriteLine("Green On : {0}", values[i++].ToString("G2"));
                    Console.WriteLine("Yellow On: {0}", values[i++].ToString("G2"));

                    Console.WriteLine();

                    Console.WriteLine("Red Off   : {0}", values[i++].ToString("G2"));
                    Console.WriteLine("Green Off : {0}", values[i++].ToString("G2"));
                    Console.WriteLine("Yellow Off: {0}", values[i++].ToString("G2"));

                    i = 0;
                    Properties.Settings.Default.LED_Red_On_Val = values[i++];
                    Properties.Settings.Default.LED_Green_On_Val = values[i++];
                    Properties.Settings.Default.LED_Yellow_On_Val = values[i++];

                    Properties.Settings.Default.LED_Red_Off_Val = values[i++];
                    Properties.Settings.Default.LED_Green_Off_Val = values[i++];
                    Properties.Settings.Default.LED_Yellow_Off_Val = values[i++];

                    Console.WriteLine("Save Values? (y/n/r):");
                    save_option = Convert.ToChar(Console.Read());
                    if (save_option != 'r')
                        break;
                }
                if (save_option == 'y')
                    Properties.Settings.Default.Save();
            }

            try
            {
                if (!options.NoJigMode)
                {

                    BatteryTest battery_test = new BatteryTest(options.Host, options.SMT_Serial);

                    battery_test.InvalidateEnabled = !options.DisableInvalidate;
                    battery_test.Status_Event += Battery_test_Status_Event;

                    battery_test.LogFolder = Properties.Settings.Default.Log_Folder;
                    Directory.CreateDirectory(battery_test.LogFolder);

                    battery_test.LED_Red.OnVal = Properties.Settings.Default.LED_Red_On_Val;
                    battery_test.LED_Red.OffVal = Properties.Settings.Default.LED_Red_Off_Val;
                    battery_test.LED_Green.OnVal = Properties.Settings.Default.LED_Green_On_Val;
                    battery_test.LED_Green.OffVal = Properties.Settings.Default.LED_Green_Off_Val;
                    battery_test.LED_Yellow.OnVal = Properties.Settings.Default.LED_Yellow_On_Val;
                    battery_test.LED_Yellow.OffVal = Properties.Settings.Default.LED_Yellow_Off_Val;

                    battery_test.Run();

                }
                else
                {
                    BatteryTestNoJig batery_test = new BatteryTestNoJig(options.Host, options.SMT_Serial);
                    batery_test.InvalidateEnabled = !options.DisableInvalidate;

                    batery_test.Status_Event += Baterytest_Status_Event;

                    batery_test.LogFolder = Properties.Settings.Default.Log_Folder;
                    Directory.CreateDirectory(batery_test.LogFolder);

                    batery_test.Run();

                }

            }
            catch (Exception ex)
            {
                _log.Fatal(ex.Message + "\r\n" + ex.StackTrace);
                return -2;
            }
            finally
            {

            }

            _log.Info("All Tests Passed");

            if (!options.PrintLabelDisabled)
            {
                _log.Info("Printing label...");
                try
                {
                    DataUtils.PrintHubLabel(
                        options.SMT_Serial,
                        Properties.Settings.Default.ZPL_Lable_File,
                        Properties.Settings.Default.Printer_Address);
                }
                catch (Exception ex)
                {
                    _log.Fatal(ex.Message + "\r\n" + ex.StackTrace);
                    return -2;
                }
                finally
                {

                }
            }

            return 0;
        }

        private static void Baterytest_Status_Event(object sender, string status_txt, BatteryTestNoJig.Status_Level status_level = BatteryTestNoJig.Status_Level.Info)
        {
            switch (status_level)
            {
                case BatteryTestNoJig.Status_Level.Info:
                    _log.Info(status_txt);
                    break;
                case BatteryTestNoJig.Status_Level.Debug:
                    _log.Debug(status_txt);
                    break;
                default:
                    _log.Info(status_txt);
                    break;
            }
        }

        private static void Battery_test_Status_Event(object sender, string status_txt, BatteryTest.Status_Level status_level)
        {
            //string msg = string.Format("{0:G}: {1}", DateTime.Now, status_txt);
            //Console.WriteLine(msg);
            switch (status_level)
            {
                case BatteryTest.Status_Level.Info:
                    _log.Info(status_txt);
                    break;
                case BatteryTest.Status_Level.Debug:
                    _log.Debug(status_txt);
                    break;
                default:
                    _log.Info(status_txt);
                    break;
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

        static void set_all_relays(bool value)
        {
            Array relays = Enum.GetValues(typeof(Relays));
            foreach (uint relay in relays)
            {
                NIUtils.Write_SingleDIO(relay, value);
            }
        }

        //# show battery

        //Battery Information:
        //Voltage:         1.10
        //Maximum Voltage: 1.10
        //Level:           -1.00
        static void verifyAlive()
        {
            _ssh.WriteLine("show battery");
            _ssh.WaitFor("Battery Information is not available", 10);
        }

    }
}
