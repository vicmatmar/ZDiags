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
        public string Com_DUT { get; set; }

        [Option("com_ble", Required = false, 
            HelpText = "BLE Serial COM Port")]
        public string com_ble { get; set; }

        [Option("program_radios", Required = false, DefaultValue = false,
            HelpText = "Executes the \"Program Radios\" command")]
        public bool Program_Radios { get; set; }

        [Option("smt_serial", Required = true,
            HelpText = "SMT Serial")]
        public string smt_serial { get; set; }

        [Option("customer_IRIS", Required = false, MutuallyExclusiveSet ="custumer",
            HelpText = "Specifies IRIS as custumer")]
        public bool Custumer_IRIS { get; set; }
        [Option("customer_Amazone", Required = false, MutuallyExclusiveSet = "custumer",
            HelpText = "Specifies Amazone as custumer")]
        public bool Custumer_Amazone { get; set; }

        [Option("hw_version", Required = true,
            HelpText = "Board version")]
        public string HW_Version { get; set; }

    }
}
