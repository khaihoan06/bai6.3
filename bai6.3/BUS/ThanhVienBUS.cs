
using System;
using System.Data;
using ProjectTaskManagement.DTO;
using ProjectTaskManagement.DAL;

namespace ProjectTaskManagement.BUS
{
    public class ThanhVienBUS
    {
        private readonly ThanhVienDAL dal = new ThanhVienDAL();

        // Lấy danh sách thành viên
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // Tìm thành viên theo mã
        public DataTable GetById(int maThanhVien)
        {
            if (maThanhVien <= 0)
                throw new Exception("Mã thành viên không hợp lệ!");

            return dal.GetById(maThanhVien);
        }

        // Thêm thành viên
        public bool Insert(ThanhVienDTO tv)
        {
            if (tv == null)
                throw new Exception("Thông tin thành viên không được để trống!");

            if (string.IsNullOrWhiteSpace(tv.HoTen))
                throw new Exception("Họ tên không được để trống!");

            if (tv.HoTen.Length > 100)
                throw new Exception("Họ tên không được vượt quá 100 ký tự!");

            if (!string.IsNullOrWhiteSpace(tv.Email))
            {
                if (tv.Email.Length > 100 || !tv.Email.Contains("@"))
                    throw new Exception("Email không hợp lệ!");
            }

            if (!string.IsNullOrWhiteSpace(tv.ChuyenMon) &&
                tv.ChuyenMon.Length > 100)
            {
                throw new Exception("Chuyên môn không được vượt quá 100 ký tự!");
            }

            return dal.Insert(tv);
        }

        // Sửa thành viên
        public bool Update(ThanhVienDTO tv)
        {
            if (tv == null)
                throw new Exception("Thông tin thành viên không được để trống!");

            if (tv.MaThanhVien <= 0)
                throw new Exception("Mã thành viên không hợp lệ!");

            if (string.IsNullOrWhiteSpace(tv.HoTen))
                throw new Exception("Họ tên không được để trống!");

            if (tv.HoTen.Length > 100)
                throw new Exception("Họ tên không được vượt quá 100 ký tự!");

            if (!string.IsNullOrWhiteSpace(tv.Email))
            {
                if (tv.Email.Length > 100 || !tv.Email.Contains("@"))
                    throw new Exception("Email không hợp lệ!");
            }

            if (!string.IsNullOrWhiteSpace(tv.ChuyenMon) &&
                tv.ChuyenMon.Length > 100)
            {
                throw new Exception("Chuyên môn không được vượt quá 100 ký tự!");
            }

            return dal.Update(tv);
        }

        // Xóa thành viên
        public bool Delete(int maThanhVien)
        {
            if (maThanhVien <= 0)
                throw new Exception("Mã thành viên không hợp lệ!");

            return dal.Delete(maThanhVien);
        }
    }
}
