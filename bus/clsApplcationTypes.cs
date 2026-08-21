using System.Data;

namespace bus
{
    public class clsApplicationTypes
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }

        public clsApplicationTypes()
        {
            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
            this.ApplicationFees = 0;
            this.Mode = enMode.AddNew;
        }

        private clsApplicationTypes(int applicationTypeID, string applicationTypeTitle, decimal applicationFees)
        {
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeTitle = applicationTypeTitle;
            this.ApplicationFees = applicationFees;
            this.Mode = enMode.Update;
        }

        public static clsApplicationTypes Find(int applicationTypeID)
        {
            string title = "";
            decimal fees = 0;

            if (Data.clsApplicationType.GetApplicationTypeInfoByID(applicationTypeID, ref title, ref fees))
            {
                return new clsApplicationTypes(applicationTypeID, title, fees);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAll()
        {
            return Data.clsApplicationType.GetAllApplicationTypes();
        }

        private bool _UpdateApplicationType()
        {
            return Data.clsApplicationType.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    return false;

                case enMode.Update:
                    return _UpdateApplicationType();
            }

            return false;
        }

        public static bool ChangeFees(decimal newFee, int applicationTypeID)
        {
            return Data.clsApplicationType.ChangeFees(newFee, applicationTypeID);
        }

        public static bool UpdateApplicationType(int applicationTypeID, string newTitle, decimal newFee)
        {
            return Data.clsApplicationType.UpdateApplicationType(applicationTypeID, newTitle, newFee);
        }
    }
}

