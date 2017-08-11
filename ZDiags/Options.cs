using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace ZDiags
{
    class Options
    {

        [Option("com_dut", Required = false,
            HelpText = "DUT Serial COM Port")]
        public string Com_dut { get; set; }

    }
}
