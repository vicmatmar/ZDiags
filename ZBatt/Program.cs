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

        static int Main(string[] args)
        {
            //Console.WriteLine("Press Enter to continue");
            //Console.Read();

            var options = new Options();
            var parser_options = new CommandLine.ParserSettings { MutuallyExclusive = true };
            var parser = new CommandLine.Parser(parser_options);
            var isValid = parser.ParseArguments(args, options);
            if (!isValid)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return -1;
            }


            try
            {
                BatteryTest battery_test = new BatteryTest(options.Host);
                battery_test.Status_Event += Battery_test_Status_Event;
                battery_test.Run();

                LED _red_led = new LED((uint)Sensors.RED_LIGHT, 0.25, 0.5);
                LED _yellow_led = new LED((uint)Sensors.YELLOW_LIGHT, 0.01, 0.18);
                LED _green_led = new LED((uint)Sensors.GREEN_LIGHT, 0.001, 0.06);

                using (_ssh = new SSHUtil(options.Host))
                {
                    set_all_relays(false);

                    write_SingleDIO(Relays.DUT, true);
                    write_SingleDIO(Relays.BATT, true);

                    LED red_led = new LED((uint)Sensors.RED_LIGHT, 0.25, 0.5);
                    bool isblink = red_led.isBlicking(3);
                    while (red_led.isBlicking()) ;
                    isblink = red_led.isBlicking();

                    while (true)
                    {
                        double val = read_SingelAi(Sensors.RED_LIGHT);
                        Console.WriteLine("{0}", val);
                        val = read_SingelAi(Sensors.YELLOW_LIGHT);
                        Console.WriteLine("{0}", val);
                        val = read_SingelAi(Sensors.GREEN_LIGHT);
                        Console.WriteLine("{0}", val);
                        Console.WriteLine();
                        Thread.Sleep(400);
                    }



                    write_SingleDIO(Relays.DUT, false);


                    _ssh.Connect();

                    verifyAlive();

                    _ssh.WriteLine("show mfg");
                    _ssh.WaitFor("Batch Number", 5, clear_data: false);
                    Console.WriteLine(_ssh.Data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

                return -2;
            }

            return 0;
        }

        private static void Battery_test_Status_Event(object sender, string status_txt)
        {
            string msg = string.Format("{0:G}: {1}", DateTime.Now, status_txt);
            Console.WriteLine(msg);
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
