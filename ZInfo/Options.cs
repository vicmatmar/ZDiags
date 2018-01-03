using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace ZInfo
{
    class Options
    {
        [Option('s', "serial", Required = false, 
            HelpText = "Serial number")]
        public string SMT_Serial { get; set; }

    }
}
