using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace bus
{
    public class clsLicenseClass
    {
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        public clsLicenseClass(int licenseClassID, string className, string classDescription, byte minimumAllowedAge, byte defaultValidityLength, decimal classFees)
        {
            this.LicenseClassID = licenseClassID;
            this.ClassName = className;
            this.ClassDescription = classDescription;
            this.MinimumAllowedAge = minimumAllowedAge;
            this.DefaultValidityLength = defaultValidityLength;
            this.ClassFees = classFees;
        }

        public clsLicenseClass()
        {
            this.LicenseClassID = 0;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 0;
            this.DefaultValidityLength = 0;
            this.ClassFees = 0;
        }

        public static DataTable GetAll()
        {
            return Data.clsLicenseClass.GetAll();
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = "";
            string ClassDescription = "";
            byte MinimumAllowedAge = 0;
            byte DefaultValidityLength = 0;
            decimal ClassFees = 0;
            Data.clsLicenseClass.getLiecenseClassByID(LicenseClassID, ref ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees);
            return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
        }
    }
}
