using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal class clsDataAccessSetting
    {

        public static string _connectionString;

        protected clsDataAccessSetting()
        {
            _connectionString = "Server =.; Database = DVLD; Trusted_Connection = True; TrustServerCertificate = True;";
        }



    }
}
