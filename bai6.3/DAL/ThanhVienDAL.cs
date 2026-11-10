
using System.Data;
using Microsoft.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class ThanhVienDAL
    {
        // Lấy danh sách thành viên
        public DataTable GetAll()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM ThanhVien ORDER BY MaThanhVien DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // Thêm thành viên
        public bool Insert(ThanhVienDTO tv)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"INSERT INTO ThanhVien
                    (HoTen, Email, ChuyenMon)
                    VALUES (@HoTen, @Email, @ChuyenMon)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100).Value =
                        tv.HoTen;

                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                        string.IsNullOrWhiteSpace(tv.Email)
                        ? DBNull.Value : (object)tv.Email;

                    cmd.Parameters.Add("@ChuyenMon", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(tv.ChuyenMon)
                        ? DBNull.Value : (object)tv.ChuyenMon;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Sửa thành viên
        public bool Update(ThanhVienDTO tv)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"UPDATE ThanhVien SET
                    HoTen = @HoTen,
                    Email = @Email,
                    ChuyenMon = @ChuyenMon
                    WHERE MaThanhVien = @MaThanhVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value =
                        tv.MaThanhVien;

                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar, 100).Value =
                        tv.HoTen;

                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value =
                        string.IsNullOrWhiteSpace(tv.Email)
                        ? DBNull.Value : (object)tv.Email;

                    cmd.Parameters.Add("@ChuyenMon", SqlDbType.NVarChar, 100).Value =
                        string.IsNullOrWhiteSpace(tv.ChuyenMon)
                        ? DBNull.Value : (object)tv.ChuyenMon;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Xóa thành viên
        public bool Delete(int maThanhVien)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"DELETE FROM ThanhVien
                                 WHERE MaThanhVien = @MaThanhVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value =
                        maThanhVien;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Tìm thành viên theo mã
        public DataTable GetById(int maThanhVien)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"SELECT * FROM ThanhVien
                                 WHERE MaThanhVien = @MaThanhVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value =
                        maThanhVien;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}
