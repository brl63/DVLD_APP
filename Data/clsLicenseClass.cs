using System;

namespace Data
{
    public class clsLicenseClass
    {
        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal Fee { get; set; }
    }
}
