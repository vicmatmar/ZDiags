using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDiags
{
/*
Model 3 
    000 = HUB520 (Iris 1 Hub - reserved)
    001 = IH200 (Iris 2 Hub)
HW Version 3 
    000 = version 0
Date MM 4 
    0000 = Jan
    1100 = Dec
Date DD 5 
    00001 = 1st
    11111 = 31st
Date YY 5 
    00000 = 2000
    01111 = 2015
Factory ID 5 
    ID of the factory where the hub was produced (0..31)
Test Station ID 7 
    ID of the test station used to validate the hub (0..127)
Tester Number 10 
    ID of the tester that verified the hub (0..1023)
Example:
    Model:  = 001 IH200
    HW Version:  = 000 000
    Date MM:  = 0001 Feb
    Date DD:  = 00010 02
    Date YY:  = 01111 2015
    Factory ID:  = 00010 02
    Test Station ID:  = 0001001 09
    Tester Number:  = 0001110010 114
    001 000 0001 00010 01111 00010 0001001 0001110010 = 554382402674
*/
    class LowesSerial
    {
        public enum Model { HUB520=0, IH200 };

        const int BITLEN = 42;



        // Lowes serial is 42bit long
        public static long GetSerial(Model model, byte hw_version, DateTime datetime, byte factory, byte test_station, short tester)
        {
            long serial = 0;

            int shiftby = BITLEN - 3;
            serial = ((long)model << shiftby);

            shiftby -= 3;
            serial |= ((long)hw_version << shiftby);

            shiftby -= 4;
            serial |= ((long)(datetime.Date.Month-1) << shiftby);

            shiftby -= 5;
            serial |= ((long)(datetime.Date.Day) << shiftby);

            shiftby -= 5;
            serial |= ((long)(datetime.Date.Year - 2000) << shiftby);

            shiftby -= 5;
            serial |= ((long)(factory) << shiftby);

            shiftby -= 7;
            serial |= ((long)(test_station) << shiftby);

            serial |= (long)tester;

            return serial;
        }
    }
}
