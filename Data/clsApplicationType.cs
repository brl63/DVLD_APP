using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Data
{
    public class clsApplicationType
    {
        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal Fee { get; set; }


        public static bool ChangeFees(decimal newFee, int applicationTypeID)
        {
            string sql = "UPDATE ApplicationTypes SET Fee = @NewFee WHERE ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewFee", newFee);
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

        public static decimal GetFee(int applicationTypeID)
        {
            string sql = "SELECT Fee FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }
                    else
                    {
                        throw new Exception("Application type not found.");
                    }
                }
            }
        }

        public static DataTable GetAllApplicationTypes()
        {
            string sql = "SELECT * FROM ApplicationTypes";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable applicationTypesTable = new DataTable();
                        adapter.Fill(applicationTypesTable);
                        return applicationTypesTable;
                    }
                }
            }
        }

        public static bool UpdateApplicationType(int applicationTypeID, string newTitle, decimal newFee)
        {
            string sql = "UPDATE ApplicationTypes SET ApplicationTypeTitle = @NewTitle, Fee = @NewFee WHERE ApplicationTypeID = @ApplicationTypeID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewTitle", newTitle);
                    command.Parameters.AddWithValue("@NewFee", newFee);
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

    }
}
