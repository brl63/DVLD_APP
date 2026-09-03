using System;
using System.Data;

namespace bus
{
    public class clsApplications
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 }
        public enum enApplicationType
        {
            NewDrivingLicense = 1,
            RenewDrivingLicense = 2,
            ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4,
            ReleaseDetainedDrivingLicense = 5,
            NewInternationalLicense = 6,
            RetakeTest = 7
        }

        public enMode Mode = enMode.AddNew;

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public clsPeople PersonInfo { get; set; }
        public string ApplicantFullName
        {
            get
            {
                return clsPeople.Find(ApplicantPersonID)?.FullName ?? "";
            }
        }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public clsApplicationTypes ApplicationTypeInfo { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public string StatusText
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }

        public int LicenseClassID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }

        public clsLicenseClass LicenseClassInfo
        {
            get
            {
                return clsLicenseClass.Find(this.LicenseClassID);
            }
        }

        public clsApplications()
        {
            this.ApplicationID = -1;
            this.LocalDrivingLicenseApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.LicenseClassID = -1;

            this.Mode = enMode.AddNew;
        }

        private clsApplications(int applicationID, int applicantPersonID, DateTime applicationDate,
            int applicationTypeID, enApplicationStatus applicationStatus, DateTime lastStatusDate,
            decimal paidFees, int createdByUserID)
        {
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = applicantPersonID;
            this.PersonInfo = clsPeople.Find(applicantPersonID);
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeInfo = clsApplicationTypes.Find(applicationTypeID);
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            this.CreatedByUserInfo = clsUser.Find(createdByUserID);

            this.Mode = enMode.Update;
        }

        // ================= Private CRUD Operations =================
        private bool _AddNewApplication()
        {
            // 1. إذا كان طلباً لرخصة محلية جديدة ولديه فئة رخصة محددة
            if (this.LicenseClassID != -1 && this.ApplicationTypeID == (int)enApplicationType.NewDrivingLicense)
            {
                this.LocalDrivingLicenseApplicationID = Data.clsApplication.AddNewLocalDrivingLicenseApplication(
                    this.ApplicantPersonID,
                    this.ApplicationDate,
                    this.ApplicationTypeID,
                    (byte)this.ApplicationStatus,
                    this.LastStatusDate,
                    this.PaidFees,
                    this.CreatedByUserID,
                    this.LicenseClassID);

                // بعد حفظ الرخصة المحلية، نقوم بجلب الـ ApplicationID الأساسي المربوط بها
                if (this.LocalDrivingLicenseApplicationID != -1)
                {
                    clsApplications appInfo = FindByLocalDrivingAppID(this.LocalDrivingLicenseApplicationID);
                    if (appInfo != null)
                    {
                        this.ApplicationID = appInfo.ApplicationID;
                    }
                }

                return (this.LocalDrivingLicenseApplicationID != -1);
            }
            else
            {
                // 2. إذا كان طلباً عاماً
                this.ApplicationID = Data.clsApplication.AddNewApplication(
                    this.ApplicantPersonID,
                    this.ApplicationDate,
                    this.ApplicationTypeID,
                    (byte)this.ApplicationStatus,
                    this.LastStatusDate,
                    this.PaidFees,
                    this.CreatedByUserID);

                return (this.ApplicationID != -1);
            }
        }

        private bool _UpdateApplication()
        {
            return Data.clsApplication.UpdateApplication(
                this.ApplicationID,
                this.ApplicantPersonID,
                this.ApplicationDate,
                this.ApplicationTypeID,
                (byte)this.ApplicationStatus,
                this.LastStatusDate,
                this.PaidFees,
                this.CreatedByUserID);
        }

        // ================= Public Methods =================
        public static clsApplications Find(int applicationID)
        {
            return FindBaseApplication(applicationID);
        }

        public static clsApplications FindBaseApplication(int applicationID)
        {
            int applicantPersonID = -1;
            DateTime applicationDate = DateTime.MinValue;
            int applicationTypeID = -1;
            byte applicationStatus = 1;
            DateTime lastStatusDate = DateTime.MinValue;
            decimal paidFees = 0;
            int createdByUserID = -1;

            bool isFound = Data.clsApplication.GetApplication(
                applicationID,
                ref applicantPersonID,
                ref applicationDate,
                ref applicationTypeID,
                ref applicationStatus,
                ref lastStatusDate,
                ref paidFees,
                ref createdByUserID);

            if (isFound)
            {
                return new clsApplications(
                    applicationID,
                    applicantPersonID,
                    applicationDate,
                    applicationTypeID,
                    (enApplicationStatus)applicationStatus,
                    lastStatusDate,
                    paidFees,
                    createdByUserID);
            }
            else
            {
                return null;
            }
        }

        public virtual bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;
        }

        public bool Cancel()
        {
            return Data.clsApplication.UpdateStatus(this.ApplicationID, (byte)enApplicationStatus.Cancelled);
        }

        public bool SetComplete()
        {
            return Data.clsApplication.UpdateStatus(this.ApplicationID, (byte)enApplicationStatus.Completed);
        }

        public bool Delete()
        {
            if (this.LocalDrivingLicenseApplicationID != -1)
            {
                return Data.clsApplication.DeleteLocalApplication(this.LocalDrivingLicenseApplicationID);
            }
            return Data.clsApplication.DeleteApplication(this.ApplicationID);
        }

        public bool UpdateStatus(enApplicationStatus newStatus)
        {
            return Data.clsApplication.UpdateStatus(this.ApplicationID, (byte)newStatus);
        }

        public bool UpdatePaidFees(decimal newFees)
        {
            return Data.clsApplication.UpdatePaidFees(this.ApplicationID, newFees);
        }

        public static bool IsApplicationExist(int applicationID)
        {
            return Data.clsApplication.IsApplicationExist(applicationID);
        }

        public static bool DoesHaveApplicant(int applicantPersonID)
        {
            return Data.clsApplication.DoesHaveApplicant(applicantPersonID);
        }

        public static bool DoesPersonHaveActiveApplication(int personID, int applicationTypeID)
        {
            return Data.clsApplication.DoesPersonHaveActiveApplication(personID, applicationTypeID);
        }

        public static int GetActiveApplicationID(int personID, enApplicationType applicationTypeID)
        {
            return Data.clsApplication.GetActiveApplicationID(personID, (int)applicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int personID, enApplicationType applicationTypeID, int licenseClassID)
        {
            return Data.clsApplication.GetActiveApplicationIDForLicenseClass(personID, (int)applicationTypeID, licenseClassID);
        }

        public static clsApplications GetLatestApplicationByPerson(int personID, enApplicationType applicationTypeID)
        {
            int applicationID = -1;
            DateTime applicationDate = DateTime.MinValue;
            byte applicationStatus = 1;
            DateTime lastStatusDate = DateTime.MinValue;
            decimal paidFees = 0;
            int createdByUserID = -1;

            bool isFound = Data.clsApplication.GetLatestApplicationByPerson(
                personID,
                (int)applicationTypeID,
                ref applicationID,
                ref applicationDate,
                ref applicationStatus,
                ref lastStatusDate,
                ref paidFees,
                ref createdByUserID);

            if (isFound)
            {
                return new clsApplications(
                    applicationID,
                    personID,
                    applicationDate,
                    (int)applicationTypeID,
                    (enApplicationStatus)applicationStatus,
                    lastStatusDate,
                    paidFees,
                    createdByUserID);
            }
            return null;
        }

        public static DataTable GetAll()
        {
            return Data.clsApplication.GetAll();
        }

        public static DataTable GetApplicationsByStatus(enApplicationStatus status)
        {
            return Data.clsApplication.GetApplicationsByStatus((byte)status);
        }

        public static DataTable GetApplicationsByApplicant(int applicantPersonID)
        {
            return Data.clsApplication.GetApplicationsByApplicant(applicantPersonID);
        }

        public static DataTable GetAllLocalApplications()
        {
            return Data.clsApplication.GetLocalDrivingApplications();
        }

        public static DataTable GetAllInternationalApplications()
        {
            return Data.clsApplication.GetInternationalDrivingApplications();
        }

        public byte GetPassedTestCount()
        {
            return Data.clsApplication.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }

        public int GetActiveLicenseID()
        {
            return Data.clsApplication.GetActiveLicenseIDByApplicationID(this.ApplicationID);
        }

        public static clsApplications FindByLocalDrivingAppID(int localDrivingLicenseApplicationID)
        {
            int applicationID = -1;
            int applicantPersonID = -1;
            DateTime applicationDate = DateTime.MinValue;
            int applicationTypeID = -1;
            byte applicationStatus = 1;
            DateTime lastStatusDate = DateTime.MinValue;
            decimal paidFees = 0;
            int createdByUserID = -1;
            int licenseClassID = -1;

            bool isFound = Data.clsApplication.GetApplicationInfoByLocalDrivingAppID(
                localDrivingLicenseApplicationID,
                ref applicationID,
                ref applicantPersonID,
                ref applicationDate,
                ref applicationTypeID,
                ref applicationStatus,
                ref lastStatusDate,
                ref paidFees,
                ref createdByUserID,
                ref licenseClassID);

            if (isFound)
            {
                clsApplications app = new clsApplications(
                    applicationID,
                    applicantPersonID,
                    applicationDate,
                    applicationTypeID,
                    (enApplicationStatus)applicationStatus,
                    lastStatusDate,
                    paidFees,
                    createdByUserID);

                app.LicenseClassID = licenseClassID;
                app.LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
                return app;
            }

            return null;
        }

        public int IssueLicenseForTheFistTime(string notes, int createdByUserID)
        {
            int driverID = -1;
            clsDriver driver = clsDriver.FindByPersonID(this.ApplicantPersonID);

            if (driver == null)
            {
                driver = new clsDriver();
                driver.PersonID = this.ApplicantPersonID;
                driver.CreatedByUserID = createdByUserID;
                if (!driver.Save())
                    return -1;

                driverID = driver.DriverID;
            }
            else
            {
                driverID = driver.DriverID;
            }

            clsLicenseClass licenseClass = clsLicenseClass.Find(this.LicenseClassID);
            int defaultValidity = licenseClass != null ? licenseClass.DefaultValidityLength : 10;
            decimal classFees = licenseClass != null ? licenseClass.ClassFees : 0;

            clsLicense license = new clsLicense();
            license.ApplicationID = this.ApplicationID;
            license.DriverID = driverID;
            license.LicenseClassID = this.LicenseClassID;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(defaultValidity);
            license.Notes = notes;
            license.PaidFees = classFees;
            license.IsActive = true;
            license.IssueReason = clsLicense.enIssueReason.FirstTime;
            license.CreatedByUserID = createdByUserID;

            if (!license.Save())
                return -1;

            this.SetComplete();

            return license.LicenseID;
        }

        public static bool DoesPersonHaveActiveApplicationForLicenseClass(int personID, int applicationTypeID, int licenseClassID)
        {
            return Data.clsApplication.DoesPersonHaveActiveApplicationForLicenseClass(personID, applicationTypeID, licenseClassID);
        }
    }
}