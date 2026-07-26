using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Data
{
    public class clsApplication
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }

        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            const string sql = "SELECT * FROM Applications";
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
                            ApplicantPersonID = reader.GetInt32(reader.GetOrdinal("ApplicantPersonID"));
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

        public static int InsertApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            const string sql = "INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID) VALUES (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID); SELECT SCOPE_IDENTITY();";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    cmd.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    cmd.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
                    cmd.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    cn.Open();
                    int applicationID = Convert.ToInt32(cmd.ExecuteScalar());
                    return applicationID;
                }
            }
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            const string sql = "UPDATE Applications SET ApplicantPersonID = @ApplicantPersonID, ApplicationDate = @ApplicationDate, ApplicationTypeID = @ApplicationTypeID, ApplicationStatus = @ApplicationStatus, LastStatusDate = @LastStatusDate, PaidFees = @PaidFees, CreatedByUserID = @CreatedByUserID WHERE ApplicationID = @ApplicationID";
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
            string sql = "SELECT COUNT(*) FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID";
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

        public static bool UpdateApplicationStatus(int ApplicationID, byte NewStatus, DateTime LastStatusDate)
        {
            const string sql = "UPDATE Applications SET ApplicationStatus = @NewStatus, LastStatusDate = @LastStatusDate WHERE ApplicationID = @ApplicationID";
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@NewStatus", NewStatus);
                    cmd.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
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
            const string sql = "SELECT COUNT(*) FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID AND ApplicationStatus = 1 AND ApplicationTypeID = @ApplicationTypeID"; // Assuming 1 is the status for active applications
            using (SqlConnection cn = new SqlConnection(clsDataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ApplicantPersonID", personID);
                    cn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public static bool GetLatestApplicationByPerson(int personID, int ApplicationTypeID, ref int ApplicationID, ref DateTime ApplicationDate, ref byte ApplicationStatus, ref DateTime LastStatusDate, ref decimal PaidFees, ref int CreatedByUserID)
        {
            string sql = "SELECT TOP 1 * FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID AND ApplicationTypeID = @ApplicationTypeID ORDER BY ApplicationDate DESC";
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
                            ApplicationTypeID = reader.GetInt32(reader.GetOrdinal("ApplicationTypeID"));
                            ApplicationStatus = reader.GetByte(reader.GetOrdinal("ApplicationStatus"));
                            LastStatusDate = reader.GetDateTime(reader.GetOrdinal("LastStatusDate"));
                            PaidFees = reader.GetDecimal(reader.GetOrdinal("PaidFees"));
                            CreatedByUserID = reader.GetInt32(reader.GetOrdinal("CreatedByUserID"));
                            return true;

                        }
                      
                    }
                }

                return false;
            }
        }

    }
}
