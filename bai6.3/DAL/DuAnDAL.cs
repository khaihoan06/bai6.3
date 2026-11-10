
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class DuAnDAL
    {
        // Lấy danh sách dự án
        public DataTable GetAll()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM DuAn ORDER BY MaDuAn DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // Thêm dự án
        public bool Insert(DuAnDTO duAn)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"INSERT INTO DuAn
                    (TenDuAn, NgayBatDau, NgayKetThuc, TrangThai)
                    VALUES
                    (@TenDuAn, @NgayBatDau, @NgayKetThuc, @TrangThai)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TenDuAn", SqlDbType.NVarChar, 150).Value =
                        duAn.TenDuAn;

                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.DateTime).Value =
                        duAn.NgayBatDau.HasValue
                        ? (object)duAn.NgayBatDau.Value
                        : DBNull.Value;

                    cmd.Parameters.Add("@NgayKetThuc", SqlDbType.DateTime).Value =
                        duAn.NgayKetThuc.HasValue
                        ? (object)duAn.NgayKetThuc.Value
                        : DBNull.Value;

                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value =
                        string.IsNullOrWhiteSpace(duAn.TrangThai)
                        ? DBNull.Value
                        : (object)duAn.TrangThai;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Sửa dự án
        public bool Update(DuAnDTO duAn)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"UPDATE DuAn SET
                    TenDuAn = @TenDuAn,
                    NgayBatDau = @NgayBatDau,
                    NgayKetThuc = @NgayKetThuc,
                    TrangThai = @TrangThai
                    WHERE MaDuAn = @MaDuAn";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value =
                        duAn.MaDuAn;

                    cmd.Parameters.Add("@TenDuAn", SqlDbType.NVarChar, 150).Value =
                        duAn.TenDuAn;

                    cmd.Parameters.Add("@NgayBatDau", SqlDbType.DateTime).Value =
                        duAn.NgayBatDau.HasValue
                        ? (object)duAn.NgayBatDau.Value
                        : DBNull.Value;

                    cmd.Parameters.Add("@NgayKetThuc", SqlDbType.DateTime).Value =
                        duAn.NgayKetThuc.HasValue
                        ? (object)duAn.NgayKetThuc.Value
                        : DBNull.Value;

                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value =
                        string.IsNullOrWhiteSpace(duAn.TrangThai)
                        ? DBNull.Value
                        : (object)duAn.TrangThai;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Xóa dự án
        public bool Delete(int maDuAn)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM DuAn WHERE MaDuAn = @MaDuAn";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Tìm dự án theo mã
        public DataTable GetById(int maDuAn)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "SELECT * FROM DuAn WHERE MaDuAn = @MaDuAn";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn;

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
