using System;
using System.Data;
using Data;

namespace bus
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }

        public clsDriver DriverInfo { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;

            Mode = enMode.AddNew;
        }

        private clsLicense(int licenseID, int applicationID, int driverID, int licenseClassID,
            DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive)
        {
            this.LicenseID = licenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseClassID = licenseClassID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;

            this.DriverInfo = clsDriver.FindByDriverID(driverID);
            this.LicenseClassInfo = clsLicenseClass.Find(licenseClassID);

            Mode = enMode.Update;
        }

        // ================= Private Methods =================
        private bool _AddNewLicense()
        {
            this.LicenseID = Data.clsLicense.InsertLicense(
                this.ApplicationID,
                this.DriverID,
                this.LicenseClassID,
                this.IssueDate,
                this.ExpirationDate,
                this.Notes,
                this.PaidFees,
                this.IsActive);

            return (this.LicenseID != -1);
        }

        private bool _UpdateLicense()
        {
            return Data.clsLicense.UpdateLicense(
                this.LicenseID,
                this.ApplicationID,
                this.DriverID,
                this.LicenseClassID,
                this.IssueDate,
                this.ExpirationDate,
                this.Notes,
                this.PaidFees,
                this.IsActive);
        }

        // ================= Public Static Methods =================
        public static clsLicense Find(int licenseID)
        {
            int applicationID = -1, driverID = -1, licenseClassID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = false;

            if (Data.clsLicense.GetLicenseByID(licenseID, ref applicationID, ref driverID,
                ref licenseClassID, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive))
            {
                return new clsLicense(licenseID, applicationID, driverID, licenseClassID,
                    issueDate, expirationDate, notes, paidFees, isActive);
            }

            return null;
        }

        public static clsLicense FindByDriverID(int driverID)
        {
            int licenseID = -1, applicationID = -1, licenseClassID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = false;

            if (Data.clsLicense.GetLicenseByDriverID(driverID, ref licenseID, ref applicationID,
                ref licenseClassID, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive))
            {
                return new clsLicense(licenseID, applicationID, driverID, licenseClassID,
                    issueDate, expirationDate, notes, paidFees, isActive);
            }

            return null;
        }

        public static DataTable GetAll()
        {
            return Data.clsLicense.GetAllLicenses();
        }

        public static DataTable GetLocalDrivingLicenses()
        {
            return Data.clsLicense.GetLocalDrivingLicenses();
        }

        public static DataTable GetLicensesByDriverID(int driverID)
        {
            return Data.clsLicense.GetLicensesByDriverID(driverID);
        }

        public static int GetActiveLicenseIDByApplicationID(int applicationID)
        {
            return Data.clsLicense.GetActiveLicenseIDByApplicationID(applicationID);
        }

        public static bool IsLicenseExists(int licenseID)
        {
            return Data.clsLicense.IsLicenseExists(licenseID);
        }

        public static bool IsLicenseActive(int licenseID)
        {
            return Data.clsLicense.IsLicenseActive(licenseID);
        }

        public static bool CheckActiveLicenseByClass(int driverID, int classID)
        {
            return Data.clsLicense.CheckActiveLicenseByClass(driverID, classID);
        }

        public static bool DeactivateAllLicensesByDriverID(int driverID)
        {
            return Data.clsLicense.DeactivateAllLicensesByDriverID(driverID);
        }

        public static bool Deactivate(int licenseID)
        {
            return Data.clsLicense.Deactivate(licenseID);
        }

        public static bool Activate(int licenseID)
        {
            return Data.clsLicense.Activate(licenseID);
        }

        public static bool ChangeLicenseStatus(int licenseID, bool newStatus)
        {
            return Data.clsLicense.ChangeLicenseStatus(licenseID, newStatus);
        }

        public static bool Delete(int licenseID)
        {
            return Data.clsLicense.DeleteLicense(licenseID);
        }

        // ================= Instance Methods =================
        public bool IsLicenseExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }

        public bool Deactivate()
        {
            return Data.clsLicense.Deactivate(this.LicenseID);
        }

        public bool Activate()
        {
            return Data.clsLicense.Activate(this.LicenseID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateLicense();
            }

            return false;
        }
    }
}