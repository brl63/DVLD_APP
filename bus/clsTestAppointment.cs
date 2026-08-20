using System;
using System.Data;
using Data;

namespace bus
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }
        public clsTestType.enTestType TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsLocked { get; set; }
        public int CreatedByUserID { get; set; }

        public int TestID
        {
            get
            {
                return Data.clsTestAppointment.GetTestID(this.TestAppointmentID);
            }
        }

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.IsLocked = false;
            this.CreatedByUserID = -1;

            this.Mode = enMode.AddNew;
        }

        private clsTestAppointment(int testAppointmentID, clsTestType.enTestType testTypeID,
            int localDrivingLicenseApplicationID, DateTime appointmentDate, decimal paidFees,
            bool isLocked, int createdByUserID)
        {
            this.TestAppointmentID = testAppointmentID;
            this.TestTypeID = testTypeID;
            this.LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.IsLocked = isLocked;
            this.CreatedByUserID = createdByUserID;

            this.Mode = enMode.Update;
        }

        private bool _AddNewAppointment()
        {
            this.TestAppointmentID = Data.clsTestAppointment.AddNewAppointment(
                (int)this.TestTypeID,
                this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate,
                this.PaidFees,
                this.CreatedByUserID);

            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateAppointment()
        {
            return Data.clsTestAppointment.UpdateAppointment(
                this.TestAppointmentID,
                (int)this.TestTypeID,
                this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate,
                this.PaidFees,
                this.IsLocked,
                this.CreatedByUserID);
        }

        public static clsTestAppointment Find(int testAppointmentID)
        {
            int testTypeID = -1;
            int localDrivingLicenseApplicationID = -1;
            DateTime appointmentDate = DateTime.Now;
            decimal paidFees = 0;
            bool isLocked = false;
            int createdByUserID = -1;

            if (Data.clsTestAppointment.GetAppointment(testAppointmentID,
                ref testTypeID, ref localDrivingLicenseApplicationID, ref appointmentDate,
                ref paidFees, ref isLocked, ref createdByUserID))
            {
                return new clsTestAppointment(testAppointmentID, (clsTestType.enTestType)testTypeID,
                    localDrivingLicenseApplicationID, appointmentDate, paidFees, isLocked, createdByUserID);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateAppointment();
            }

            return false;
        }

        public bool Lock()
        {
            return Data.clsTestAppointment.Lock(this.TestAppointmentID);
        }

        public static DataTable GetAll()
        {
            return Data.clsTestAppointment.GetAll();
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int localDrivingLicenseApplicationID, clsTestType.enTestType testTypeID)
        {
            return Data.clsTestAppointment.GetApplicationTestAppointmentsPerTestType(localDrivingLicenseApplicationID, (int)testTypeID);
        }
    }
}
