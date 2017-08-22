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

        string _dat_log_loc;
        FileStream _fs;

        string _data;
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }


        bool _isDisposed = false;
        public bool IsDisposed { get { return _isDisposed; } }

        public SerialCOM(string portName)
        {
            _dat_log_loc = portName + ".txt";
            _fs = new FileStream(_dat_log_loc, FileMode.Create, FileAccess.Write, FileShare.Read);

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
            _data += data;

            _fs.Write(Encoding.UTF8.GetBytes(data), 0, data.Length);
            _fs.FlushAsync();
            //_fs.Flush();
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
        public int WaitForStr(string str, int timeout_sec = 1, int sample_ms = 500, 
            bool isRegx = false, RegexOptions regxopt= RegexOptions.Singleline, int startIndex = 0, bool clear_data = true)
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
                    if(match.Success)
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
                    string msg = string.Format("Timeout after {0} sec.\r\nWait for: {1}.\r\nData was: {2}",
                        timeout_sec, str, data);
                    throw new Exception(msg);
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

            if (_fs != null)
            {
                _fs.Close();
                _fs.Dispose();
                _fs = null;
            }

            _isDisposed = true;
        }
    }
}
