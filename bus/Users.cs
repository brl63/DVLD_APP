using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Data;

namespace bus
{
    public class clsUsers
    {
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public static DataTable GetAll()
        {
            return Data.clsUser.GetALL();
        }

        public static bool Login(string UserName, string Password)
        {
            if (Data.clsUser.IsUserNameExists(UserName))
            {
                return Data.clsUser.ValidateUser(UserName, Password,  UserID);

                
            }
            else
            {
                // put in logs Files ( I didnt do that yet)
                
                return false;
            }
        }
    }
}
