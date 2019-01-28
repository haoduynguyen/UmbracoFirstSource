using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SubscriberEmail
    {
        public int AddEmail(string email)
        {
            DataAccess model = new DataAccess();
            Dictionary<string, object> dataInsert = new Dictionary<string, object>();
            dataInsert.Add("email", email);
            dataInsert.Add("ip", PathHelper.GetIP());
            int id = model.InsertTable("mySubscriberEmail", dataInsert);
            return id;
        }

        public DataTable GetAllEmail()
        {
            DataAccess db = new DataAccess();
            string queryString = "SELECT * FROM mySubscriberEmail Order By subscriberId Desc";
            return db.Query(queryString);
        }
    }
}
