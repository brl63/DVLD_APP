using System;
using System.Data;

namespace bus
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementForDamaged = 3, ReplacementForLost = 4 }

        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserID { get; set; }

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
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;
        }

        private clsLicense(int licenseID, int applicationID, int driverID, int licenseClassID,
            DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive,
            enIssueReason issueReason, int createdByUserID)
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
            this.IssueReason = issueReason;
            this.CreatedByUserID = createdByUserID;

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
                this.IsActive,
                (byte)this.IssueReason,
                this.CreatedByUserID);

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
                this.IsActive,
                (byte)this.IssueReason,
                this.CreatedByUserID);
        }

        // ================= Public Static Methods =================
        public static clsLicense Find(int licenseID)
        {
            int applicationID = -1, driverID = -1, licenseClassID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = false;
            byte issueReason = 1;

            if (Data.clsLicense.GetLicenseByID(licenseID, ref applicationID, ref driverID,
                ref licenseClassID, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
            {
                return new clsLicense(licenseID, applicationID, driverID, licenseClassID,
                    issueDate, expirationDate, notes, paidFees, isActive, (enIssueReason)issueReason, createdByUserID);
            }

            return null;
        }

        public static clsLicense FindByDriverID(int driverID)
        {
            int licenseID = -1, applicationID = -1, licenseClassID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = false;
            byte issueReason = 1;

            if (Data.clsLicense.GetLicenseByDriverID(driverID, ref licenseID, ref applicationID,
                ref licenseClassID, ref issueDate, ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
            {
                return new clsLicense(licenseID, applicationID, driverID, licenseClassID,
                    issueDate, expirationDate, notes, paidFees, isActive, (enIssueReason)issueReason, createdByUserID);
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

        public int GetActiveInternationalLicenseID()
        {
            return Data.clsInternationalLicense.GetActiveInternationalLicenseIDByLocalLicenseID(this.LicenseID);
        }

        public bool CanBeRenewed(out string errorMessage)
        {
            errorMessage = "";

            if (!this.IsActive)
            {
                errorMessage = "Selected license is not active, choose an active license.";
                return false;
            }

            if (!this.IsLicenseExpired())
            {
                errorMessage = $"Selected license is not expired yet. It will expire on: {this.ExpirationDate.ToShortDateString()}";
                return false;
            }

            return true;
        }

        public clsLicense RenewLicense(string notes, int createdByUserID)
        {
            clsApplications app = new clsApplications();
            app.ApplicantPersonID = this.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = (int)clsApplications.enApplicationType.RenewDrivingLicense; // 2
            app.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            app.LastStatusDate = DateTime.Now;
            app.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RenewDrivingLicense)?.ApplicationFees ?? 0;
            app.CreatedByUserID = createdByUserID;

            if (!app.Save())
            {
                return null;
            }

            int defaultValidityLength = this.LicenseClassInfo != null ? this.LicenseClassInfo.DefaultValidityLength : 10;
            decimal classFees = this.LicenseClassInfo != null ? this.LicenseClassInfo.ClassFees : 0;

            clsLicense newLicense = new clsLicense();
            newLicense.ApplicationID = app.ApplicationID;
            newLicense.DriverID = this.DriverID;
            newLicense.LicenseClassID = this.LicenseClassID;
            newLicense.IssueDate = DateTime.Now;
            newLicense.ExpirationDate = DateTime.Now.AddYears(defaultValidityLength);
            newLicense.Notes = string.IsNullOrEmpty(notes) ? "" : notes;
            newLicense.PaidFees = classFees;
            newLicense.IsActive = true;
            newLicense.IssueReason = enIssueReason.Renew;
            newLicense.CreatedByUserID = createdByUserID;

            if (!newLicense.Save())
            {
                return null;
            }

            this.Deactivate();

            return newLicense;
        }

        public clsLicense Replace(enIssueReason issueReason, int createdByUserID)
        {
            int applicationTypeID = (issueReason == enIssueReason.ReplacementForDamaged) ? 4 : 3;

            // 1. إنشاء سجل الطلب العام
            clsApplications app = new clsApplications();
            app.ApplicantPersonID = this.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = applicationTypeID;
            app.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            app.LastStatusDate = DateTime.Now;
            app.PaidFees = clsApplicationTypes.Find(applicationTypeID)?.ApplicationFees ?? 0;
            app.CreatedByUserID = createdByUserID;

            if (!app.Save())
            {
                return null;
            }

            // 2. إنشاء الرخصة البديلة
            clsLicense newLicense = new clsLicense();
            newLicense.ApplicationID = app.ApplicationID;
            newLicense.DriverID = this.DriverID;
            newLicense.LicenseClassID = this.LicenseClassID;
            newLicense.IssueDate = DateTime.Now;
            newLicense.ExpirationDate = this.ExpirationDate; // تأخذ نفس تاريخ انتهاء القديمة
            newLicense.Notes = this.Notes;
            newLicense.PaidFees = 0; // الرسوم دُفعت في الطلب
            newLicense.IsActive = true;
            newLicense.IssueReason = issueReason;
            newLicense.CreatedByUserID = createdByUserID;

            if (!newLicense.Save())
            {
                return null;
            }

            // 3. إلغاء تفعيل القديمة
            this.Deactivate();

            return newLicense;
        }

        public bool IsDetained
        {
            get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID); }
        }

        public int Detain(decimal fineFees, int createdByUserID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = fineFees;
            detainedLicense.CreatedByUserID = createdByUserID;
            detainedLicense.IsReleased = false;

            if (detainedLicense.Save())
            {
                return detainedLicense.DetainID;
            }
            return -1;
        }

        public bool ReleaseDetainedLicense(int releasedByUserID, ref int applicationID)
        {
            clsDetainedLicense detainedLicense = clsDetainedLicense.FindByLicenseID(this.LicenseID);
            if (detainedLicense == null || detainedLicense.IsReleased) return false;

            // 1. إنشاء طلب فك حجز عام
            clsApplications app = new clsApplications();
            app.ApplicantPersonID = this.DriverInfo.PersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = (int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicense; // 5
            app.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            app.LastStatusDate = DateTime.Now;
            app.PaidFees = clsApplicationTypes.Find(5)?.ApplicationFees ?? 0;
            app.CreatedByUserID = releasedByUserID;

            if (!app.Save()) return false;

            applicationID = app.ApplicationID;

            // 2. تحديث سجل الحجز
            return detainedLicense.Release(releasedByUserID, applicationID);
        }
    }
}