using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Text.RegularExpressions;
using ZCommon;

namespace ZBatt
{
    public class BatteryTest
    {

        LED_Group _leds;
        LED _red_led;
        public LED LED_Red { get { return _red_led; } }

        LED _green_led;
        public LED LED_Green { get { return _green_led; } }

        LED _yellow_led;
        public LED LED_Yellow { get { return _yellow_led; } }


        bool _led_test_enabled = false;
        public bool LEDTestEnabled { get { return _led_test_enabled; } set { _led_test_enabled = value; } }

        bool _invalidate_enabled = false;
        public bool InvalidateEnabled { get { return _invalidate_enabled; } set { _invalidate_enabled = value; } }

        string _host;

        public enum Status_Level { Info, Debug }

        public delegate void StatusHandler(object sender, string status_txt, Status_Level level = Status_Level.Info);
        public event StatusHandler Status_Event;

        const string _ssh_prompt = "#";

        public BatteryTest(string host)
        {
            _host = host;

            _red_led = new LED((uint)BatteryJig.Sensors.RED_LIGHT, 0.25, 0.5, "red");
            _green_led = new LED((uint)BatteryJig.Sensors.GREEN_LIGHT, 0.001, 0.06, "green");
            _yellow_led = new LED((uint)BatteryJig.Sensors.YELLOW_LIGHT, 0.01, 0.18, "yellow");

            _leds = new LED_Group(new LED[] { _red_led, _green_led, _yellow_led });

        }

        //# show battery

        //Battery Information:
        //Voltage:         1.10
        //Maximum Voltage: 1.10
        //Level:           -1.00

