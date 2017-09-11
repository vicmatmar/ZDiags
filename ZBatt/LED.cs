using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZCommon;

namespace ZBatt
{
    public class LED
    {
        uint _ai_linenum = 0;

        double _off_val = double.MinValue;
        public double OffVal { get { return _off_val; } set { _off_val = value; } }

        double _on_val = double.MaxValue;
        public double OnVal { get { return _on_val; } set { _on_val = value; } }

        double _mid_val_point;

        const string _fs_control_path_fmt = "/sys/class/leds/{0}/brightness";
        string _color_name;
        public string ColorName {  get { return _color_name; } set { _color_name = value; } }

        public LED(uint ai, double off_val, double on_val, string color_name = null)
        {
            _ai_linenum = ai;

            _off_val = off_val;
            _on_val = on_val;

            if (_off_val >= _on_val)
                throw new Exception("on value should be greater than off value");
            _mid_val_point = (_on_val - _off_val) / 2 + off_val;

            _color_name = color_name;
        }

        double _last_value = double.MinValue;
        public double LastValue
        {
            get { return _last_value; }
        }

        public double Value
        {
            get { return NIUtils.Read_SingelAi(_ai_linenum); }
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


        public bool isOn
        {
            get
            {
                _last_value = Value;
                if (_last_value > _mid_val_point)
                    return true;
                else
                    return false;
            }
        }

        public bool isOff
        {
            get
            {
                _last_value = Value;
                if (_last_value < _mid_val_point)
                    return true;
                else
                    return false;
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
