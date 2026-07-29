using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bus
{
    public class clsPeople
    {
       private int PersonID { get; set; }
       private string NationalNo { get; set; }
       private string FirstName { get; set; }
       private string SecondName { get; set; }
       private string ThirdName { get; set; }
       private string LastName { get; set; }
       private DateTime DateOfBirth { get; set; }
       private byte Gender { get; set; }
       private string Address { get; set; }
       private string Phone { get; set; }
       private string Email { get; set; }
       private int NationalityCountryID { get; set; }
       private string ImagePath { get; set; }


        public static DataTable GetAll()
        {
            return Data.clsPerson.GetAll();
        }

        public static int Add()
        {
            // that returns the New PersonID
            return Data.clsPerson.AddNew( NationalNo,  FirstName,  SecondName,  ThirdName,  LastName,  DateOfBirth,  Gender,  Address,  Phone,  Email,  NationalityCountryID,  ImagePath)
);
        }

        public static bool Delete(int PersonID)
        {
            return Data.clsPerson.Delete(PersonID);
        }
     
    }
}
