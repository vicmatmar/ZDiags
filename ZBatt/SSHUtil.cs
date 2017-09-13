using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using Renci.SshNet;

namespace ZBatt
{
    public class SSHUtil:IDisposable
    {
        string _host;
        string _user = "root";
        string _password = "";

        SshClient _sshclient;
        StreamWriter _sw;
        StreamReader _sr;

        string _data = "";
        public string Data
        {
            get { return _data; }
            set
            {
                lock (_data)
                {
                    _data = value;
                }
            }
        }

        public SSHUtil(string host)
        {
            _host = host;
        }

        public bool IsConnected
        {
            get { return _sshclient.IsConnected; }
        }

        public void Connect()
        {
            ConnectionInfo coninfo = new ConnectionInfo(_host, _user, new AuthenticationMethod[] {
                new PasswordAuthenticationMethod(_user, _password) });

            _sshclient = new SshClient(coninfo);
            _sshclient.Connect();

            ShellStream shellStream = _sshclient.CreateShellStream("SSH Shell", 80, 24, 800, 600, 1024);

            _sw = new StreamWriter(shellStream);
            _sw.AutoFlush = true;
            _sr = new StreamReader(shellStream);
        }

        public string ReadToEnd()
        {
            return _sr.ReadToEnd();
        }

        public string WriteWait(string cmd, string exp, int timeout_sec = 1, 
            bool isRegx = false, bool clear_data = true, RegexOptions regxopt = RegexOptions.Singleline)
        {
            WriteLine(cmd);
            Thread.Sleep(150);
            string data = "";
            try
            {
                data = WaitFor(str: exp, timeout_sec: timeout_sec, 
                    isRegx: isRegx, clear_data: clear_data, regxopt: regxopt);
            }
            catch (TimeoutException ex)
            {
                string msg = string.Format("Sent: {0}\r\n", cmd);
                msg += ex.Message;
                throw new TimeoutException(msg);
            }

            return data;
        }

        public void WriteLine(string txt)
        {
            _sw.WriteLine(txt);
        }

        /// <summary>
        /// Waits for the a string in the serial data
        /// </summary>
        /// <param name="str">String to wait for</param>
        /// <param name="timeout_sec">How long to wait in secs</param>
        /// <param name="sample_ms">How often to look at the data</param>
        /// <param name="startIndex">Where to start looking for</param>
        /// <param name="clear_data">Clears all serial data</param>
        /// <param name="isRegx">Whether to treat str as a regx</param>
        /// <param name="regxopt">regulat exp options</param>
        /// <returns>All data to this match</returns>
        public string WaitFor(string str, int timeout_sec = 1, int sample_ms = 500,
            bool isRegx = false, RegexOptions regxopt = RegexOptions.Singleline, int startIndex = 0, bool clear_data = true)
        {
            DateTime start = DateTime.Now;
            int index = -1;
            while (true)
            {
                Data += _sr.ReadToEnd();

                if (isRegx)
                {
                    Match match = Regex.Match(Data, str, regxopt);
                    if (match.Success)
                    {
                        index = match.Index + match.Length;
                        break;
                    }
                }
                else
                {
                    index = Data.LastIndexOf(str);
                    if (index >= 0)
                    {
                        index = index + str.Length + 1;
                        break;
                    }
                }
                Thread.Sleep(sample_ms);

                TimeSpan ts = DateTime.Now - start;
                if (ts.TotalSeconds > timeout_sec)
                {
                    string msg = string.Format("Timeout after {0} sec.\r\nWait for: {1}\r\nData was: {2}",
                        timeout_sec, str, Data);
                    throw new TimeoutException(msg);
                }
            }

            string data = Data;
            if (clear_data)
                Data = "";

            return data;
        }

        public void Dispose()
        {
            if(_sshclient != null) {
                _sshclient.Disconnect();
                _sshclient.Dispose();
            }
        }
    }
}
