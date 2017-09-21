using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZCommon;

namespace ZBatt
{

    public class LED_Group
    {
        LED[] _leds;
        public LED[] LEDS { get { return _leds; } }

        public LED_Group(LED[] leds)
        {
            _leds = leds;
        }

        public LED Get(string color_name)
        {
            foreach (LED led in _leds)
            {
                if (led.ColorName == color_name)
                    return led;
            }

            throw new ArgumentException("No led with " + color_name + " color name");
        }

        uint[] get_linenums()
        {
            uint[] linemums = new uint[_leds.Length];
            for (int i = 0; i < linemums.Length; i++)
                linemums[i] = _leds[i].AILineNum;
            return linemums;
        }

        public double[] ReadValues()
        {
            uint[] linemums = get_linenums();
            double[] values = NIUtils.Read_MultiAi(linemums);
            //for (int i = 0; i < linemums.Length; i++)
            //    _leds[i].LastValue = values[i];

            return values;
        }

        public bool areOn
        {
            get
            {
                bool ison = true;
                double[] values = ReadValues();
                for (int i = 0; i < values.Length; i++)
                {
                    ison &= _leds[i].getIsOn(values[i]);
                }
                return ison;
            }
        }

        public bool areOff
        {
            get { return !areOn; }
        }

        public bool arePattern(bool[] pattern)
        {
            bool ispattern = true;
            double[] values = ReadValues();
            for (int i = 0; i < values.Length; i++)
            {
                ispattern &= (_leds[i].getIsOn(values[i]) == pattern[i]);
            }
            return ispattern;

        }

        public void ResetMaxValues()
        {
            foreach (LED led in _leds)
                led.ResetMaxVal();
        }

    }


    public class LED
    {
        uint _ai_linenum = 0;
        public uint AILineNum { get { return _ai_linenum; } }

        double _off_val = double.MinValue;
        public double OffVal { get { return _off_val; } set { _off_val = value; } }

        double _on_val = double.MaxValue;
        public double OnVal { get { return _on_val; } set { _on_val = value; } }


        const string _fs_control_path_fmt = "/sys/class/leds/{0}/brightness";
        string _color_name;
        public string ColorName { get { return _color_name; } set { _color_name = value; } }

        double _min_value = double.MaxValue;
        public double MinValue { get { return _min_value; } }

        double _mid_value;
        public double MidValue { get { return _mid_value; } }

        double _max_value = double.MinValue;
        public double MaxValue { get { return _max_value; } }


        public LED(uint ai, double off_val, double on_val, string color_name = null)
        {
            _ai_linenum = ai;

            _off_val = off_val;
            _on_val = on_val;

            if (_off_val >= _on_val)
                throw new Exception("on value should be greater than off value");
            _mid_value = (_on_val - _off_val) / 2 + off_val;

            _color_name = color_name;
        }


        public void ResetMaxVal()
        {
            _max_value = double.MinValue;
        }

        public void ResetMinVal()
        {
            _min_value = double.MinValue;
        }

        double _last_value = double.MinValue;
        public double LastValue
        {
            get { return _last_value; }
            set {

                _last_value = value;

                if (_last_value > _max_value)
                    _max_value = value;
                if (_last_value < _min_value)
                    _min_value = value;

            }
        }

        /// <summary>
        /// Reads voltage vallue from NI box
        /// </summary>
        public double Value
        {
            get {
                double value = NIUtils.Read_SingelAi(_ai_linenum);

                _last_value = value;

                return value;
            }

            set { _last_value = value; }
        }

        public void Turn(bool value, SSHUtil ssh)
        {
            if (ColorName == null)
                throw new Exception("Color name not set");
            string path = string.Format(_fs_control_path_fmt, ColorName);

            string cmd;
            if (value)
                cmd = "echo 1 > " + path;
            else
                cmd = "echo 0 > " + path;

            ssh.WriteLine(cmd);
        }


        public bool getIsOn(double value)
        {
            if (value > _mid_value)
                return true;
            else
                return false;
        }


        /// <summary>
        /// This function causes a voltage read
        /// </summary>
        public bool isOn
        {
            get
            {
                if (Value > _mid_value)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// This function causes a voltage read
        /// </summary>
        public bool isOff
        {
            get
            {
                return !isOn;
            }
        }

        public bool isBlicking(int timeout_sec = 1)
        {
            bool detected_high = false;
            bool detected_low = false;

            DateTime start = DateTime.Now;
            while (true)
            {
                if (isOn)
                    detected_high = true;
                else
                    detected_low = true;

                if (detected_high && detected_low)
                    return true;

                TimeSpan ts = DateTime.Now - start;
                if (ts > new TimeSpan(0, 0, timeout_sec))
                    break;
            }

            return false;
        }
    }
}
