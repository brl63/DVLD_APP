using System.Data;

namespace bus
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;
            this.Mode = enMode.AddNew;
        }

        private clsTest(int testID, int testAppointmentID, bool testResult, string notes, int createdByUserID)
        {
            this.TestID = testID;
            this.TestAppointmentID = testAppointmentID;
            this.TestResult = testResult;
            this.Notes = notes;
            this.CreatedByUserID = createdByUserID;
            this.Mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            this.TestID = Data.clsTest.InsertTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            if (this.TestID != -1)
            {
                Data.clsTestAppointment.Lock(this.TestAppointmentID);
                return true;
            }

            return false;
        }

        private bool _UpdateTest()
        {
            return Data.clsTest.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public static clsTest Find(int testID)
        {
            int testAppointmentID = -1;
            bool testResult = false;
            string notes = "";
            int createdByUserID = -1;

            if (Data.clsTest.GetByID(testID, ref testAppointmentID, ref testResult, ref notes, ref createdByUserID))
            {
                return new clsTest(testID, testAppointmentID, testResult, notes, createdByUserID);
            }
            else
            {
                return null;
            }
        }

        public static clsTest FindLastTestPerPersonAndTestType(int personID, int testTypeID, int licenseClassID)
        {
            int testID = -1;
            int testAppointmentID = -1;
            bool testResult = false;
            string notes = "";
            int createdByUserID = -1;

            if (Data.clsTest.GetLastTestByPersonAndTestType(personID, testTypeID, licenseClassID,
                ref testID, ref testAppointmentID, ref testResult, ref notes, ref createdByUserID))
            {
                return new clsTest(testID, testAppointmentID, testResult, notes, createdByUserID);
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
                    if (_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }

        public static DataTable GetAllTests()
        {
            return Data.clsTest.GetAll();
        }

        public static byte TotalTrialsPerTest(int localDrivingLicenseApplicationID, clsTestType.enTestType testTypeID)
        {
            return Data.clsTest.TotalTrialsPerTest(localDrivingLicenseApplicationID, (int)testTypeID);
        }
    }
}