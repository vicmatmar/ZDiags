using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZCommon;

namespace ZBatt
{
    public class BatteryJig
    {
        public enum Relays : uint { BATT = 0, DUT };

        public enum Sensors : uint { RED_LIGHT = 0, GREEN_LIGHT, YELLOW_LIGHT };

        public static void Set_all_relays(bool value)
        {
            Array relays = Enum.GetValues(typeof(Relays));
            foreach (uint relay in relays)
            {
                NIUtils.Write_SingleDIO(relay, value);
            }
        }

        public static void Write_SingleDIO(Relays relay, bool value)
        {
            NIUtils.Write_SingleDIO((uint)relay, value);
        }

        public static double Read_SingelAi(Sensors sensor)
        {
            return NIUtils.Read_SingelAi((uint)sensor);
        }


    }
}
