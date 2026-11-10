
using System;
using System.Data;
using ProjectTaskManagement.DTO;
using ProjectTaskManagement.DAL;

namespace ProjectTaskManagement.BUS
{
    public class DuAnBUS
    {
        private readonly DuAnDAL dal = new DuAnDAL();

        // Lấy danh sách dự án
        public DataTable GetAll()
        {
            return dal.GetAll();
        }

        // Tìm dự án theo mã
        public DataTable GetById(int maDuAn)
        {
            if (maDuAn <= 0)
                throw new Exception("Mã dự án không hợp lệ!");

            return dal.GetById(maDuAn);
        }

        // Thêm dự án
        public bool Insert(DuAnDTO duAn)
        {
            if (duAn == null)
                throw new Exception("Thông tin dự án không được để trống!");

            if (string.IsNullOrWhiteSpace(duAn.TenDuAn))
                throw new Exception("Tên dự án không được để trống!");

            if (duAn.TenDuAn.Length > 150)
                throw new Exception("Tên dự án không được vượt quá 150 ký tự!");

            if (duAn.NgayBatDau.HasValue &&
                duAn.NgayKetThuc.HasValue &&
                duAn.NgayKetThuc.Value < duAn.NgayBatDau.Value)
            {
                throw new Exception("Ngày kết thúc phải sau ngày bắt đầu!");
            }

            return dal.Insert(duAn);
        }

        // Sửa dự án
        public bool Update(DuAnDTO duAn)
        {
            if (duAn == null)
                throw new Exception("Thông tin dự án không được để trống!");

            if (duAn.MaDuAn <= 0)
                throw new Exception("Mã dự án không hợp lệ!");

            if (string.IsNullOrWhiteSpace(duAn.TenDuAn))
                throw new Exception("Tên dự án không được để trống!");

            if (duAn.TenDuAn.Length > 150)
                throw new Exception("Tên dự án không được vượt quá 150 ký tự!");

            if (duAn.NgayBatDau.HasValue &&
                duAn.NgayKetThuc.HasValue &&
                duAn.NgayKetThuc.Value < duAn.NgayBatDau.Value)
            {
                throw new Exception("Ngày kết thúc phải sau ngày bắt đầu!");
            }

            return dal.Update(duAn);
        }

        // Xóa dự án
        public bool Delete(int maDuAn)
        {
            if (maDuAn <= 0)
                throw new Exception("Mã dự án không hợp lệ!");

            return dal.Delete(maDuAn);
        }
    }
}
