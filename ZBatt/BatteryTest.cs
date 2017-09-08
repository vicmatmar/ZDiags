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

        LED _red_led = new LED((uint)BatteryJig.Sensors.RED_LIGHT, 0.25, 0.5);
        LED _yellow_led = new LED((uint)BatteryJig.Sensors.YELLOW_LIGHT, 0.01, 0.18);
        LED _green_led = new LED((uint)BatteryJig.Sensors.GREEN_LIGHT, 0.001, 0.06);

        string _host;

        public delegate void StatusHandler(object sender, string status_txt);
        public event StatusHandler Status_Event;

        public BatteryTest(string host)
        {
            _host = host;
        }

        //# show battery

        //Battery Information:
        //Voltage:         1.10
        //Maximum Voltage: 1.10
        //Level:           -1.00

        public void Run()
        {

            // For some reason we need to power using main first 
            // and wait sometime before we apply battery power or the hub
            // won't be.  3 seconds seems to be enough.
            // Also if we don't have battery applied before we log in
            // we get wrong values.

            fire_status("Cycle Power");
            BatteryJig.Set_all_relays(false);
            Thread.Sleep(500);

            LED _red_led = new LED((uint)BatteryJig.Sensors.RED_LIGHT, 0.25, 0.5);
            LED _yellow_led = new LED((uint)BatteryJig.Sensors.YELLOW_LIGHT, 0.29, 0.4);
            LED _green_led = new LED((uint)BatteryJig.Sensors.GREEN_LIGHT, 0.001, 0.06);

            BatteryJig.Write_SingleDIO(BatteryJig.Relays.DUT, true);


            DateTime start = DateTime.Now;
            TimeSpan ts_total = DateTime.Now - start;
            while (true)
            {
                if (_red_led.isOn && _yellow_led.isOn && _green_led.isOn)
                {
                    break;
                }
                else if (_red_led.isOn && _green_led.isOn)
                {
                    string emsg = string.Format("Y:{0}", _yellow_led.Value.ToString("G2"));
                    fire_status(emsg);

                }
                TimeSpan ts = DateTime.Now - start;
                ts_total += ts;
                if (ts.TotalSeconds > 5)
                {
                    throw new Exception("Not all LED detected ON");
                }
            }

            start = DateTime.Now;
            while (true)
            {
                if (_red_led.isOff && _yellow_led.isOff && _green_led.isOff)
                {
                    break;
                }
                TimeSpan ts = DateTime.Now - start;
                ts_total += ts;
                if (ts.TotalSeconds > 10)
                {
                    string emsg = string.Format("Not all LED detected OFF. R:{0} Y:{1} G:{2}",
                        _red_led.Value.ToString("G2"), _yellow_led.Value.ToString("G2"), _green_led.Value.ToString("G2"));

                    emsg += string.Format("\r\nR{0} Y:{1} G{2}", _red_led.isOff, _yellow_led.isOff, _green_led.isOff);
                    throw new Exception(emsg);
                }
            }

            int ts_towait = 4000 - (int)ts_total.TotalSeconds;
            if (ts_towait > 0)
                Thread.Sleep((int)ts_towait);


            BatteryJig.Write_SingleDIO(BatteryJig.Relays.BATT, true);
            Thread.Sleep(1000);

            // Try to connect
            fire_status("Connecting...");
            SSHUtil ssh = new SSHUtil(_host);
            start = DateTime.Now;
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
                    throw new Exception("Unable to connect to host: " + _host);
                }
            }

            try
            {
                ssh.WaitFor("IRIS MFG Shell.*#", 5, isRegx: true);

                fire_status("Battery Test");
                BatteryJig.Write_SingleDIO(BatteryJig.Relays.BATT, true);
                Thread.Sleep(500);
                string data1 = ssh.WriteWait("show battery", "Level:", 3);
                //fire_status(data1);
                double volts1 = parseVolatge(data1);
                string msg1 = string.Format("Battery voltage before DUT power removed detected at {0}", volts1);
                fire_status(msg1);
                if (volts1 < 5.0)
                {
                    string emsg = string.Format("Battery power before DUT power removed too low.  Detected at {0}", volts1);
                    throw new Exception(emsg);
                }

                fire_status("DUT power off");
                BatteryJig.Write_SingleDIO(BatteryJig.Relays.DUT, false);
                Thread.Sleep(2000);

                string data2 = ssh.WriteWait("show battery", "Level:", 3);
                //fire_status(data2);
                double volts2 = parseVolatge(data2);
                string msg2 = string.Format("Battery voltage after DUT power removed detected at {0}", volts2);
                fire_status(msg2);
                if (volts2 < 4.0)
                {
                    string emsg = string.Format("Battery power after DUT power removed too low.  Detected at {0}", volts2);
                    throw new Exception(emsg);
                }

                fire_status("DUT power back on");
                BatteryJig.Write_SingleDIO(BatteryJig.Relays.DUT, true);

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


        double parseVolatge(string data)
        {

            Regex regx = new Regex(@"^Voltage:\s+(\d+\.\d+)", RegexOptions.Multiline);
            Match m = regx.Match(data);
            if (!m.Success && m.Groups.Count < 2)
                throw new Exception("Unable to parse volatge data.\r\nInput was:\r\n" + data);


            double value = Convert.ToDouble(m.Groups[1].Value);
            return value;
        }

        void fire_status(string msg)
        {
            Status_Event?.Invoke(this, msg);
        }

    }
}
