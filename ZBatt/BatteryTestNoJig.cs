using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Text.RegularExpressions;
using ZCommon;
using System.IO;


namespace ZBatt
{
    public class BatteryTestNoJig
    {

        bool _invalidate_enabled = false;
        public bool InvalidateEnabled { get { return _invalidate_enabled; } set { _invalidate_enabled = value; } }

        string _host;

        public enum Status_Level { Info, Debug }

        public delegate void StatusHandler(object sender, string status_txt, Status_Level level = Status_Level.Info);
        public event StatusHandler Status_Event;

        const string _ssh_prompt = "#";

        string _log_folder = "";
        public string LogFolder { get { return _log_folder; } set { _log_folder = value; } }

        string _smt_serial;

        bool _check_bat_voltage = false;

        public BatteryTestNoJig(string host, string smt_serial)
        {
            _host = host;
            _smt_serial = smt_serial;

        }

        public void Run()
        {

            fire_status("Insert BATTERIES and press enter to continue.");
            Console.ReadLine();

            fire_status("Connect POWER adaptor and check all LEDs blink.  Press Y if they do, N if not");
            while(true)
            {
                char c = Console.ReadKey().KeyChar;
                if (c == 'y' || c == 'Y')
                    break;
                else if (c == 'n' || c == 'N')
                    throw new Exception("No all LEDs blinked at first power up.");
            }


            fire_status("Connecting...");
            SSHUtil ssh = Connect(_host);
            try
            {
                bootWait(ssh);

                fire_status("Battery Test");

                string data;
                double volts;
                string msg = "";

                string cmd = "show battery";
                string outputcheck = "Level:";

                try
                {
                    data = ssh.WriteWait(cmd, outputcheck, 1);
                    _check_bat_voltage = true;

                }
                catch(System.TimeoutException ex)
                {
                    _check_bat_voltage = false;
                }

                if (!_check_bat_voltage)
                {
                    outputcheck = "Battery Information is not available";
                }
                fire_status("Outputcheck: " + outputcheck);

                if (_check_bat_voltage)
                {
                    data = ssh.WriteWait(cmd, outputcheck, 3);
                    volts = parseVolatge(data);
                    msg = string.Format("Battery voltage before DUT power removed detected at {0}", volts);
                    fire_status(msg);
                    if (volts < 5.0)
                    {
                        msg = string.Format("Battery power before DUT power removed too low.  Detected at {0}", volts);
                        throw new Exception(msg);
                    }
                }
                else
                {
                    data = ssh.WriteWait(cmd, outputcheck, 3);
                    fire_status(outputcheck);
                }

                fire_status("Remove the POWER adapter and press enter to continue.");
                Console.ReadLine();
                Thread.Sleep(3000);

                if (_check_bat_voltage)
                {
                    data = ssh.WriteWait(cmd, outputcheck, 3);
                    volts = parseVolatge(data);
                    msg = string.Format("Battery voltage after DUT power removed detected at {0}", volts);
                    fire_status(msg);
                    if (volts < 4.0)
                    {
                        msg = string.Format("Battery power after DUT power removed too low.  Detected at {0}", volts);
                        throw new Exception(msg);
                    }
                }
                else
                {
                    data = ssh.WriteWait(cmd, outputcheck, 3);
                    fire_status(outputcheck);
                }


                fire_status("Reconnect the POWER adapter and press enter to continue");
                Console.ReadLine();

                if (_check_bat_voltage)
                {
                    data = ssh.WriteWait(cmd, outputcheck, 3);
                    volts = parseVolatge(data);
                    msg = string.Format("Battery voltage after DUT power re-applied at {0}", volts);
                    fire_status(msg);
                    if (volts < 5.0)
                    {
                        msg = string.Format("Battery power after DUT power ed too low.  Detected at {0}", volts);
                        throw new Exception(msg);
                    }
                }
                else
                {
                    data = ssh.WriteWait(cmd, outputcheck, 3);
                    fire_status(outputcheck);
                }

                if (InvalidateEnabled)
                {
                    fire_status("Clean up...");
                    data = cleanup(ssh);
                    fire_status(data, Status_Level.Debug);

                    fire_status("Invalidate...");
                    //ssh.WriteWait("boot invalidate", "Current partition has been invalidated!", 5);

                    fire_status("Rebooting");
                    ssh.WriteWait("reboot", "The system is going down for reboot NOW");
                    Thread.Sleep(5000);

                    // Check for the boot LED pattern until it fails
                    // The LED patter should have changed
                    fire_status("LED blinking pattern should change.  Only GREEN and YELLOW bliking. Y/N?");
                    while (true)
                    {
                        char c = Console.ReadKey().KeyChar;
                        if (c == 'y' || c == 'Y')
                            break;
                        else if (c == 'n' || c == 'N')
                            throw new Exception("LEDs blinking pattern did not change.");
                    }

                    // Check our connection is not good anymore
                    fire_status("Test for not connected");
                    if (ssh.IsConnected)
                        throw new Exception("Our ssh is still connected. Are you sure the hub rebootted?");

                    // Now try to connect
                    // It should not let os
                    fire_status("Test connecting is not possible");
                    try
                    {
                        ssh.Connect();
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }

                    // To revert using chipserver
                    //jumpered lowes hub
                    //power up
                    //root/a1
                    //mount /dev/mmcblk0p5 /mnt
                    //vi /mnt/bootindex
                    //# Change to 5
                    //sync
                    //umount /mnt
                    //# Remove jumper
                    //reboot
                }

                SaveShowMfg(ssh);

            }
            catch
            {
                throw;
            }
            finally
            {
                ssh.Dispose();
            }
        }

