
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class FrmDuAn : Form
    {
        private readonly DuAnBUS bus = new DuAnBUS();

        private TextBox txtTenDuAn = null!;
        private DateTimePicker dtpNgayBatDau = null!;
        private DateTimePicker dtpNgayKetThuc = null!;
        private ComboBox cboTrangThai = null!;
        private DataGridView dgvDuAn = null!;

        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        private int maDuAnDangChon = 0;

        public FrmDuAn()
        {
            InitializeComponent();
            LoadData();
        }

        // KHỞI TẠO GIAO DIỆN
        private void InitializeComponent()
        {
            Text = "QUẢN LÝ DỰ ÁN";
            Size = new Size(1100, 700);
            MinimumSize = new Size(950, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);

            // TIÊU ĐỀ
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ DỰ ÁN",
                Dock = DockStyle.Top,
                Height = 65,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                BackColor = Color.White
            };

            Controls.Add(lblTitle);

            // NHÓM THÔNG TIN
            GroupBox grpThongTin = new GroupBox
            {
                Text = "Thông tin dự án",
                Dock = DockStyle.Top,
                Height = 205,
                Padding = new Padding(15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Controls.Add(grpThongTin);

            // TÊN DỰ ÁN
            Label lblTen = new Label
            {
                Text = "Tên dự án:",
                Location = new Point(20, 35),
                AutoSize = true
            };

            grpThongTin.Controls.Add(lblTen);

            txtTenDuAn = new TextBox
            {
                Location = new Point(130, 30),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10)
            };

            grpThongTin.Controls.Add(txtTenDuAn);

            // NGÀY BẮT ĐẦU
            Label lblBatDau = new Label
            {
                Text = "Ngày bắt đầu:",
                Location = new Point(480, 35),
                AutoSize = true
            };

            grpThongTin.Controls.Add(lblBatDau);

            dtpNgayBatDau = new DateTimePicker
            {
                Location = new Point(600, 30),
                Size = new Size(220, 30),
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false
            };

            grpThongTin.Controls.Add(dtpNgayBatDau);

            // NGÀY KẾT THÚC
            Label lblKetThuc = new Label
            {
                Text = "Ngày kết thúc:",
                Location = new Point(20, 80),
                AutoSize = true
            };

            grpThongTin.Controls.Add(lblKetThuc);

            dtpNgayKetThuc = new DateTimePicker
            {
                Location = new Point(130, 75),
                Size = new Size(300, 30),
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false
            };

            grpThongTin.Controls.Add(dtpNgayKetThuc);

            // TRẠNG THÁI
            Label lblTrangThai = new Label
            {
                Text = "Trạng thái:",
                Location = new Point(480, 80),
                AutoSize = true
            };

            grpThongTin.Controls.Add(lblTrangThai);

            cboTrangThai = new ComboBox
            {
                Location = new Point(600, 75),
                Size = new Size(220, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            cboTrangThai.Items.AddRange(new object[]
            {
                "Chưa bắt đầu",
                "Đang thực hiện",
                "Hoàn thành",
                "Tạm dừng"
            });

            cboTrangThai.SelectedIndex = 0;

            grpThongTin.Controls.Add(cboTrangThai);

            // CÁC NÚT
            btnThem = CreateButton("Thêm", 20, 130);
            btnSua = CreateButton("Sửa", 145, 130);
            btnXoa = CreateButton("Xóa", 270, 130);
            btnLamMoi = CreateButton("Làm mới", 395, 130);

            grpThongTin.Controls.Add(btnThem);
            grpThongTin.Controls.Add(btnSua);
            grpThongTin.Controls.Add(btnXoa);
            grpThongTin.Controls.Add(btnLamMoi);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            // BẢNG DỮ LIỆU
            dgvDuAn = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill,

                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,

                SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect,

                MultiSelect = false,
                RowHeadersVisible = false,

                Font = new Font("Segoe UI", 10)
            };

            dgvDuAn.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvDuAn.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 41, 59);

            dgvDuAn.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvDuAn.EnableHeadersVisualStyles = false;
            dgvDuAn.RowTemplate.Height = 35;

            dgvDuAn.CellClick += DgvDuAn_CellClick;

            Controls.Add(dgvDuAn);

            dgvDuAn.BringToFront();
            grpThongTin.BringToFront();
            lblTitle.BringToFront();
        }

        // TẠO NÚT
        private Button CreateButton(string text, int x, int y)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(110, 38),

                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,

                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),

                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            return btn;
        }

        // TẢI DỮ LIỆU
        private void LoadData()
        {
            try
            {
                DataTable data = bus.GetAll();

                dgvDuAn.DataSource = data;

                SetColumnHeader("MaDuAn", "Mã dự án");
                SetColumnHeader("TenDuAn", "Tên dự án");
                SetColumnHeader("NgayBatDau", "Ngày bắt đầu");
                SetColumnHeader("NgayKetThuc", "Ngày kết thúc");
                SetColumnHeader("TrangThai", "Trạng thái");

                if (dgvDuAn.Columns.Contains("NgayBatDau"))
                {
                    dgvDuAn.Columns["NgayBatDau"]!.DefaultCellStyle.Format =
                        "dd/MM/yyyy";
                }

                if (dgvDuAn.Columns.Contains("NgayKetThuc"))
                {
                    dgvDuAn.Columns["NgayKetThuc"]!.DefaultCellStyle.Format =
                        "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách dự án!\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ĐỔI TÊN CỘT
        private void SetColumnHeader(string columnName, string header)
        {
            if (dgvDuAn.Columns.Contains(columnName))
            {
                dgvDuAn.Columns[columnName]!.HeaderText = header;
            }
        }

        // LẤY DỮ LIỆU TỪ FORM
        private DuAnDTO GetDuAnFromForm()
        {
            return new DuAnDTO
            {
                MaDuAn = maDuAnDangChon,

                TenDuAn = txtTenDuAn.Text.Trim(),

                NgayBatDau = dtpNgayBatDau.Checked
                    ? dtpNgayBatDau.Value
                    : null,

                NgayKetThuc = dtpNgayKetThuc.Checked
                    ? dtpNgayKetThuc.Value
                    : null,

                TrangThai =
                    cboTrangThai.SelectedItem?.ToString()
                    ?? "Chưa bắt đầu"
            };
        }

        // KIỂM TRA DỮ LIỆU
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenDuAn.Text))
            {
                MessageBox.Show("Vui lòng nhập tên dự án!");

                txtTenDuAn.Focus();

                return false;
            }

            if (txtTenDuAn.Text.Trim().Length > 150)
            {
                MessageBox.Show(
                    "Tên dự án không được vượt quá 150 ký tự!");

                txtTenDuAn.Focus();

                return false;
            }

            if (dtpNgayBatDau.Checked &&
                dtpNgayKetThuc.Checked &&
                dtpNgayKetThuc.Value.Date <
                dtpNgayBatDau.Value.Date)
            {
                MessageBox.Show(
                    "Ngày kết thúc không được trước ngày bắt đầu!");

                return false;
            }

            return true;
        }

        // THÊM DỰ ÁN
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                DuAnDTO duAn = GetDuAnFromForm();

                if (bus.Insert(duAn))
                {
                    MessageBox.Show("Thêm dự án thành công!");

                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Thêm dự án thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi thêm dự án:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // SỬA DỰ ÁN
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (maDuAnDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn dự án cần sửa!");
                return;
            }

            if (!ValidateInput())
                return;

            try
            {
                DuAnDTO duAn = GetDuAnFromForm();

                if (bus.Update(duAn))
                {
                    MessageBox.Show("Cập nhật dự án thành công!");

                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật dự án thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi cập nhật dự án:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // XÓA DỰ ÁN
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (maDuAnDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn dự án cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa dự án này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (bus.Delete(maDuAnDangChon))
                {
                    MessageBox.Show("Xóa dự án thành công!");

                    LoadData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Xóa dự án thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xóa dự án:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // CHỌN DỰ ÁN TRONG BẢNG
        private void DgvDuAn_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.RowIndex >= dgvDuAn.Rows.Count)
            {
                return;
            }

            DataGridViewRow row = dgvDuAn.Rows[e.RowIndex];

            if (!dgvDuAn.Columns.Contains("MaDuAn") ||
                !dgvDuAn.Columns.Contains("TenDuAn"))
            {
                return;
            }

            object? ma = row.Cells["MaDuAn"].Value;

            if (ma == null || ma == DBNull.Value)
                return;

            if (!int.TryParse(ma.ToString(), out int id))
                return;

            maDuAnDangChon = id;

            txtTenDuAn.Text =
                row.Cells["TenDuAn"].Value?.ToString() ?? "";

            if (dgvDuAn.Columns.Contains("NgayBatDau"))
            {
                SetDatePickerValue(
                    dtpNgayBatDau,
                    row.Cells["NgayBatDau"].Value);
            }

            if (dgvDuAn.Columns.Contains("NgayKetThuc"))
            {
                SetDatePickerValue(
                    dtpNgayKetThuc,
                    row.Cells["NgayKetThuc"].Value);
            }

            string trangThai = "Chưa bắt đầu";

            if (dgvDuAn.Columns.Contains("TrangThai"))
            {
                trangThai =
                    row.Cells["TrangThai"].Value?.ToString()
                    ?? "Chưa bắt đầu";
            }

            int index = cboTrangThai.Items.IndexOf(trangThai);

            cboTrangThai.SelectedIndex =
                index >= 0 ? index : 0;
        }

        // GÁN NGÀY CHO DATETIMEPICKER
        private void SetDatePickerValue(
            DateTimePicker picker,
            object? value)
        {
            if (value == null || value == DBNull.Value)
            {
                picker.Checked = false;
                return;
            }

            if (DateTime.TryParse(
                value.ToString(),
                out DateTime date))
            {
                picker.Value = date;
                picker.Checked = true;
            }
            else
            {
                picker.Checked = false;
            }
        }

        // LÀM MỚI
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            ClearForm();
            LoadData();
        }

        // XÓA DỮ LIỆU TRÊN FORM
        private void ClearForm()
        {
            maDuAnDangChon = 0;

            txtTenDuAn.Clear();

            dtpNgayBatDau.Checked = false;
            dtpNgayKetThuc.Checked = false;

            cboTrangThai.SelectedIndex = 0;

            dgvDuAn.ClearSelection();

            txtTenDuAn.Focus();
        }
    }
}
