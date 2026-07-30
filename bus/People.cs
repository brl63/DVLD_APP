using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace bus
{
    public class clsPeople
    {
       public int PersonID { get; set; }
       public string NationalNo { get; set; }
       public string FirstName { get; set; }
       public string SecondName { get; set; }
       public string ThirdName { get; set; }
       public string LastName { get; set; }
       public DateTime DateOfBirth { get; set; }
       public byte Gender { get; set; }
       public string Address { get; set; }
       public string Phone { get; set; }
       public string Email { get; set; }
       public int NationalityCountryID { get; set; }
       public string ImagePath { get; set; }


        public static DataTable GetAll()
        {
            return Data.clsPerson.GetAll();
        }

        /*     public static int Add()
             {
                 // that returns the New PersonID
                 return Data.clsPerson.AddNew(NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);

            }  */



        public static bool Delete(int PersonID)
        {
            return Data.clsPerson.Delete(PersonID);
        }
     
    }
}
