
using System;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class FrmThanhVien : Form
    {
        private readonly ThanhVienBUS thanhVienBUS = new ThanhVienBUS();

        private DataGridView dgvThanhVien = null!;
        private TextBox txtHoTen = null!;
        private TextBox txtEmail = null!;
        private TextBox txtChuyenMon = null!;

        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        private int maThanhVienDangChon = 0;

        public FrmThanhVien()
        {
            InitializeComponent();
            LoadThanhVien();
        }

        private void InitializeComponent()
        {
            Text = "QUẢN LÝ THÀNH VIÊN";
            Size = new Size(1050, 680);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ THÀNH VIÊN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Location = new Point(25, 20),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            GroupBox grpThongTin = new GroupBox
            {
                Text = "Thông tin thành viên",
                Location = new Point(25, 70),
                Size = new Size(970, 190),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            Controls.Add(grpThongTin);

            AddLabel(grpThongTin, "Họ và tên:", 25, 35);
            txtHoTen = AddTextBox(grpThongTin, 140, 32, 300);

            AddLabel(grpThongTin, "Email:", 500, 35);
            txtEmail = AddTextBox(grpThongTin, 600, 32, 320);

            AddLabel(grpThongTin, "Chuyên môn:", 25, 85);
            txtChuyenMon = AddTextBox(grpThongTin, 140, 82, 780);

            btnThem = CreateButton("Thêm", 140, 130, Color.FromArgb(22, 163, 74));
            btnSua = CreateButton("Sửa", 260, 130, Color.FromArgb(37, 99, 235));
            btnXoa = CreateButton("Xóa", 380, 130, Color.FromArgb(220, 38, 38));
            btnLamMoi = CreateButton("Làm mới", 500, 130, Color.FromArgb(100, 116, 139));

            grpThongTin.Controls.Add(btnThem);
            grpThongTin.Controls.Add(btnSua);
            grpThongTin.Controls.Add(btnXoa);
            grpThongTin.Controls.Add(btnLamMoi);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            dgvThanhVien = new DataGridView
            {
                Location = new Point(25, 280),
                Size = new Size(970, 330),
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

            dgvThanhVien.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 41, 59);
            dgvThanhVien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvThanhVien.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);
            dgvThanhVien.EnableHeadersVisualStyles = false;
            dgvThanhVien.RowTemplate.Height = 35;

            dgvThanhVien.Columns.Add("MaThanhVien", "Mã thành viên");
            dgvThanhVien.Columns.Add("HoTen", "Họ và tên");
            dgvThanhVien.Columns.Add("Email", "Email");
            dgvThanhVien.Columns.Add("ChuyenMon", "Chuyên môn");

            dgvThanhVien.Columns["MaThanhVien"]!.FillWeight = 25;
            dgvThanhVien.Columns["HoTen"]!.FillWeight = 35;
            dgvThanhVien.Columns["Email"]!.FillWeight = 45;
            dgvThanhVien.Columns["ChuyenMon"]!.FillWeight = 35;

            dgvThanhVien.CellClick += DgvThanhVien_CellClick;

            Controls.Add(dgvThanhVien);
        }

        private void AddLabel(Control parent, string text, int x, int y)
        {
            Label lbl = new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            parent.Controls.Add(lbl);
        }

        private TextBox AddTextBox(Control parent, int x, int y, int width)
        {
            TextBox txt = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 30),
                Font = new Font("Segoe UI", 10)
            };

            parent.Controls.Add(txt);
            return txt;
        }

        private Button CreateButton(string text, int x, int y, Color color)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(105, 38),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        // Hiển thị danh sách thành viên

        private void LoadThanhVien()
        {
            try
            {
                dgvThanhVien.Rows.Clear();

                System.Data.DataTable danhSach = thanhVienBUS.GetAll();

                foreach (System.Data.DataRow row in danhSach.Rows)
                {
                    dgvThanhVien.Rows.Add(
                        row["MaThanhVien"],
                        row["HoTen"],
                        row["Email"],
                        row["ChuyenMon"]
                    );
                }

                dgvThanhVien.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách thành viên.\nChi tiết: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // Chọn một thành viên trong bảng
        private void DgvThanhVien_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvThanhVien.Rows[e.RowIndex];

            maThanhVienDangChon = Convert.ToInt32(
                row.Cells["MaThanhVien"].Value
            );

            txtHoTen.Text = Convert.ToString(
                row.Cells["HoTen"].Value
            ) ?? string.Empty;

            txtEmail.Text = Convert.ToString(
                row.Cells["Email"].Value
            ) ?? string.Empty;

            txtChuyenMon.Text = Convert.ToString(
                row.Cells["ChuyenMon"].Value
            ) ?? string.Empty;
        }

        // Thêm thành viên
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            try
            {
                ThanhVienDTO tv = new ThanhVienDTO
                {
                    HoTen = txtHoTen.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    ChuyenMon = txtChuyenMon.Text.Trim()
                };

                thanhVienBUS.Insert(tv);

                MessageBox.Show(
                    "Thêm thành viên thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ClearForm();
                LoadThanhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể thêm thành viên.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Sửa thành viên
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (maThanhVienDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên cần sửa!");
                return;
            }

            try
            {
                ThanhVienDTO tv = new ThanhVienDTO
                {
                    MaThanhVien = maThanhVienDangChon,
                    HoTen = txtHoTen.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    ChuyenMon = txtChuyenMon.Text.Trim()
                };

                thanhVienBUS.Update(tv);

                MessageBox.Show(
                    "Cập nhật thành viên thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ClearForm();
                LoadThanhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể cập nhật thành viên.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Xóa thành viên
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (maThanhVienDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn thành viên cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa thành viên này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                thanhVienBUS.Delete(maThanhVienDangChon);

                MessageBox.Show(
                    "Xóa thành viên thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                ClearForm();
                LoadThanhVien();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể xóa thành viên.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Làm mới
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearForm();
            LoadThanhVien();
        }

        private void ClearForm()
        {
            maThanhVienDangChon = 0;

            txtHoTen.Clear();
            txtEmail.Clear();
            txtChuyenMon.Clear();

            dgvThanhVien.ClearSelection();
            txtHoTen.Focus();
        }
    }
}
