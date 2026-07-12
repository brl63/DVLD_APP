using System;

namespace DVLD.Data
{
    public class clsTestAppointment
    {
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int ApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsLocked { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
