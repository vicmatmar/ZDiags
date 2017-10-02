using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace NIUtils
{
    class Options
    {
        [Option('r', "read_single", Required = false, DefaultValue =-1,
            HelpText = "Analog line number")]
        public int Linenum { get; set; }

        [Option('t', Required = false, DefaultValue = false,
            HelpText = "Continous")]
        public bool Continous { get; set; }

    }

}
