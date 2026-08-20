using System;
using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class clsApplication
    {
        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM Applications ORDER BY ApplicationDate DESC";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static bool GetApplication(int applicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            const string sql = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ApplicantPersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                            ApplicationDate = reader.GetDateTime(reader.GetOrdinal("ApplicationDate"));
                            ApplicationTypeID = reader.GetInt32(reader.GetOrdinal("ApplicationTypeID"));
                            ApplicationStatus = reader.GetByte(reader.GetOrdinal("ApplicationStatus"));
                            LastStatusDate = reader.GetDateTime(reader.GetOrdinal("LastStatusDate"));
                            PaidFees = reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                            CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // تم توحيد الاسم ليطابق الـ Business Layer
        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            const string sql = @"INSERT INTO Applications 
                                (PersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID) 
                                VALUES 
                                (@PersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                                SELECT SCOPE_IDENTITY();";

            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("PersonID", ApplicantPersonID);
                    cmd.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    return (result != null && int.TryParse(result.ToString(), out int insertedID)) ? insertedID : -1;
                }
            }
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            const string sql = @"UPDATE Applications 
                                SET ApplicantPersonID = @ApplicantPersonID, 
                                    ApplicationDate = @ApplicationDate, 
                                    ApplicationTypeID = @ApplicationTypeID, 
                                    ApplicationStatus = @ApplicationStatus, 
                                    LastStatusDate = @LastStatusDate, 
                                    PaidFees = @PaidFees, 
                                    CreatedByUserID = @CreatedByUserID 
                                WHERE ApplicationID = @ApplicationID";

            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    cmd.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            const string sql = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static DataTable GetApplicationsByStatus(byte status)
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM Applications WHERE ApplicationStatus = @ApplicationStatus";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationStatus", status);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static DataTable GetApplicationsByApplicant(int applicantPersonID)
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static bool DoesHaveApplicant(int applicantID)
        {
            const string sql = "SELECT COUNT(*) FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", applicantID);
                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool UpdateStatus(int applicationID, byte status)
        {
            const string sql = "UPDATE Applications SET ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate WHERE ApplicationID = @ApplicationID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                    cmd.Parameters.AddWithValue("@ApplicationStatus", status);
                    cmd.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool UpdatePaidFees(int ApplicationID, decimal PaidFees)
        {
            const string sql = "UPDATE Applications SET PaidFees = @PaidFees WHERE ApplicationID = @ApplicationID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool DoesPersonHaveActiveApplication(int personID, int ApplicationTypeID)
        {
            return (GetActiveApplicationID(personID, ApplicationTypeID) != -1);
        }

        public static int GetActiveApplicationID(int personID, int applicationTypeID)
        {
            int activeApplicationID = -1;
            const string sql = @"SELECT ActiveApplicationID = ApplicationID 
                                FROM Applications 
                                WHERE ApplicantPersonID = @ApplicantPersonID 
                                  AND ApplicationTypeID = @ApplicationTypeID 
                                  AND ApplicationStatus = 1"; // 1 = New

            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", personID);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        activeApplicationID = id;
                    }
                }
            }
            return activeApplicationID;
        }

        // 👈 دالة هامة جداً لمنع تكرار طلب الرخصة المحلي لنفس الشخص ونفس الفئة
        public static int GetActiveApplicationIDForLicenseClass(int personID, int applicationTypeID, int licenseClassID)
        {
            int activeApplicationID = -1;
            const string sql = @"SELECT Applications.ApplicationID 
                                FROM Applications 
                                INNER JOIN LocalDrivingLicenseApplications 
                                    ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                                WHERE Applications.ApplicantPersonID = @ApplicantPersonID 
                                  AND Applications.ApplicationTypeID = @ApplicationTypeID 
                                  AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID 
                                  AND Applications.ApplicationStatus = 1"; // 1 = New

            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", personID);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
                    cmd.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        activeApplicationID = id;
                    }
                }
            }
            return activeApplicationID;
        }

        public static bool GetLatestApplicationByPerson(int personID, int ApplicationTypeID, ref int ApplicationID, ref DateTime ApplicationDate, ref byte ApplicationStatus, ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            const string sql = "SELECT TOP 1 * FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID AND ApplicationTypeID = @ApplicationTypeID ORDER BY ApplicationDate DESC";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", personID);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ApplicationID = reader.GetInt32(reader.GetOrdinal("ApplicationID"));
                            ApplicationDate = reader.GetDateTime(reader.GetOrdinal("ApplicationDate"));
                            ApplicationStatus = reader.GetByte(reader.GetOrdinal("ApplicationStatus"));
                            LastStatusDate = reader.GetDateTime(reader.GetOrdinal("LastStatusDate"));
                            PaidFees = reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                            CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static DataTable GetLocalDrivingApplications()
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM LocalDrivingLicenseApplications_View;";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static DataTable GetInternationalDrivingApplications()
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM InternationalLicenseApplications_View;";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static bool IsApplicationExist(int applicationID)
        {
            const string sql = "SELECT COUNT(*) FROM Applications WHERE ApplicationID = @ApplicationID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static byte GetPassedTestCount(int localDrivingLicenseApplicationID)
        {
            byte passedCount = 0;
            const string sql = @"SELECT COUNT(DISTINCT TestTypeID) 
                         FROM TestAppointments 
                         INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                         WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                           AND Tests.TestResult = 1";

            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
                    cn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && byte.TryParse(result.ToString(), out byte count))
                    {
                        passedCount = count;
                    }
                }
            }
            return passedCount;
        }
        public static int GetActiveLicenseIDByApplicationID(int applicationID)
        {
            int licenseID = -1;
            const string sql = "SELECT LicenseID FROM Licenses WHERE ApplicationID = @ApplicationID";

            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
                    cn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                    {
                        licenseID = id;
                    }
                }
            }
            return licenseID;
        }

        public static bool GetApplicationInfoByLocalDrivingAppID(int localDrivingLicenseApplicationID,
    ref int applicationID, ref int applicantPersonID, ref DateTime applicationDate,
    ref int applicationTypeID, ref byte applicationStatus, ref DateTime lastStatusDate,
    ref decimal paidFees, ref int createdByUserID, ref int licenseClassID)
        {
            bool isFound = false;
            const string sql = @"SELECT Applications.*, LocalDrivingLicenseApplications.LicenseClassID
                        FROM Applications 
                        INNER JOIN LocalDrivingLicenseApplications 
                            ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            applicationID = (int)reader["ApplicationID"];
                            applicantPersonID = (int)reader["PersonID"];
                            applicationDate = (DateTime)reader["ApplicationDate"];
                            applicationTypeID = (int)reader["ApplicationTypeID"];
                            applicationStatus = (byte)reader["ApplicationStatus"];
                            lastStatusDate = (DateTime)reader["LastStatusDate"];
                            paidFees = (decimal)reader["PaidFees"];
                            createdByUserID = (int)reader["CreatedByUserID"];
                            licenseClassID = (int)reader["LicenseClassID"];
                        }
                    }
                }
            }
            return isFound;
        }
    }
}
