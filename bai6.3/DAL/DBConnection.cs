
using System;
using Microsoft.Data.SqlClient;

namespace ProjectTaskManagement.DAL
{
    public class DBConnection
    {
        private static string connectionString =
            @"Data Source=.;Initial Catalog=ProjectTaskDB;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
