using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using System.Data;

namespace bus
{
    public class clsInternationalLicense
    {
        public int InternationalLicenseID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }

        public int DriverID { get; set; }

        public int ApplicationID { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public static DataTable GetAll()
        {
            return Data.clsInternationalLicense.GetAll();
        }


    }
}
