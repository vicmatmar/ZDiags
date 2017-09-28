using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZCommon;

namespace ZPrint
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();
            var parser_options = new CommandLine.ParserSettings { MutuallyExclusive = true };
            var parser = new CommandLine.Parser(parser_options);

            var isValid = parser.ParseArguments(args, options);
            if (!isValid)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return -1;
            }

             DataUtils.PrintHubLabel(
                options.SMTSerial,
                options.ZPLFile,
                options.PrinterAddres
                );

            return 0;
        }
    }
}
