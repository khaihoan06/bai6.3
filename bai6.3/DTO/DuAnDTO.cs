
using System;

namespace ProjectTaskManagement.DTO
{
    public class DuAnDTO
    {
        public int MaDuAn { get; set; }

        public string TenDuAn { get; set; } = string.Empty;

        public DateTime? NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        public string TrangThai { get; set; } = string.Empty;
    }
}
