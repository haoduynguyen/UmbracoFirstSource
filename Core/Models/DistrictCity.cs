using Customs.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DistrictCity
    {
        public static DataTable GetDistrict()
        {
            DataAccess db = new DataAccess();
            string queryString = "SELECT * FROM districtCity";
            return db.Query(queryString);
        }

        public static DataTable GetCity()
        {
            DataAccess db = new DataAccess();
            string queryString = "SELECT * FROM city";
            return db.Query(queryString);
        }
    }
}
