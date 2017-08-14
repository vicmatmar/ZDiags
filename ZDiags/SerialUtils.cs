using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Ports;
//using System.Text;

namespace ZDiags
{
    class SerialUtils: IDisposable
    {
        SerialPort _port;

        string _dat_log_loc;
        FileStream _fs;

        string _data;
        public string Data {
            get
            {
                return _data;
            }
        }
        

        public SerialUtils(string portName)
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
            //_fs.FlushAsync();
            _fs.Flush();
        }

        public void Open()
        {
            if(!_port.IsOpen)
                _port.Open();
        }

        public static string WriteLine(string txt, string portname)
        {
            String buffer = String.Empty;

            SerialPort port = new SerialPort(portname);

            port.Open();
            try
            {
                port.WriteLine(txt);

                while (port.BytesToRead > 0)
                {
                    byte[] bytes = new byte[port.BytesToRead];
                    port.Read(bytes, 0, bytes.Length);
                    buffer += bytes;

                }
            }
            finally
            {
                port.Close();
            }

            return buffer;
        }

        public void Dispose()
        {
            if (_port != null)
            {
                _port.Close();
                _port.Dispose();
            }

            if(_fs != null)
            {
                _fs.Close();
                _fs.Dispose();
            }
        }
    }
}
