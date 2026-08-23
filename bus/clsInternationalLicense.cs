using Data;
using System;
using System.Data;

namespace bus
{
    public class clsInternationalLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }

        private clsInternationalLicense(int internationalLicenseID, int applicationID, int driverID, int issuedUsingLocalLicenseID, DateTime issueDate, DateTime expirationDate, bool isActive, int createdByUserID)
        {
            this.InternationalLicenseID = internationalLicenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.IsActive = isActive;
            this.CreatedByUserID = createdByUserID;
            Mode = enMode.Update;
        }

        public static clsInternationalLicense Find(int internationalLicenseID)
        {
            int applicationID = -1, driverID = -1, issuedUsingLocalLicenseID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isActive = false;

            if (Data.clsInternationalLicense.GetInternationalLicenseByID(internationalLicenseID, ref applicationID, ref driverID, ref issuedUsingLocalLicenseID, ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))
            {
                return new clsInternationalLicense(internationalLicenseID, applicationID, driverID, issuedUsingLocalLicenseID, issueDate, expirationDate, isActive, createdByUserID);
            }
            return null;
        }

        public static DataTable GetAll() => Data.clsInternationalLicense.GetAll();

        public static DataTable GetDriverInternationalLicenses(int driverID) => Data.clsInternationalLicense.GetInternationalLicensesByDriverID(driverID);

        public static bool IsActiveLicenseExistsByLocalLicenseID(int localLicenseID)
        {
            return (Data.clsInternationalLicense.GetActiveInternationalLicenseIDByLocalLicenseID(localLicenseID) != -1);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    int insertedID = Data.clsInternationalLicense.InsertInternationalLicense(ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
                    if (insertedID != -1)
                    {
                        this.InternationalLicenseID = insertedID;
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return Data.clsInternationalLicense.ChangeInternationalLicenseStatus(this.InternationalLicenseID, this.IsActive);
            }
            return false;
        }


        public static int GetActiveInternationalLicenseIDByLocalLicenseID(int localLicenseID)
        {
            return Data.clsInternationalLicense.GetActiveInternationalLicenseIDByLocalLicenseID(localLicenseID);
        }

        public static bool CanIssueInternationalLicense(int localLicenseID, out string errorMessage)
        {
            errorMessage = "";
            clsLicense localLicense = clsLicense.Find(localLicenseID);

            if (localLicense == null)
            {
                errorMessage = "Local driving license does not exist.";
                return false;
            }

            if (!localLicense.IsActive)
            {
                errorMessage = "Selected license is not active, choose an active license.";
                return false;
            }

            if (localLicense.IsLicenseExpired())
            {
                errorMessage = "Selected license is expired, you cannot issue an international license from it.";
                return false;
            }

            if (localLicense.LicenseClassID != 3)
            {
                errorMessage = "Selected license class must be (Class 3 - Ordinary driving license).";
                return false;
            }

            int activeIntLicenseID = GetActiveInternationalLicenseIDByLocalLicenseID(localLicenseID);
            if (activeIntLicenseID != -1)
            {
                errorMessage = $"Person already has an active international license with ID = {activeIntLicenseID}";
                return false;
            }

            return true;
        }
        public static clsInternationalLicense IssueInternationalLicense(int localLicenseID, int createdByUserID, out string errorMessage)
        {
            if (!CanIssueInternationalLicense(localLicenseID, out errorMessage))
            {
                return null;
            }

            clsLicense localLicense = clsLicense.Find(localLicenseID);

            clsApplications app = new clsApplications();
            app.ApplicantPersonID = localLicense.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 6; // New International License
            app.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            app.LastStatusDate = DateTime.Now;
            app.PaidFees = clsApplicationType.GetFee(6);
            app.CreatedByUserID = createdByUserID;

            if (!app.Save())
            {
                errorMessage = "Failed to create application record.";
                return null;
            }

            clsInternationalLicense intLicense = new clsInternationalLicense();
            intLicense.ApplicationID = app.ApplicationID;
            intLicense.DriverID = localLicense.DriverID;
            intLicense.IssuedUsingLocalLicenseID = localLicenseID;
            intLicense.IssueDate = DateTime.Now;
            intLicense.ExpirationDate = DateTime.Now.AddYears(1);
            intLicense.IsActive = true;
            intLicense.CreatedByUserID = createdByUserID;

            if (!intLicense.Save())
            {
                errorMessage = "Failed to create international license record.";
                return null;
            }

            return intLicense;
        }

    }
}