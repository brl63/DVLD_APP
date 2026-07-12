using System;

namespace Data
{
    public class clsInternationalLicense 
    {
        public int InternationalLicenseID { get; set; }

       public int IssuedUsingLocalLicenseID { get; set; }
       public DateTime ExpirationDate { get; set; }
       public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }
    }
}

