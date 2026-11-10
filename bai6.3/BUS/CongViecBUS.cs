
using System;
using System.Data;
using ProjectTaskManagement.DTO;
using ProjectTaskManagement.DAL;

namespace ProjectTaskManagement.BUS
{
    public class CongViecBUS
    {
        private readonly CongViecDAL dal = new CongViecDAL();

        // Lấy tất cả công việc
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // Lọc công việc theo dự án
        public DataTable GetByProject(int maDuAn)
        {
            if (maDuAn <= 0)
                throw new Exception("Mã dự án không hợp lệ!");

            return dal.GetByProject(maDuAn);
        }

        // Lọc công việc theo dự án và trạng thái
        public DataTable GetByProjectAndStatus(
            int maDuAn, string trangThai)
        {
            if (maDuAn <= 0)
                throw new Exception("Vui lòng chọn dự án!");

            if (!IsValidStatus(trangThai))
                throw new Exception("Trạng thái công việc không hợp lệ!");

            return dal.GetByProjectAndStatus(maDuAn, trangThai);
        }

        // Kiểm tra trạng thái hợp lệ
        private bool IsValidStatus(string trangThai)
        {
            return trangThai == "To Do" ||
                   trangThai == "In Progress" ||
                   trangThai == "Review" ||
                   trangThai == "Done";
        }

        // Thêm công việc
        public bool Insert(CongViecDTO cv)
        {
            if (cv == null)
                throw new Exception("Thông tin công việc không được để trống!");

            if (cv.MaDuAn <= 0)
                throw new Exception("Vui lòng chọn dự án!");

            if (string.IsNullOrWhiteSpace(cv.TenTask))
                throw new Exception("Tên công việc không được để trống!");

            if (cv.TenTask.Length > 200)
                throw new Exception("Tên công việc không được vượt quá 200 ký tự!");

            if (!string.IsNullOrWhiteSpace(cv.MoTa) && cv.MoTa.Length > 100000)
                throw new Exception("Mô tả công việc quá dài!");

            if (cv.HanNop.HasValue &&
                cv.HanNop.Value < DateTime.Today)
            {
                throw new Exception("Hạn nộp không được nằm trong quá khứ!");
            }

            if (!IsValidPriority(cv.DoUuTien))
                throw new Exception("Độ ưu tiên không hợp lệ!");

            if (!IsValidStatus(cv.TrangThai))
                throw new Exception("Trạng thái công việc không hợp lệ!");

            return dal.Insert(cv);
        }

        // Kiểm tra độ ưu tiên
        private bool IsValidPriority(string doUuTien)
        {
            return doUuTien == "Cao" ||
                   doUuTien == "Trung bình" ||
                   doUuTien == "Thấp";
        }

        // Sửa công việc
        public bool Update(CongViecDTO cv)
        {
            if (cv == null)
                throw new Exception("Thông tin công việc không được để trống!");

            if (cv.MaTask <= 0)
                throw new Exception("Mã công việc không hợp lệ!");

            if (cv.MaDuAn <= 0)
                throw new Exception("Vui lòng chọn dự án!");

            if (string.IsNullOrWhiteSpace(cv.TenTask))
                throw new Exception("Tên công việc không được để trống!");

            if (cv.TenTask.Length > 200)
                throw new Exception("Tên công việc không được vượt quá 200 ký tự!");

            if (!string.IsNullOrWhiteSpace(cv.MoTa) && cv.MoTa.Length > 100000)
                throw new Exception("Mô tả công việc quá dài!");

            if (!IsValidPriority(cv.DoUuTien))
                throw new Exception("Độ ưu tiên không hợp lệ!");

            if (!IsValidStatus(cv.TrangThai))
                throw new Exception("Trạng thái công việc không hợp lệ!");

            return dal.Update(cv);
        }

        // Xóa công việc
        public bool Delete(int maTask)
        {
            if (maTask <= 0)
                throw new Exception("Mã công việc không hợp lệ!");

            return dal.Delete(maTask);
        }

        // Chuyển trạng thái công việc
        public bool UpdateStatus(int maTask, string trangThai)
        {
            if (maTask <= 0)
                throw new Exception("Mã công việc không hợp lệ!");

            if (!IsValidStatus(trangThai))
                throw new Exception("Trạng thái không hợp lệ!");

            return dal.UpdateStatus(maTask, trangThai);
        }

        // Lấy danh sách công việc quá hạn
        public DataTable GetOverdueTasks()
        {
            return dal.GetOverdueTasks();
        }

        // Thống kê tiến độ thành viên
        public DataTable GetMemberProgress(int maDuAn)
        {
            if (maDuAn <= 0)
                throw new Exception("Vui lòng chọn dự án!");

            return dal.GetMemberProgress(maDuAn);
        }

        // Đếm số công việc hoàn thành
        public int CountCompleted()
        {
            return dal.CountCompleted();
        }
    }
}
