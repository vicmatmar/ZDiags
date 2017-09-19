using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCommon
{
    public class DataUtils
    {

        public static string TesterName(string txt)
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

        public static int TesterId(string tester)
        {
            string name = TesterName(tester);
            int id = -1;
            using (CLData.CLStoreEntities cx = new CLData.CLStoreEntities())
            {
                var q = cx.Testers.Where(t => t.Name == name);
                if(!q.Any())
                {
                    int last_id = cx.Testers.OrderByDescending(t => t.Id).FirstOrDefault().Id;

                    CLData.Tester td = new CLData.Tester();
                    td.Name = name;
                    td.Active = true;
                    td.Id = last_id + 1;
                    td.CreateDate = DateTime.Now;

                    cx.Testers.Add(td);
                    cx.SaveChanges();
                }
                id = q.Single().Id;
            }
            return id;
        }
    }
}
