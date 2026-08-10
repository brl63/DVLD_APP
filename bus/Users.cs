using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Data;
using System.Security.Cryptography.X509Certificates;

namespace bus
{
    public class clsUser
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

        public static clsUser Login(string userName, string password)
        {
            int userID = 0;
            int personID = 0;
            bool isActive = false;

            if (Data.clsUser.ValidateUser(userName, password, ref userID) == true)
            {
                Data.clsUser.GetUserByID(userID, ref personID, ref userName, ref password, ref isActive);

                return new clsUser
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

        public static clsUser Find(int UserId)
        {
            int personID = 0;
            string userName = "";
            string password = "";
            bool isActive = false;
            Data.clsUser.GetUserByID(UserId, ref personID, ref userName, ref password, ref isActive);
            return new clsUser
            {
                UserID = UserId,
                PersonID = personID,
                UserName = userName,
                Password = password,
                IsActive = isActive
            };
        }

        public static int Add(int personID, string userName, string password, bool isActive)
        {
            return Data.clsUser.AddNew(personID, userName, password, isActive);
        }

        public static bool ChangePassword(int userID, string newPassword)
        {
            return Data.clsUser.ChangePassword(userID, newPassword);
        }

    }
}

