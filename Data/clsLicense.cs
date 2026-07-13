using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Data
{
    public class clsLicense
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }

        public static bool GetLicenseByID(int licenseID, ref int applicationID, ref int driverID, ref int licenseClassID, ref DateTime issueDate, ref DateTime expirationDate, ref string notes, ref decimal paidFees, ref bool isActive)
        {
            string sql = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            applicationID = reader.GetInt32(reader.GetOrdinal("ApplicationID"));
                            driverID = reader.GetInt32(reader.GetOrdinal("DriverID"));
                            licenseClassID = reader.GetInt32(reader.GetOrdinal("LicenseClassID"));
                            issueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate"));
                            expirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));
                            if (reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                notes = ""; // Handle null value for Notes
                            }
                            else
                            {
                                notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                            paidFees = reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            return true; // License found and values assigned
                        }
                        else
                        {
                            return false; // No license found with the given ID
                        }
                    }
                }
            }
        }

        public static bool GetLicenseByDriverID(int driverID, ref int licenseID, ref int applicationID, ref int licenseClassID, ref DateTime issueDate, ref DateTime expirationDate, ref string notes, ref decimal paidFees, ref bool isActive)
        {
            string sql = "SELECT * FROM Licenses WHERE DriverID = @DriverID And IsActive = 1";
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
                            licenseID = reader.GetInt32(reader.GetOrdinal("LicenseID"));
                            applicationID = reader.GetInt32(reader.GetOrdinal("ApplicationID"));
                            licenseClassID = reader.GetInt32(reader.GetOrdinal("LicenseClassID"));
                            issueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate"));
                            expirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));
                            if (reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                notes = ""; // Handle null value for Notes
                            }
                            else
                            {
                                notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                            paidFees = reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            return true; // License found and values assigned
                        }
                        else
                        {
                            return false; // No license found for the given DriverID
                        }
                    }
                }
            }
        }

        public static DataTable GetAllLicenses()
        {
            string sql = "SELECT * FROM Licenses";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable licensesTable = new DataTable();

                        adapter.Fill(licensesTable);
                        return licensesTable;
                    }
                }
            }
        }

        public static bool ChangeLicenseStatus(int licenseID, bool newStatus)
        {
            string sql = "UPDATE Licenses SET IsActive = @NewStatus WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NewStatus", newStatus);
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

        public static DataTable GetLocalDrivingLicenses()
        {
            string sql = "SELECT * FROM LocalDrivingLicenseApplications_View";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable licensesTable = new DataTable();
                        adapter.Fill(licensesTable);
                        return licensesTable;
                    }
                }
            }
        }

        public static bool UpdateLicense(int licenseID, int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive)
        {
            string sql = "UPDATE Licenses SET ApplicationID = @ApplicationID, DriverID = @DriverID, LicenseClassID = @LicenseClassID, IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, Notes = @Notes, PaidFees = @PaidFees, IsActive = @IsActive WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    if (string.IsNullOrEmpty(notes))
                    {
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Notes", notes);
                    }
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }



        }

        public static bool DeleteLicense(int licenseID)
        {
            string sql = "DELETE FROM Licenses WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was deleted
                }
            }
        }

        public static int InsertLicense(int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive)
        {
            string sql = "INSERT INTO Licenses (ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive) VALUES (@ApplicationID, @DriverID, @LicenseClassID, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive); SELECT SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    if (string.IsNullOrEmpty(notes))
                    {
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Notes", notes);
                    }
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();
                    return Convert.ToInt32(result); // Return the new ID generated
                }
            }
        }

        public static DataTable GetLicensesByDriverID(int driverID)
        {
            string sql = "SELECT * FROM Licenses WHERE DriverID = @DriverID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        
                        DataTable licensesTable = new DataTable();
                        adapter.Fill(licensesTable);
                        return licensesTable;
                    }
                }
            }
        }

        public static bool Deactivate(int licenseID)
        {
            string sql = "UPDATE Licenses SET IsActive = 0 WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

        public static bool Activate(int licenseID)
        {
            string sql = "UPDATE Licenses SET IsActive = 1 WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

        public static bool IsLicenseActive(int licenseID)
        {
            string sql = "SELECT IsActive FROM Licenses WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToBoolean(result);
                    }
                    else
                    {
                        throw new Exception("License not found.");
                    }
                }
            }
        }

        public static bool IsLicenseExists(int licenseID)
        {
            string sql = "SELECT COUNT(*) FROM Licenses WHERE LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseID);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0; // Return true if at least one license exists with the given ID
                }
            }
        }

        public static bool DeactivateAllLicensesByDriverID(int driverID)
        {
            string sql = "UPDATE Licenses SET IsActive = 0 WHERE DriverID = @DriverID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; // Return true if at least one row was updated
                }
            }
        }

        public static bool CheckActiveLicenseByClass(int DriverID, int classID)
        {
            string sql = "SELECT COUNT(*) FROM Licenses WHERE DriverID = @DriverID AND LicenseClass = @ClassID AND IsActive = 1";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@ClassID", classID);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    connection.Close();
                    return count > 0; // Return true if at least one active license exists for the given class
                }
            }
        }
    }
}