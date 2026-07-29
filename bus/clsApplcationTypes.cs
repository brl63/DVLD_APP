using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Data;

namespace bus
{
    public class clsApplicationTypes
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal Fee { get; set; }

        public static DataTable GetAll()
        {
            return Data.clsApplicationType.GetAllApplicationTypes();
        }
    }
}
