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

        [Option('h', "host", Required = true,
            HelpText = "Host name")]
        public string Host { get; set; }

        [Option("calibrate_leds", Required = false, DefaultValue = false,
            HelpText = "Calibrate LED values")]
        public bool CalibrateLEDs { get; set; }

        [Option("disable_led_test", Required = false, DefaultValue = false,
            HelpText = "Disables LED values")]
        public bool DisableLEDTest { get; set; }

        [Option("disable_invalidate", Required = false, DefaultValue = false,
            HelpText = "Disables boot invalidation")]
        public bool DisableInvalidate { get; set; }

    }

}
