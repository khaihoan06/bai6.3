
using System;
using System.Data;
using ProjectTaskManagement.DTO;
using ProjectTaskManagement.DAL;

namespace ProjectTaskManagement.BUS
{
    public class PhanCongBUS
    {
        private readonly PhanCongDAL dal = new PhanCongDAL();

        // Lấy toàn bộ danh sách phân công
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // Giao công việc cho thành viên
        public bool Insert(PhanCongDTO pc)
        {
            if (pc == null)
                throw new Exception("Thông tin phân công không được để trống!");

            if (pc.MaTask <= 0)
                throw new Exception("Vui lòng chọn công việc!");

            if (pc.MaThanhVien <= 0)
                throw new Exception("Vui lòng chọn thành viên!");

            // Kiểm tra trùng phân công
            if (dal.IsAssigned(pc.MaTask, pc.MaThanhVien))
            {
                throw new Exception(
                    "Thành viên này đã được phân công công việc!");
            }

            return dal.Insert(pc);
        }

        // Hủy phân công
        public bool Delete(int maTask, int maThanhVien)
        {
            if (maTask <= 0 || maThanhVien <= 0)
                throw new Exception("Thông tin phân công không hợp lệ!");

            return dal.Delete(maTask, maThanhVien);
        }

        // Lấy thành viên theo công việc
        public DataTable GetByTask(int maTask)
        {
            if (maTask <= 0)
                throw new Exception("Mã công việc không hợp lệ!");

            return dal.GetByTask(maTask);
        }

        // Lấy công việc theo thành viên
        public DataTable GetByMember(int maThanhVien)
        {
            if (maThanhVien <= 0)
                throw new Exception("Mã thành viên không hợp lệ!");

            return dal.GetByMember(maThanhVien);
        }

        // Kiểm tra đã phân công hay chưa
        public bool IsAssigned(int maTask, int maThanhVien)
        {
            if (maTask <= 0 || maThanhVien <= 0)
                return false;

            return dal.IsAssigned(maTask, maThanhVien);
        }
    }
}
