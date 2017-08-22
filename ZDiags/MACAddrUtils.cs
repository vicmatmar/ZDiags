using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data.Entity.Core.Objects;

namespace ZDiags
{
    class MACAddrUtils
    {

        long _block_start_addr = 0x0016A2048100; //‭97207484672‬
        public long BlockStartAddr { get { return _block_start_addr; } set { _block_start_addr = value; } }

        long _block_end_addr = 0x0016A204FF00;
        public long BlockEndAddr { get { return _block_end_addr; } set { _block_end_addr = value; } }

        public const long INVALID_MAC = 0;
        public const int INVALID_ID = -1;


        public long GetNewMac()
        {
            long mac_out = INVALID_MAC;
            using (CLStoreEntities context = new CLStoreEntities())
            {
                ObjectParameter newmac = new ObjectParameter("newmac", typeof(long));
                try
                {
                    context.GetNextMac(BlockStartAddr, BlockEndAddr, newmac);
                }
                catch (Exception ex)
                {
                    string msg = ex.InnerException.Message;
                    msg += ex.InnerException.StackTrace;
                }
                mac_out = (long)newmac.Value;
            }

            if (mac_out == INVALID_MAC)
                throw new OverflowException("Unable to get a new MAC address");

            return mac_out;
        }

        public static int GetMacId(long mac)
        {
            int id = INVALID_ID;
            using (CLStoreEntities cx = new CLStoreEntities())
            {
                var q = cx.MacAddresses.Where(m => m.MAC == mac);
                if (q.Any())
                {
                    var ma = q.Single();
                    id = ma.Id;
                }

            }

            if (id == MACAddrUtils.INVALID_ID)
                throw new Exception("Unable to find id for mac = " + mac.ToString());

            return id;
        }

        public static string LongToStr(long value, string delimeter = ":")
        {
            string macstr = string.Format("{0:X12}", value);
            var list = Enumerable
                .Range(0, macstr.Length / 2)
                .Select(i => macstr.Substring(i * 2, 2))
                .ToList();
            var macfrt = string.Join(delimeter, list);

            return macfrt;

        }

        public void DeleteBlock()
        {
            using (CLStoreEntities context = new CLStoreEntities())
            {
                var addrs = context.MacAddresses.Where(m => m.MAC >= BlockStartAddr && m.MAC < BlockEndAddr);
                context.MacAddresses.RemoveRange(addrs);
                context.SaveChanges();
            }
        }

    }
}
