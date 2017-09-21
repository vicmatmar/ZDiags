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
        public string SMT_Serial { get; set; }

        [Option("customer_IRIS", Required = false, MutuallyExclusiveSet ="customer",
            HelpText = "Specifies IRIS as custumer")]
        public bool Custumer_IRIS { get; set; }
        [Option("customer_Amazone", Required = false, MutuallyExclusiveSet = "customer",
            HelpText = "Specifies Amazone as custumer")]
        public bool Custumer_Amazone { get; set; }

        [Option("hw_version", Required = true,
            HelpText = "Board version")]
        public int HW_Version { get; set; }

        [Option("tester", Required = true, 
            HelpText = "Tester Name LastName")]
        public string Tester { get; set; }

        [Option("hub_ip_addr", Required = false,
            HelpText = "IP to assigned to hub")]
        public string Hub_IP { get; set; }

    }
}
