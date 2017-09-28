using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace ZBatt
{
    class Options
    {

        [Option("smt_serial", Required = true,
            HelpText = "SMT Serial")]
        public string SMT_Serial { get; set; }

        [Option('h', "host", Required = true,
            HelpText = "Host name")]
        public string Host { get; set; }

        [Option("calibrate_leds", Required = false, DefaultValue = false,
            HelpText = "Calibrate LED values")]
        public bool CalibrateLEDs { get; set; }

        [Option("disable_invalidate", Required = false, DefaultValue = false,
            HelpText = "Disables boot invalidation")]
        public bool DisableInvalidate { get; set; }

        [Option("no_jig_mode", Required = false, DefaultValue = false,
            HelpText = "Runs testd in No Jig Mode")]
        public bool NoJigMode { get; set; }

        [Option("disable_print_label", Required = false, DefaultValue = false,
            HelpText = "Disables printing label at the end of the test")]
        public bool PrintLabelDisabled { get; set; }

    }

}
