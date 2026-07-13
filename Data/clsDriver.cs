using System;
using System .Data;
using System.Data.SqlClient;

namespace Data
{
    public class clsDriver
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public static int CreateDriver(int personID, int createdByUserID)
        {
            string sql = "INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate) VALUES (@PersonID, @CreatedByUserID, @CreatedDate); select scope_identity()";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();
                    return Convert.ToInt32(result); // Return the new ID generated
                }
            }
        }

        public static DataTable GetAllDrivers()
        {
            string sql = "SELECT * FROM Drivers_View";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable driversTable = new DataTable();
                        adapter.Fill(driversTable);
                        return driversTable;
                    }
                }
            }
        }

        public static bool GetDriverByID(int driverID, ref int personID, ref int createdByUserID, ref DateTime createdDate)
        {
            string sql = "SELECT * FROM Drivers  WHERE DriverID = @DriverID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personID = Convert.ToInt32(reader["PersonID"]);
                            createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                            return true;
                        }
                        else
                        {
                            return false; // No driver found with the given ID
                        }
                    }

                }
            }
        }

        public static bool GetDriversByPersonID(int personID,ref int driverID, ref int createdByUserID, ref DateTime createdDate)
        {
            string sql = "SELECT * FROM Drivers WHERE PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);
                using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            driverID = Convert.ToInt32(reader["DriverID"]);
                            createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                            return true;
                        }
                        else
                        {
                            return false; // No driver found with the given PersonID
                        }
                    }
                }
            }
        }

        public static bool IsDriverExists(int personID)
        {
            string sql = "SELECT COUNT(*) FROM Drivers WHERE PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personID);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0; // Return true if at least one driver exists for the given person ID
                }
            }




        }

        public static bool IsDriverExistsByID(int driverID)
        {
            string sql = "SELECT COUNT(*) FROM Drivers WHERE DriverID = @DriverID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0; // Return true if at least one driver exists for the given driver ID
                }
            }
        }

       
    }
}              