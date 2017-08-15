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
        public string COM_DUT { get; set; }


        [Option("program_radios", Required = false, DefaultValue = true,
            HelpText = "Executes the Program Radios command")]
        public bool Program_Radios { get; set; }

    }
}
