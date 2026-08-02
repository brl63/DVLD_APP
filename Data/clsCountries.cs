using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Data
{
    public class clsCountries
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }


        public static DataTable GetAllCountries()
        {
            string query = "SELECT * FROM Countries";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable countriesTable = new DataTable();
                        adapter.Fill(countriesTable);
                        return countriesTable;
                    }
                }
        }
    }

        public static string GetCountryName(int countryID)
        {
            string query = "SELECT CountryName FROM Countries WHERE CountryID = @CountryID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", countryID);
                    object result = command.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }
    }
}
