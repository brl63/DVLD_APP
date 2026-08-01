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

        public static clsUsers Login(string userName, string password)
        {
           int userID = 0;
            int personID = 0;
            bool isActive = false;
            
                if( Data.clsUser.ValidateUser(userName, password, ref userID) == true)
                {
                    Data.clsUser.GetUserByID(userID,ref personID , ref userName, ref password, ref isActive);

                    return new clsUsers
                    {
                        UserID = userID,
                        PersonID = personID,
                        UserName = userName,
                        Password = password,
                        IsActive = isActive
                    };
                }
            else
            {
                //TODO put in logs Files ( I didnt do that yet)
}
            return null;
            }
        }
    }

