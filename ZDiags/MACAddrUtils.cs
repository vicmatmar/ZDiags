using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Core.Objects;
using System.Net.NetworkInformation;

namespace ZDiags
{
    class MACAddrUtils
    {
        public const long INVALID_MAC = 0;
        public const int INVALID_ID = -1;

        // Next Lowes block: 00:16:A2:05:02:00 - 00:16:A2:05:FE:FF
        // We may want to start at 0x0016A2050000 when we go live at Flex
        static long _block_start_addr = INVALID_MAC;
        static public long BlockStartAddr { get { return _block_start_addr; } set { _block_start_addr = value; } }

        static long _block_end_addr = INVALID_MAC;
        static public long BlockEndAddr { get { return _block_end_addr; } set { _block_end_addr = value; } }

        static string[] _station_macs;
        static public string[] StationMACS
        {
            get
            {
                if(_station_macs == null)
                    _station_macs = getMachinesUpMacAddress();

                return _station_macs;
            }
        }


        public static long GetNewMac()
        {
            long mac_out = INVALID_MAC;
            using (CLStoreEntities context = new CLStoreEntities())
            {
                ObjectParameter newmac = new ObjectParameter("newmac", typeof(long));
                //try
                {
                    context.GetNextMac(BlockStartAddr, BlockEndAddr, newmac);
                }
                //catch (Exception ex)
                //{
                //    string msg = ex.InnerException.Message;
                //    msg += ex.InnerException.StackTrace;
                //}
                mac_out = (long)newmac.Value;
            }

            if (mac_out == INVALID_MAC)
                throw new OverflowException("Unable to get a new MAC address");

            return mac_out;
        }

        public static bool Inrange(long macaddress)
        {
            if (macaddress >= BlockStartAddr && macaddress < BlockEndAddr)
                return true;
            else
                return false;
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

        public static void DeleteBlock()
        {
            using (CLStoreEntities context = new CLStoreEntities())
            {
                var addrs = context.MacAddresses.Where(m => m.MAC >= BlockStartAddr && m.MAC < BlockEndAddr);
                context.MacAddresses.RemoveRange(addrs);
                context.SaveChanges();
            }
        }

        static string[] getMachinesUpMacAddress()
        {
            List<string> macs = new List<string>();

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    var macphy = nic.GetPhysicalAddress();
                    string macstr = macphy.ToString();
                    if (macstr != "")
                        macs.Add(macstr);
                }
            }

            return macs.ToArray();
        }


        public static int ProductionSiteId()
        {
            using (CLStoreEntities cx = new CLStoreEntities())
            {
                foreach (string mac in StationMACS)
                {
                    var q = cx.StationSites.Where(s => s.StationMac == mac);
                    if (q.Any())
                    {
                        return q.Single().ProductionSiteId;
                    }
                }
            }

            throw new NullReferenceException("Machine no found as Station Site");
        }

        public static int StationSiteId()
        {
            using (CLStoreEntities cx = new CLStoreEntities())
            {
                foreach (string mac in StationMACS)
                {
                    var q = cx.StationSiteIds.Where(s => s.StationMac == mac);
                    if (q.Any())
                    {
                        return q.Single().Id;
                    }
                    else
                    {
                        // Add if this MAC is in the StationSite table
                        if(cx.StationSites.Where(s=>s.StationMac == mac).Any())
                        {
                            StationSiteId si = new ZDiags.StationSiteId();
                            si.StationMac = mac;

                            cx.StationSiteIds.Add(si);
                            cx.SaveChanges();

                            return cx.StationSiteIds.Where(s => s.StationMac == mac).Single().Id;
                        }
                    }
                }
            }

            throw new NullReferenceException("Machine no found as Station Site");
        }
    }
}