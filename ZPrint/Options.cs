using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;

namespace ZPrint
{
    class Options
    {
        [Option("zpl_file", Required = true,
            HelpText = "ZPL file")]
        public string ZPLFile { get; set; }

        [Option("printer_addrs", Required = true,
            HelpText = "Printer Address")]
        public string PrinterAddres { get; set; }

        [Option("smt_serial", Required = true, 
            HelpText = "Hub serial number")]
        public string SMTSerial { get; set; }

    }

}
