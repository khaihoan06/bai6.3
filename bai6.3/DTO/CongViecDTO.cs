
using System;

namespace ProjectTaskManagement.DTO
{
    public class CongViecDTO
    {
        public int MaTask { get; set; }
        public int MaDuAn { get; set; }
        public string TenTask { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public DateTime? HanNop { get; set; }
        public string DoUuTien { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
    }
}
