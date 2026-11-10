
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class FrmPhanCong : Form
    {
        private readonly PhanCongBUS phanCongBUS = new PhanCongBUS();
        private readonly CongViecBUS congViecBUS = new CongViecBUS();
        private readonly ThanhVienBUS thanhVienBUS = new ThanhVienBUS();

        private ComboBox cboCongViec = null!;
        private ComboBox cboThanhVien = null!;
        private DataGridView dgvPhanCong = null!;

        private Button btnPhanCong = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        public FrmPhanCong()
        {
            InitializeComponent();
            LoadComboBox();
            LoadPhanCong();
        }

        private void InitializeComponent()
        {
            Text = "QUẢN LÝ PHÂN CÔNG CÔNG VIỆC";
            Size = new Size(1050, 680);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label
            {
                Text = "PHÂN CÔNG CÔNG VIỆC",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(25, 20),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            GroupBox grpPhanCong = new GroupBox
            {
                Text = "Thông tin phân công",
                Location = new Point(25, 75),
                Size = new Size(970, 190),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Controls.Add(grpPhanCong);

            Label lblCongViec = new Label
            {
                Text = "Công việc:",
                Location = new Point(25, 40),
                AutoSize = true
            };
            grpPhanCong.Controls.Add(lblCongViec);

            cboCongViec = new ComboBox
            {
                Location = new Point(140, 35),
                Size = new Size(760, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            grpPhanCong.Controls.Add(cboCongViec);

            Label lblThanhVien = new Label
            {
                Text = "Thành viên:",
                Location = new Point(25, 85),
                AutoSize = true
            };
            grpPhanCong.Controls.Add(lblThanhVien);

            cboThanhVien = new ComboBox
            {
                Location = new Point(140, 80),
                Size = new Size(760, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            grpPhanCong.Controls.Add(cboThanhVien);

            btnPhanCong = CreateButton(
                "Phân công", 140, 130, Color.FromArgb(22, 163, 74));

            btnXoa = CreateButton(
                "Xóa phân công", 270, 130, Color.FromArgb(220, 38, 38));

            btnLamMoi = CreateButton(
                "Làm mới", 430, 130, Color.FromArgb(100, 116, 139));

            grpPhanCong.Controls.Add(btnPhanCong);
            grpPhanCong.Controls.Add(btnXoa);
            grpPhanCong.Controls.Add(btnLamMoi);

            btnPhanCong.Click += BtnPhanCong_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            Label lblDanhSach = new Label
            {
                Text = "DANH SÁCH PHÂN CÔNG",
                Location = new Point(25, 285),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            Controls.Add(lblDanhSach);

            dgvPhanCong = new DataGridView
            {
                Location = new Point(25, 320),
                Size = new Size(970, 290),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10)
            };

            dgvPhanCong.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 41, 59);

            dgvPhanCong.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPhanCong.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvPhanCong.EnableHeadersVisualStyles = false;
            dgvPhanCong.RowTemplate.Height = 35;

            dgvPhanCong.Columns.Add("MaTask", "Mã công việc");
            dgvPhanCong.Columns.Add("MaThanhVien", "Mã thành viên");
            dgvPhanCong.Columns.Add("NgayGiao", "Ngày giao");

            Controls.Add(dgvPhanCong);
        }

        private Button CreateButton(
            string text, int x, int y, Color color)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(140, 38),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // Tải danh sách công việc và thành viên vào ComboBox
        private void LoadComboBox()
        {
            try
            {
                DataTable danhSachCongViec = congViecBUS.GetAll();

                cboCongViec.DataSource = danhSachCongViec;
                cboCongViec.DisplayMember = "TenTask";
                cboCongViec.ValueMember = "MaTask";

                DataTable danhSachThanhVien = thanhVienBUS.GetAll();

                cboThanhVien.DataSource = danhSachThanhVien;
                cboThanhVien.DisplayMember = "HoTen";
                cboThanhVien.ValueMember = "MaThanhVien";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải dữ liệu công việc hoặc thành viên.\n" +
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Lấy mã công việc đang chọn
        private int GetMaTask()
        {
            if (cboCongViec.SelectedValue == null ||
                cboCongViec.SelectedValue is DataRowView)
            {
                return 0;
            }

            return Convert.ToInt32(cboCongViec.SelectedValue);
        }

        // Lấy mã thành viên đang chọn
        private int GetMaThanhVien()
        {
            if (cboThanhVien.SelectedValue == null ||
                cboThanhVien.SelectedValue is DataRowView)
            {
                return 0;
            }

            return Convert.ToInt32(cboThanhVien.SelectedValue);
        }

        // Hiển thị danh sách phân công
        private void LoadPhanCong()
        {
            try
            {
                dgvPhanCong.Rows.Clear();

                DataTable danhSach = phanCongBUS.GetAll();

                foreach (DataRow row in danhSach.Rows)
                {
                    object ngayGiao = row["NgayGiao"];

                    string ngayGiaoText = ngayGiao == DBNull.Value
                        ? ""
                        : Convert.ToDateTime(ngayGiao).ToString("dd/MM/yyyy");

                    dgvPhanCong.Rows.Add(
                        row["MaTask"],
                        row["MaThanhVien"],
                        ngayGiaoText
                    );
                }

                dgvPhanCong.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách phân công.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Thực hiện phân công
        private void BtnPhanCong_Click(object? sender, EventArgs e)
        {
            int maTask = GetMaTask();
            int maThanhVien = GetMaThanhVien();

            if (maTask <= 0)
            {
                MessageBox.Show("Vui lòng chọn công việc!");
                return;
            }

            if (maThanhVien <= 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên!");
                return;
            }

            try
            {
                PhanCongDTO pc = new PhanCongDTO
                {
                    MaTask = maTask,
                    MaThanhVien = maThanhVien,
                    NgayGiao = DateTime.Now
                };

                phanCongBUS.Insert(pc);

                MessageBox.Show(
                    "Phân công công việc thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadPhanCong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể phân công công việc.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Xóa phân công đang chọn
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (dgvPhanCong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn phân công cần xóa!");
                return;
            }

            DataGridViewRow row = dgvPhanCong.SelectedRows[0];

            int maTask = Convert.ToInt32(
                row.Cells["MaTask"].Value
            );

            int maThanhVien = Convert.ToInt32(
                row.Cells["MaThanhVien"].Value
            );

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa phân công này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                phanCongBUS.Delete(maTask, maThanhVien);

                MessageBox.Show(
                    "Xóa phân công thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadPhanCong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa phân công.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Làm mới dữ liệu
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            LoadComboBox();
            LoadPhanCong();
        }
    }
}
