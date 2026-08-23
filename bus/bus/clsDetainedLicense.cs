using System;
using System.Data;

namespace bus
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }

        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MinValue;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }

        private clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees,
            int createdByUserID, bool isReleased, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        {
            this.DetainID = detainID;
            this.LicenseID = licenseID;
            this.DetainDate = detainDate;
            this.FineFees = fineFees;
            this.CreatedByUserID = createdByUserID;
            this.IsReleased = isReleased;
            this.ReleaseDate = releaseDate;
            this.ReleasedByUserID = releasedByUserID;
            this.ReleaseApplicationID = releaseApplicationID;

            Mode = enMode.Update;
        }

        public static clsDetainedLicense Find(int detainID)
        {
            int licenseID = -1, createdByUserID = -1, releasedByUserID = -1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.Now, releaseDate = DateTime.MinValue;
            decimal fineFees = 0;
            bool isReleased = false;

            if (Data.clsDetainedLicense.GetDetainedLicenseInfoByID(detainID, ref licenseID, ref detainDate,
                ref fineFees, ref createdByUserID, ref isReleased, ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainedLicense(detainID, licenseID, detainDate, fineFees, createdByUserID,
                    isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            return null;
        }

        public static clsDetainedLicense FindByLicenseID(int licenseID)
        {
            int detainID = -1, createdByUserID = -1, releasedByUserID = -1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.Now, releaseDate = DateTime.MinValue;
            decimal fineFees = 0;
            bool isReleased = false;

            if (Data.clsDetainedLicense.GetDetainedLicenseInfoByLicenseID(licenseID, ref detainID, ref detainDate,
                ref fineFees, ref createdByUserID, ref isReleased, ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainedLicense(detainID, licenseID, detainDate, fineFees, createdByUserID,
                    isReleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            return null;
        }

        public static bool IsLicenseDetained(int licenseID)
        {
            return Data.clsDetainedLicense.IsLicenseDetained(licenseID);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            return Data.clsDetainedLicense.GetAllDetainedLicenses();
        }

        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                this.DetainID = Data.clsDetainedLicense.InsertDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
                return (this.DetainID != -1);
            }
            return false;
        }

        public bool Release(int releasedByUserID, int releaseApplicationID)
        {
            return Data.clsDetainedLicense.ReleaseDetainedLicense(this.DetainID, releasedByUserID, releaseApplicationID);
        }
    }
}
