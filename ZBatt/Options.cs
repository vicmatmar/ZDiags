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

    }
}
