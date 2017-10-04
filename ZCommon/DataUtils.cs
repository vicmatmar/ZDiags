using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCommon
{
    public class DataUtils
    {

        public static void PrintHubLabel(string hub_serial, string zplfile, string printer_addres)
        {
            string hub_mac;
            string hub_id;

            using (CLStoreEntities cx = new CLStoreEntities())
            {
                var lhs = cx.LowesHubs.Where(lh => lh.smt_serial == hub_serial).OrderByDescending(lh => lh.date).First();
                hub_mac = lhs.MacAddress.MAC.ToString("X12");
                hub_id = lhs.hub_id;
            }

            // get the zpl file
            System.IO.FileStream fs_zpl = new System.IO.FileStream(zplfile, System.IO.FileMode.Open);
            byte[] zpl_file_bytes = new byte[fs_zpl.Length];
            fs_zpl.Read(zpl_file_bytes, 0, zpl_file_bytes.Length);

            // Open connection
            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
            int port = 9100;
            client.Connect(printer_addres, port);

            // Write ZPL String to connection
            System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());

            string short_serial = hub_serial;
            if (short_serial.StartsWith("CLT"))
                short_serial = short_serial.Substring(3);
            string barcode_serial = 
                ">;" + 
                short_serial.Substring(0, short_serial.Length-1) +
                ">6" +
                short_serial[short_serial.Length-1];
            object[] data = new object[4] { short_serial, barcode_serial, hub_mac, hub_id };

            string label = string.Format(System.Text.Encoding.UTF8.GetString(zpl_file_bytes), data);

            writer.Write(label);
            writer.Flush();

            // Close Connection
            writer.Close();
            client.Close();
        }

        public static string OperatorName(string txt)
        {
            if (txt == null || txt == "")
            {
                throw new Exception("Invalid name: " + txt);
            }

            string[] names = txt.Trim().ToLower().Split(new char[] { ' ' });
            if(names.Length != 2)
            {
                throw new Exception("Invalid name: " + txt);
            }

            string firstname = names[0].Trim();
            string lastname = names[1].Trim();

            string fullname = firstname[0].ToString().ToUpper() + firstname.Substring(1) + " " +
                lastname[0].ToString().ToUpper() + lastname.Substring(1);

            return fullname;

        }

        public static int OperatorId(string tester)
        {
            string name = OperatorName(tester);
            int id = -1;
            using (CLStoreEntities cx = new CLStoreEntities())
            {
                var q = cx.Operators.Where(t => t.Name == name);
                if(!q.Any())
                {
                    Operator td = new Operator();
                    td.Name = name;

                    cx.Operators.Add(td);
                    cx.SaveChanges();
                }
                id = q.Single().Id;
            }
            return id;
        }
    }
}
