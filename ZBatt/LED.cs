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
        double _on_val = double.MaxValue;
        double _mid_val_point;

        public LED(uint ai, double off_val, double on_val)
        {
            _ai_linenum = ai;

            _off_val = off_val;
            _on_val = on_val;

            if (_off_val >= _on_val)
                throw new Exception("on value should be greater than off value");
            _mid_val_point = (_on_val - _off_val) / 2 + off_val;
        }

        public bool isOn()
        {
            double val = NIUtils.Read_SingelAi(_ai_linenum);
            if (val > _mid_val_point)
                return true;
            else
                return false;
        }

        public bool isOff()
        {
            double val = NIUtils.Read_SingelAi(_ai_linenum);
            if (val < _mid_val_point)
                return true;
            else
                return false;
        }

        public bool isBlicking(int timeout_sec = 1)
        {
            bool detected_high = false;
            bool detected_low = false;

            DateTime start = DateTime.Now;
            while (true)
            {
                if (isOn())
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
