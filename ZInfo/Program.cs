using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZCommon;

namespace ZInfo
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLine.Parser(s => { s.MutuallyExclusive = false; } );

            var isValid = parser.ParseArguments(args, options);
            if (!isValid)
            {
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return -1;
            }

            try
            {

                Console.WriteLine("Get Hub info for serial: " + options.SMT_Serial);


                using (CLStoreEntities cx = new CLStoreEntities())
                {
                    var lhs = cx.LowesHubs.Where(l => l.smt_serial == options.SMT_Serial).OrderByDescending(l => l.date).First();
                    Console.WriteLine("Hub last tested: " + lhs.date);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                return -2;
            }


            return 0;
        }
    }
}