        public void SaveShowMfg(SSHUtil ssh)
        {
            // Make sure we can talk to hub
            ssh.WriteWait("", "#", 3);
            ssh.Data = "";
            string mfg_data = ssh.WriteWait("show mfg", "Batch Number:", 3);

            fire_status("SMT_Number: " + _smt_serial);
            fire_status(mfg_data);

            string fileloc = Path.Combine(this.LogFolder, "b" + _smt_serial.ToString() + ".txt");
            using (FileStream fs = new FileStream(fileloc, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("SMT_Number: " + _smt_serial);

                sw.Write(mfg_data);
                sw.Close();
            }
        }


        string cleanup(SSHUtil ssh)
        {

            ssh.Data = "";

            enterShell(ssh, clear_data: false);

            string cmds = @"find /data/agent/ -type f -exec md5sum {} \; | sort -k 34 | md5sum";

            ssh.WriteWait(cmds, Regex.Escape(cmds) + ".*" + _ssh_prompt, 10, clear_data: false, isRegx: true);

            string[] cmd_list = new string[]
            {
                "cd /data/agent",
                "chmod 0777 /data/agent",
                "chown -R agent .",
                "chgrp -R agent .",
                "touch factory_reset",
                "rm -f /data/log/messages*",
                "rm -f /data/log/dmesg*",
                "rm -f /data/zwave_*",
                "cat /data/mfg_test_report.json",
                "rm -f /data/mfg_test_report.json",
                "sync"
            };
            foreach (string cmd in cmd_list)
            {
                ssh.WriteWait(cmd, Regex.Escape(cmd) + ".*" + _ssh_prompt, 5, clear_data: false, isRegx: true);
            }

            exitShell(ssh, clear_data: false);

            return ssh.Data;
        }

        static void enterShell(SSHUtil ssh, bool clear_data = true)
        {
            // Enter debug
            ssh.WriteWait("debug", "debug>", 3, clear_data: clear_data);
            // Enter shell
            ssh.WriteWait("sh", "#", 3, clear_data: clear_data);
        }

        static void exitShell(SSHUtil ssh, bool clear_data = true)
        {
            // Exit shell
            ssh.WriteWait("exit", "debug>", 3, clear_data: clear_data);
            // Exit debug
            ssh.WriteWait("exit", "#", 3, clear_data: clear_data);

        }

        static void bootWait(SSHUtil ssh)
        {
            ssh.WaitFor("IRIS MFG Shell.*#", 5, isRegx: true);
        }

        static public SSHUtil Connect(string host)
        {
            SSHUtil ssh = new SSHUtil(host);
            DateTime start = DateTime.Now;
            while (true)
            {
                try
                {
                    ssh.Connect();
                    break;
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    string msg = ex.Message;
                }

                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalSeconds > 30)
                {
                    ssh.Dispose();
                    throw new Exception("Unable to connect to host: " + host);
                }
            }

            return ssh;

        }

        double parseVolatge(string data)
        {

            Regex regx = new Regex(@"^Voltage:\s+(\d+\.\d+)", RegexOptions.Multiline);
            Match m = regx.Match(data);
            if (!m.Success && m.Groups.Count < 2)
                throw new Exception("Unable to parse volatge data.\r\nInput was:\r\n" + data);


            double value = Convert.ToDouble(m.Groups[1].Value);
            return value;
        }

        void fire_status(string msg, Status_Level status_level = Status_Level.Info)
        {
            Status_Event?.Invoke(this, msg, status_level);
        }

    }
}
