using System;
using System.Data;

namespace bus
{
    public class clsDriver
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsPeople PersonInfo { get; set; }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;
        }

        private clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate = createdDate;
            this.PersonInfo = clsPeople.Find(personID);

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = Data.clsDriver.CreateDriver(this.PersonID, this.CreatedByUserID);
            return (this.DriverID != -1);
        }

        public static clsDriver FindByDriverID(int driverID)
        {
            int personID = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (Data.clsDriver.GetDriverByID(driverID, ref personID, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(driverID, personID, createdByUserID, createdDate);
            }
            return null;
        }

        public static clsDriver FindByPersonID(int personID)
        {
            int driverID = -1, createdByUserID = -1;
            DateTime createdDate = DateTime.Now;

            if (Data.clsDriver.GetDriversByPersonID(personID, ref driverID, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(driverID, personID, createdByUserID, createdDate);
            }
            return null;
        }

        public static DataTable GetAll()
        {
            return Data.clsDriver.GetAllDrivers();
        }

        public static bool IsDriverExists(int personID)
        {
            return Data.clsDriver.IsDriverExists(personID);
        }

        public static bool IsDriverExistsByID(int driverID)
        {
            return Data.clsDriver.IsDriverExistsByID(driverID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    // لا يوجد تعديل على جدول السائقين
                    return true;
            }
            return false;
        }
    }
}
