using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace ZDiags
{
    class SerialUtils
    {

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

    }
}
