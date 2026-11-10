
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.DAL
{
    public class PhanCongDAL
    {
        // Lấy toàn bộ danh sách phân công
        public DataTable GetAll()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        pc.MaTask,
                        cv.TenTask,
                        pc.MaThanhVien,
                        tv.HoTen,
                        pc.NgayGiao
                    FROM PhanCong pc
                    INNER JOIN CongViec cv
                        ON pc.MaTask = cv.MaTask
                    INNER JOIN ThanhVien tv
                        ON pc.MaThanhVien = tv.MaThanhVien
                    ORDER BY pc.NgayGiao DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // Giao công việc cho thành viên
        public bool Insert(PhanCongDTO pc)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    INSERT INTO PhanCong
                    (MaTask, MaThanhVien)
                    VALUES
                    (@MaTask, @MaThanhVien)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value =
                        pc.MaTask;

                    cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value =
                        pc.MaThanhVien;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Hủy phân công
        public bool Delete(int maTask, int maThanhVien)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    DELETE FROM PhanCong
                    WHERE MaTask = @MaTask
                    AND MaThanhVien = @MaThanhVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value =
                        maTask;

                    cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value =
                        maThanhVien;

                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Lấy danh sách thành viên của một công việc
        public DataTable GetByTask(int maTask)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        tv.MaThanhVien,
                        tv.HoTen,
                        tv.Email,
                        tv.ChuyenMon,
                        pc.NgayGiao
                    FROM PhanCong pc
                    INNER JOIN ThanhVien tv
                        ON pc.MaThanhVien = tv.MaThanhVien
                    WHERE pc.MaTask = @MaTask";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value =
                        maTask;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        // Lấy danh sách công việc của một thành viên
        public DataTable GetByMember(int maThanhVien)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT
                        cv.MaTask,
                        cv.TenTask,
                        cv.MaDuAn,
                        cv.HanNop,
                        cv.DoUuTien,
                        cv.TrangThai,
                        pc.NgayGiao
                    FROM PhanCong pc
                    INNER JOIN CongViec cv
                        ON pc.MaTask = cv.MaTask
                    WHERE pc.MaThanhVien = @MaThanhVien
                    ORDER BY cv.HanNop ASC";

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

        // Kiểm tra thành viên đã được giao công việc chưa
        public bool IsAssigned(int maTask, int maThanhVien)
        {
            using (SqlConnection conn = DBConnection.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM PhanCong
                    WHERE MaTask = @MaTask
                    AND MaThanhVien = @MaThanhVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MaTask", SqlDbType.Int).Value =
                        maTask;

                    cmd.Parameters.Add("@MaThanhVien", SqlDbType.Int).Value =
                        maThanhVien;

                    conn.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
    }
}
