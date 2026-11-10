
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class CongViecDAL
    {
        // Lấy tất cả công việc
        public DataTable GetAll()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT cv.*, da.TenDuAn
                    FROM CongViec cv
                    INNER JOIN DuAn da ON cv.MaDuAn = da.MaDuAn
                    ORDER BY cv.MaTask DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // Lọc công việc theo dự án
        public DataTable GetByProject(int maDuAn)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT * FROM CongViec
                    WHERE MaDuAn = @MaDuAn
                    ORDER BY MaTask DESC";

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

        // Lọc công việc theo dự án và trạng thái
        public DataTable GetByProjectAndStatus(
            int maDuAn, string trangThai)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT * FROM CongViec
                    WHERE MaDuAn = @MaDuAn
                    AND TrangThai = @TrangThai
                    ORDER BY HanNop ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value = maDuAn;

                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value =
                        trangThai;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // Thêm công việc
        public bool Insert(CongViecDTO cv)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO CongViec
                    (MaDuAn, TenTask, MoTa, HanNop, DoUuTien, TrangThai)
                    VALUES
                    (@MaDuAn, @TenTask, @MoTa, @HanNop, @DoUuTien, @TrangThai)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value =
                        cv.MaDuAn;

                    cmd.Parameters.Add("@TenTask", SqlDbType.NVarChar, 200).Value =
                        cv.TenTask;

                    cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, -1).Value =
                        string.IsNullOrWhiteSpace(cv.MoTa)
                        ? DBNull.Value : (object)cv.MoTa;

                    cmd.Parameters.Add("@HanNop", SqlDbType.DateTime).Value =
                        cv.HanNop.HasValue
                        ? (object)cv.HanNop.Value : DBNull.Value;

                    cmd.Parameters.Add("@DoUuTien", SqlDbType.NVarChar, 20).Value =
                        string.IsNullOrWhiteSpace(cv.DoUuTien)
                        ? DBNull.Value : (object)cv.DoUuTien;

                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value =
                        cv.TrangThai;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Sửa công việc
        public bool Update(CongViecDTO cv)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    UPDATE CongViec SET
                    MaDuAn = @MaDuAn,
                    TenTask = @TenTask,
                    MoTa = @MoTa,
                    HanNop = @HanNop,
                    DoUuTien = @DoUuTien,
                    TrangThai = @TrangThai
                    WHERE MaTask = @MaTask";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value =
                        cv.MaTask;

                    cmd.Parameters.Add("@MaDuAn", SqlDbType.Int).Value =
                        cv.MaDuAn;

                    cmd.Parameters.Add("@TenTask", SqlDbType.NVarChar, 200).Value =
                        cv.TenTask;

                    cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar, -1).Value =
                        string.IsNullOrWhiteSpace(cv.MoTa)
                        ? DBNull.Value : (object)cv.MoTa;

                    cmd.Parameters.Add("@HanNop", SqlDbType.DateTime).Value =
                        cv.HanNop.HasValue
                        ? (object)cv.HanNop.Value : DBNull.Value;

                    cmd.Parameters.Add("@DoUuTien", SqlDbType.NVarChar, 20).Value =
                        string.IsNullOrWhiteSpace(cv.DoUuTien)
                        ? DBNull.Value : (object)cv.DoUuTien;

                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value =
                        cv.TrangThai;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Xóa công việc
        public bool Delete(int maTask)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = "DELETE FROM CongViec WHERE MaTask = @MaTask";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Chuyển trạng thái công việc
        public bool UpdateStatus(int maTask, string trangThai)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    UPDATE CongViec
                    SET TrangThai = @TrangThai
                    WHERE MaTask = @MaTask";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 50).Value =
                        trangThai;

                    cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value = maTask;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Lấy danh sách công việc quá hạn bằng SqlDataReader
        public DataTable GetOverdueTasks()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        cv.MaTask,
                        cv.TenTask,
                        da.TenDuAn,
                        cv.HanNop,
                        cv.DoUuTien,
                        cv.TrangThai
                    FROM CongViec cv
                    INNER JOIN DuAn da
                    ON cv.MaDuAn = da.MaDuAn
                    WHERE cv.HanNop < GETDATE()
                    AND cv.TrangThai <> N'Done'
                    ORDER BY cv.HanNop ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }

            return dt;
        }

        // Thống kê tỷ lệ hoàn thành theo thành viên trong dự án
        public DataTable GetMemberProgress(int maDuAn)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        tv.MaThanhVien,
                        tv.HoTen,
                        COUNT(cv.MaTask) AS TongCongViec,
                        SUM(CASE
                            WHEN cv.TrangThai = N'Done' THEN 1
                            ELSE 0
                        END) AS CongViecHoanThanh,

                        CAST(
                            SUM(CASE
                                WHEN cv.TrangThai = N'Done' THEN 1.0
                                ELSE 0
                            END) * 100.0 /
                            NULLIF(COUNT(cv.MaTask), 0)
                            AS DECIMAL(5,2)
                        ) AS TiLeHoanThanh

                    FROM ThanhVien tv
                    INNER JOIN PhanCong pc
                    ON tv.MaThanhVien = pc.MaThanhVien

                    INNER JOIN CongViec cv
                    ON pc.MaTask = cv.MaTask

                    WHERE cv.MaDuAn = @MaDuAn

                    GROUP BY tv.MaThanhVien, tv.HoTen
                    ORDER BY tv.HoTen";

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

        // Thống kê tổng số công việc đã hoàn thành
        public int CountCompleted()
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM CongViec
                    WHERE TrangThai = N'Done'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}
