using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NIUtils
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

            if (options.Linenum >= 0)
            {
                while (true)
                {
                    double val = ZCommon.NIUtils.Read_SingelAi((uint)options.Linenum);
                    string msg = string.Format("A{0} = {1}", options.Linenum, val.ToString("G2"));
                    Console.WriteLine(msg);

                    if (!options.Continous)
                        break; ;
                }
            }

            return 0;
        }
    }
}
