using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace ZDiags
{
    class SerialCOM : IDisposable
    {
        SerialPort _port;

        public delegate void StatusHandler(object sender, string data);
        public event StatusHandler DataReceived;

        string _data = "";
        public string Data
        {
            get { return _data; }
            set
            {
//                lock (_data)
                {
                    _data = value;
                }
            }
        }

        bool _isDisposed = false;
        public bool IsDisposed { get { return _isDisposed; } }

        public SerialCOM(string portName)
        {
            _port = new SerialPort()
            {
                PortName = portName,
                BaudRate = 115200,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
                Handshake = Handshake.XOnXOff
            };
            _port.DataReceived += _port_DataReceived;
            _port.Open();

        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;

            string data = sp.ReadExisting();
            lock (_data)
            {
                _data += data;
            }
            DataReceived?.Invoke(this, data);
        }

        public void WaitForPrompt(string prompt = "#")
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    WriteWait("", prompt, 1, clear_data: false);
                    break;
                }
                catch (TimeoutException) { }
                WriteWait("", prompt, 3, clear_data:false);
            }

        }
        public void WriteWait(string cmd, string exp, int timeout_sec = 1, bool isRegx = false, bool clear_data = true)
        {
            WriteLine(cmd);
            Thread.Sleep(150);
            try
            {
                WaitFor(str: exp, timeout_sec: timeout_sec, isRegx: isRegx, clear_data: clear_data);
            }
            catch (TimeoutException ex)
            {
                string msg = string.Format("Sent: {0}\r\n", cmd);
                msg += ex.Message;
                throw new TimeoutException(msg);
            }
        }

        public void WriteLine(string txt = "")
        {
            _port.WriteLine(txt);
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
        /// <returns>The position of the last occurance + the size of the string</returns>
        public int WaitFor(string str, int timeout_sec = 1, int sample_ms = 500,
            bool isRegx = false, RegexOptions regxopt = RegexOptions.Singleline, int startIndex = 0, bool clear_data = true)
        {
            DateTime start = DateTime.Now;
            int index = -1;
            while (true)
            {
                string data = "";
                try
                {
                    data = Data.Substring(startIndex);
                }
                catch (System.NullReferenceException) { }

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
                    index = data.LastIndexOf(str);
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
                        timeout_sec, str, data);
                    throw new TimeoutException(msg);
                }
            }

            if (clear_data)
                Data = "";

            return index;
        }


        public void Dispose()
        {
            if (_port != null)
            {
                _port.Close();
                _port.Dispose();
                _port = null;
            }
            _isDisposed = true;
        }
    }
}
