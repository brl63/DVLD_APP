using System;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class clsLicense
    {
        public static bool GetLicenseByID(int licenseID, ref int applicationID, ref int driverID, ref int licenseClassID,
            ref DateTime issueDate, ref DateTime expirationDate, ref string notes, ref decimal paidFees, ref bool isActive,
            ref byte issueReason, ref int createdByUserID)
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
                            licenseClassID = reader.GetInt32(reader.GetOrdinal("LicenseClass"));
                            issueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate"));
                            expirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));
                            notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString(reader.GetOrdinal("Notes"));
                            paidFees = reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            issueReason = reader.GetByte(reader.GetOrdinal("IssueReason"));
                            createdByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));

                            return true;
                        }
                        return false;
                    }
                }
            }
        }

        public static bool GetLicenseByDriverID(int driverID, ref int licenseID, ref int applicationID, ref int licenseClassID,
            ref DateTime issueDate, ref DateTime expirationDate, ref string notes, ref decimal paidFees, ref bool isActive,
            ref byte issueReason, ref int createdByUserID)
        {
            string sql = "SELECT * FROM Licenses WHERE DriverID = @DriverID AND IsActive = 1";
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
                            licenseClassID = reader.GetInt32(reader.GetOrdinal("LicenseClass"));
                            issueDate = reader.GetDateTime(reader.GetOrdinal("IssueDate"));
                            expirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"));
                            notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString(reader.GetOrdinal("Notes"));
                            paidFees = reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            issueReason = reader.GetByte(reader.GetOrdinal("IssueReason"));
                            createdByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));

                            return true;
                        }
                        return false;
                    }
                }
            }
        }

        public static int InsertLicense(int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive, byte issueReason, int createdByUserID)
        {
            string sql = @"INSERT INTO Licenses 
                           (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID) 
                           VALUES 
                           (@ApplicationID, @DriverID, @LicenseClassID, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID); 
                           SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes);
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@IssueReason", issueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    return (result != null && int.TryParse(result.ToString(), out int insertedID)) ? insertedID : -1;
                }
            }
        }

        public static bool UpdateLicense(int licenseID, int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expirationDate, string notes, decimal paidFees, bool isActive, byte issueReason, int createdByUserID)
        {
            string sql = @"UPDATE Licenses SET 
                           ApplicationID = @ApplicationID, 
                           DriverID = @DriverID, 
                           LicenseClass = @LicenseClassID, 
                           IssueDate = @IssueDate, 
                           ExpirationDate = @ExpirationDate, 
                           Notes = @Notes, 
                           PaidFees = @PaidFees, 
                           IsActive = @IsActive, 
                           IssueReason = @IssueReason, 
                           CreatedByUserID = @CreatedByUserID 
                           WHERE LicenseID = @LicenseID";

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
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? (object)DBNull.Value : notes);
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@IssueReason", issueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
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

        public static DataTable GetLocalDrivingLicenses()
        {
            string sql = "SELECT * FROM LocalDrivingLicenseApplications_View";
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
                    return rowsAffected > 0;
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
                    return rowsAffected > 0;
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
                    return rowsAffected > 0;
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
                    return (result != null && result != DBNull.Value) ? Convert.ToBoolean(result) : false;
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
                    return count > 0;
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
                    return rowsAffected > 0;
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
                    return rowsAffected > 0;
                }
            }
        }

        public static bool CheckActiveLicenseByClass(int driverID, int classID)
        {
            string sql = "SELECT COUNT(*) FROM Licenses WHERE DriverID = @DriverID AND LicenseClass = @ClassID AND IsActive = 1";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverID);
                    command.Parameters.AddWithValue("@ClassID", classID);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static int GetActiveLicenseIDByApplicationID(int applicationID)
        {
            int licenseID = -1;
            string sql = "SELECT LicenseID FROM Licenses WHERE ApplicationID = @ApplicationID AND IsActive = 1";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        licenseID = id;
                    }
                }
            }
            return licenseID;
        }
    }
}