using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Data;
using System.Data;

namespace bus
{
    public class clsApplications
        
    {
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 }


        public static DataTable GetAll()
        {
            return Data.clsApplication.GetAll();
        }
    }
}

