using System;
using System.Data;
using Data;

namespace bus
{
    public class clsTestType
    {
        public enum enTestType
        {
            VisionTest = 1,
            WrittenTest = 2,
            StreetTest = 3
        }

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.Update;

        public enTestType ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Fees { get; set; }

        public clsTestType()
        {
            this.ID = enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            this.Mode = enMode.AddNew;
        }

        private clsTestType(enTestType id, string title, string description, decimal fees)
        {
            this.ID = id;
            this.Title = title;
            this.Description = description;
            this.Fees = fees;
            this.Mode = enMode.Update;
        }

        private bool _UpdateTestType()
        {
            return Data.clsTestType.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }

        public static clsTestType Find(enTestType testTypeID)
        {
            string title = "";
            string description = "";
            decimal fees = 0;

            if (Data.clsTestType.GetTestType((int)testTypeID, ref title, ref description, ref fees))
            {
                return new clsTestType(testTypeID, title, description, fees);
            }

            return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    return false;

                case enMode.Update:
                    return _UpdateTestType();
            }

            return false;
        }

        public static bool UpdateFees(enTestType testTypeID, decimal newFees)
        {
            return Data.clsTestType.UpdateFees((int)testTypeID, newFees);
        }

        public static DataTable GetAllTestTypes()
        {
            return Data.clsTestType.GetAll();
        }
    }
}