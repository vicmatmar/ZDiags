using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCommon
{
    public class DataUtils
    {

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
