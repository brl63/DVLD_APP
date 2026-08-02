using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace bus
{
    public class clsCountries
    {
        public int id { get; set; }
        public string name { get; set; }

        public static DataTable GetAllCountries()
        {
            return Data.clsCountries.GetAllCountries();
        }

        public static string GetCountryName(int id)
        {
            return Data.clsCountries.GetCountryName(id);
        }
    }
}
