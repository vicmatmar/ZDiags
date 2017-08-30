using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;
using Renci.SshNet;
using ZCommon;

namespace ZBatt
{
    class Program
    {
        enum Relays : uint { DUT = 0, BATT };
        enum Sensors : uint { BUZZER_AUDIO = 0, GREEN_LIGHT, RED_LIGHT, YELLOW_LIGHT };

        static StreamWriter _sw;
        static StreamReader _sr;

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

            try
            {
                string host = options.Host;
                ConnectionInfo coninfo = new ConnectionInfo(host, "root", new PasswordAuthenticationMethod("root", ""));

                using (var ssh = new SshClient(coninfo))
                {
                    ssh.Connect();

                    ShellStream shellStream = ssh.CreateShellStream("SSH Shell", 80, 24, 800, 600, 1024);

                    _sw = new StreamWriter(shellStream);
                    _sw.AutoFlush = true;
                    _sr = new StreamReader(shellStream);

                    verifyAlive();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                Console.WriteLine(ex.StackTrace);

                return -2;
            }

            return 0;
        }

        static void set_all_relays(bool value)
        {
            Array relays = Enum.GetValues(typeof(Relays));
            foreach (uint relay in relays)
            {
                NIUtils.Write_SingleDIO(relay, value);
            }
        }
        static void verifyAlive()
        {
            _sw.WriteLine("show battery");
            waitFor("Battery Information is not available", 10);
        }

        static void waitFor(string str, int timeout_sec = 3)
        {
            string data = "";
            DateTime start = DateTime.Now;
            while (true)
            {
                data += _sr.ReadToEnd();
                if (data.Contains(str))
                {
                    break;
                }
                Thread.Sleep(250);

                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalSeconds > timeout_sec)
                {
                    string msg = string.Format("Timeout after {0} sec.\r\nWait for: {1}\r\nData was: {2}",
                        timeout_sec, str, data);
                    throw new TimeoutException(msg);
                }

            }
        }
    }
}