        public void Run()
        {

            // There seems to be a problem with the jigs I have not been able to discovered yet.
            // The test should be to apply battery power fisrt, then main power and the board should boot.
            // Does not happen in the jig.
            // I'm able to sypply battery power using jig and manually connect main power to a transformer and
            // it works. So it seems to be a problem with the jig and not the units.

            fire_status("Cycle Power");
            BatteryJig.Set_all_relays(false);
            Thread.Sleep(500);
            fire_status("Power on DUT");
            BatteryJig.Write_SingleDIO(BatteryJig.Relays.DUT, true);
            Thread.Sleep(1000);


            DateTime start = DateTime.Now;
            if (LEDTestEnabled)
            {
                fire_status("LED Boot Test...");
                LEDBootPatternTest();
            }
            TimeSpan ts_total = DateTime.Now - start;
            int ts_towait = 4000 - (int)ts_total.TotalSeconds * 1000;
            if (ts_towait > 0)
                Thread.Sleep((int)ts_towait);

            fire_status("Power on BATTERY");
            BatteryJig.Write_SingleDIO(BatteryJig.Relays.BATT, true);
            Thread.Sleep(1000);

            // Try to connect
            fire_status("Connecting...");
            SSHUtil ssh = Connect(_host);
            try
            {
                bootWait(ssh);

                fire_status("Battery Test");
                string data = ssh.WriteWait("show battery", "Level:", 3);
                double volts = parseVolatge(data);
                string msg = string.Format("Battery voltage before DUT power removed detected at {0}", volts);
                fire_status(msg);
                if (volts < 5.0)
                {
                    msg = string.Format("Battery power before DUT power removed too low.  Detected at {0}", volts);
                    throw new Exception(msg);
                }


                fire_status("DUT power off");
                BatteryJig.Write_SingleDIO(BatteryJig.Relays.DUT, false);
                Thread.Sleep(3000);

                data = ssh.WriteWait("show battery", "Level:", 3);
                volts = parseVolatge(data);
                msg = string.Format("Battery voltage after DUT power removed detected at {0}", volts);
                fire_status(msg);
                if (volts < 4.0)
                {
                    msg = string.Format("Battery power after DUT power removed too low.  Detected at {0}", volts);
                    throw new Exception(msg);
                }

                fire_status("DUT power back on");
                BatteryJig.Write_SingleDIO(BatteryJig.Relays.DUT, true);
                Thread.Sleep(1000);

                data = ssh.WriteWait("show battery", "Level:", 3);
                volts = parseVolatge(data);
                msg = string.Format("Battery voltage after DUT power re-applied at {0}", volts);
                fire_status(msg);
                if (volts < 5.0)
                {
                    msg = string.Format("Battery power after DUT power ed too low.  Detected at {0}", volts);
                    throw new Exception(msg);
                }

                if (InvalidateEnabled)
                {
                    fire_status("Clean up...");
                    data = cleanup(ssh);
                    fire_status(data, Status_Level.Debug);

                    fire_status("Invalidate...");

                    //# boot invalidate
                    //Current partition has been invalidated!

                    fire_status("Reboot");
                    ssh.WriteWait("reboot", "The system is going down for reboot NOW");
                    BatteryJig.Write_SingleDIO(BatteryJig.Relays.BATT, false); // If batt is not off, we can't reboot (weird!)
                    Thread.Sleep(5000);

                    // Check for the boot LED pattern until it fails
                    LEDBootPatternTest();
                    start = DateTime.Now;
                    TimeSpan ts;
                    while (true)
                    {
                        try
                        {
                            LEDBootPatternTest();
                            Thread.Sleep(3000);
                        }
                        catch
                        {
                            break;
                        }

                        ts = DateTime.Now - start;
                        if (ts.TotalSeconds > 30)
                            throw new Exception("LED pattern did not changed");
                    }
                    ts = DateTime.Now - start;
                    // The LED patter should have changed

                    LEDInvalidatedPatternTest();

                    // Check our connection is not good anymore
                    if (ssh.IsConnected)
                        throw new Exception("Our ssh is still connected. Are you sure the hub rebootted?");

                    // Now try to connect
                    // It should not let os
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

        void LEDInvalidatedPatternTest()
        {
            DateTime start = DateTime.Now;
            int detected_count = 0;
            while (true)
            {
                if (_red_led.isOff && _yellow_led.isOn && _green_led.isOn)
                {
                    detected_count++;
                    Thread.Sleep(500);

                    if (detected_count > 5)
                        break;
                }
                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalSeconds > 10)
                {
                    throw new Exception("LEDs invalidated pattern not detected");
                }
            }

        }


        void LEDBootPatternTest()
        {
            DateTime start = DateTime.Now;

            _leds.ResetMaxValues();
            while (true)
            {
                //if (_red_led.isOn && _yellow_led.isOn && _green_led.isOn)
                if(_leds.areOn)
                {
                    break;
                }

                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalSeconds > 4)
                {
                    string emsg = string.Format("Not all LEDs detected ON. R:{0}, G:{1}, Y:{2}",
                        _red_led.MaxValue.ToString("G2"), _green_led.MaxValue.ToString("G2"), _yellow_led.MaxValue.ToString("G2"));
                    throw new Exception(emsg);
                }
            }
            fire_status("LEDs detected ON");

            start = DateTime.Now;
            while (true)
            {
                if (_leds.areOff)
                {
                    break;
                }
                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalSeconds > 10)
                {
                    string emsg = string.Format("Not all LEDs detected OFF. R:{0} Y:{1} G:{2}",
                        _red_led.Value.ToString("G2"), _yellow_led.Value.ToString("G2"), _green_led.Value.ToString("G2"));

                    emsg += string.Format("\r\nR:{0} Y:{1} G{2}", _red_led.isOff, _yellow_led.isOff, _green_led.isOff);
                    throw new Exception(emsg);
                }
            }
            fire_status("LEDs detected OFF");
        }

        /// <summary>
        /// Truns all LEDs on and then off capturing 
        /// voltage values
        /// </summary>
        /// <returns>RGY on and off values</returns>
        public double[] GetLEDsValues()
        {
            double[] values = new double[6];

            // Trun power on
            BatteryJig.Set_all_relays(false);
            BatteryJig.Write_SingleDIO(BatteryJig.Relays.DUT, true);

            SSHUtil ssh = Connect(_host);
            try
            {
                bootWait(ssh);
                enterShell(ssh);

                bool state = true;
                _red_led.Turn(state, ssh);
                _yellow_led.Turn(state, ssh);
                _green_led.Turn(state, ssh);

                Thread.Sleep(500);

                double vred_on = _red_led.Value;
                double vgreen_on = _green_led.Value;
                double vyellow_on = _yellow_led.Value;

                state = false;
                _red_led.Turn(state, ssh);
                _yellow_led.Turn(state, ssh);
                _green_led.Turn(state, ssh);

                Thread.Sleep(500);

                double vred_off = _red_led.Value;
                double vgreen_off = _green_led.Value;
                double vyellow_off = _yellow_led.Value;

                int i = 0;
                values[i++] = vred_on;
                values[i++] = vgreen_on;
                values[i++] = vyellow_on;
                values[i++] = vred_off;
                values[i++] = vgreen_off;
                values[i++] = vyellow_off;

                exitShell(ssh);
            }
            finally
            {
                ssh.Dispose();
            }

            return values;
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
