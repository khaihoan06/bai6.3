
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;
using ProjectTaskManagement.DTO;

namespace ProjectTaskManagement.GUI
{
    public class FrmCongViec : Form
    {
        private readonly CongViecBUS congViecBUS = new CongViecBUS();
        private readonly DuAnBUS duAnBUS = new DuAnBUS();

        private ComboBox cboDuAn = null!;
        private TextBox txtTenTask = null!;
        private TextBox txtMoTa = null!;
        private DateTimePicker dtpHanNop = null!;
        private ComboBox cboDoUuTien = null!;
        private ComboBox cboTrangThai = null!;
        private DataGridView dgvCongViec = null!;

        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;

        private int maTaskDangChon = 0;

        public FrmCongViec()
        {
            InitializeComponent();
            LoadProjects();
        }

        // KHỞI TẠO GIAO DIỆN
        private void InitializeComponent()
        {
            Text = "QUẢN LÝ CÔNG VIỆC";
            Size = new Size(1200, 750);
            MinimumSize = new Size(1050, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);

            // TIÊU ĐỀ
            Label lblTitle = new Label
            {
                Text = "QUẢN LÝ CÔNG VIỆC",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                BackColor = Color.White
            };

            Controls.Add(lblTitle);

            // NHÓM THÔNG TIN
            GroupBox grpThongTin = new GroupBox
            {
                Text = "Thông tin công việc",
                Dock = DockStyle.Top,
                Height = 300,
                Padding = new Padding(15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            Controls.Add(grpThongTin);

            // CHỌN DỰ ÁN
            AddLabel(grpThongTin, "Dự án:", 20, 35);

            cboDuAn = new ComboBox
            {
                Location = new Point(130, 30),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            grpThongTin.Controls.Add(cboDuAn);
            cboDuAn.SelectedIndexChanged += CboDuAn_SelectedIndexChanged;

            // TÊN CÔNG VIỆC
            AddLabel(grpThongTin, "Tên công việc:", 480, 35);

            txtTenTask = new TextBox
            {
                Location = new Point(600, 30),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10)
            };

            grpThongTin.Controls.Add(txtTenTask);

            // MÔ TẢ
            AddLabel(grpThongTin, "Mô tả:", 20, 80);

            txtMoTa = new TextBox
            {
                Location = new Point(130, 75),
                Size = new Size(300, 65),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 10)
            };

            grpThongTin.Controls.Add(txtMoTa);

            // HẠN NỘP
            AddLabel(grpThongTin, "Hạn nộp:", 480, 80);

            dtpHanNop = new DateTimePicker
            {
                Location = new Point(600, 75),
                Size = new Size(220, 30),
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false
            };

            grpThongTin.Controls.Add(dtpHanNop);

            // ĐỘ ƯU TIÊN
            AddLabel(grpThongTin, "Độ ưu tiên:", 20, 160);

            cboDoUuTien = new ComboBox
            {
                Location = new Point(130, 155),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            cboDoUuTien.Items.AddRange(new object[]
            {
                "Cao",
                "Trung bình",
                "Thấp"
            });

            cboDoUuTien.SelectedIndex = 1;
            grpThongTin.Controls.Add(cboDoUuTien);

            // TRẠNG THÁI
            AddLabel(grpThongTin, "Trạng thái:", 480, 160);

            cboTrangThai = new ComboBox
            {
                Location = new Point(600, 155),
                Size = new Size(220, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            // Giá trị phải khớp với CongViecBUS
            cboTrangThai.Items.AddRange(new object[]
            {
                "To Do",
                "In Progress",
                "Review",
                "Done"
            });

            cboTrangThai.SelectedIndex = 0;
            grpThongTin.Controls.Add(cboTrangThai);

            // CÁC NÚT
            btnThem = CreateButton("Thêm", 20, 220);
            btnSua = CreateButton("Sửa", 145, 220);
            btnXoa = CreateButton("Xóa", 270, 220);
            btnLamMoi = CreateButton("Làm mới", 395, 220);

            grpThongTin.Controls.Add(btnThem);
            grpThongTin.Controls.Add(btnSua);
            grpThongTin.Controls.Add(btnXoa);
            grpThongTin.Controls.Add(btnLamMoi);

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;

            // BẢNG CÔNG VIỆC
            dgvCongViec = new DataGridView
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

            dgvCongViec.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvCongViec.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 41, 59);

            dgvCongViec.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dgvCongViec.EnableHeadersVisualStyles = false;
            dgvCongViec.RowTemplate.Height = 35;

            dgvCongViec.CellClick += DgvCongViec_CellClick;

            Controls.Add(dgvCongViec);

            dgvCongViec.BringToFront();
            grpThongTin.BringToFront();
            lblTitle.BringToFront();
        }

        // TẠO NHÃN
        private void AddLabel(
            Control parent,
            string text,
            int x,
            int y)
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

        // TẢI DANH SÁCH DỰ ÁN
        private void LoadProjects()
        {
            try
            {
                DataTable data = duAnBUS.GetAll();

                cboDuAn.DataSource = null;
                cboDuAn.Items.Clear();

                if (data.Rows.Count == 0)
                {
                    dgvCongViec.DataSource = null;

                    MessageBox.Show(
                        "Chưa có dự án nào. Hãy thêm dự án trước!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                cboDuAn.DisplayMember = "TenDuAn";
                cboDuAn.ValueMember = "MaDuAn";
                cboDuAn.DataSource = data;

                if (cboDuAn.SelectedIndex >= 0)
                    LoadTaskData();
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

        // LẤY MÃ DỰ ÁN ĐANG CHỌN
        private int GetSelectedProjectId()
        {
            object? value = cboDuAn.SelectedValue;

            if (value == null)
                return 0;

            if (value is DataRowView)
                return 0;

            if (int.TryParse(value.ToString(), out int id))
                return id;

            return 0;
        }

        // KHI CHỌN DỰ ÁN KHÁC
        private void CboDuAn_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            int maDuAn = GetSelectedProjectId();

            if (maDuAn > 0 && dgvCongViec != null)
            {
                ClearForm();
                LoadTaskData();
            }
        }

        // TẢI DANH SÁCH CÔNG VIỆC
        private void LoadTaskData()
        {
            int maDuAn = GetSelectedProjectId();

            if (maDuAn <= 0)
                return;

            try
            {
                DataTable data = congViecBUS.GetByProject(maDuAn);

                dgvCongViec.DataSource = data;

                SetColumnHeader("MaTask", "Mã công việc");
                SetColumnHeader("MaDuAn", "Mã dự án");
                SetColumnHeader("TenTask", "Tên công việc");
                SetColumnHeader("MoTa", "Mô tả");
                SetColumnHeader("HanNop", "Hạn nộp");
                SetColumnHeader("DoUuTien", "Độ ưu tiên");
                SetColumnHeader("TrangThai", "Trạng thái");

                if (dgvCongViec.Columns.Contains("HanNop"))
                {
                    dgvCongViec.Columns["HanNop"]!.DefaultCellStyle.Format =
                        "dd/MM/yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách công việc!\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ĐỔI TÊN CỘT
        private void SetColumnHeader(
            string columnName,
            string header)
        {
            if (dgvCongViec.Columns.Contains(columnName))
            {
                dgvCongViec.Columns[columnName]!.HeaderText = header;
            }
        }

        // LẤY DỮ LIỆU TỪ FORM
        private CongViecDTO GetTaskFromForm()
        {
            return new CongViecDTO
            {
                MaTask = maTaskDangChon,
                MaDuAn = GetSelectedProjectId(),
                TenTask = txtTenTask.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),

                HanNop = dtpHanNop.Checked
                    ? dtpHanNop.Value
                    : null,

                DoUuTien =
                    cboDoUuTien.SelectedItem?.ToString()
                    ?? "Trung bình",

                TrangThai =
                    cboTrangThai.SelectedItem?.ToString()
                    ?? "To Do"
            };
        }

        // KIỂM TRA DỮ LIỆU
        private bool ValidateInput()
        {
            if (GetSelectedProjectId() <= 0)
            {
                MessageBox.Show("Vui lòng chọn dự án!");
                cboDuAn.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenTask.Text))
            {
                MessageBox.Show("Vui lòng nhập tên công việc!");
                txtTenTask.Focus();
                return false;
            }

            if (txtTenTask.Text.Trim().Length > 200)
            {
                MessageBox.Show(
                    "Tên công việc không được vượt quá 200 ký tự!");
                txtTenTask.Focus();
                return false;
            }

            return true;
        }

        // THÊM CÔNG VIỆC
        private void BtnThem_Click(object? sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                CongViecDTO task = GetTaskFromForm();

                if (congViecBUS.Insert(task))
                {
                    MessageBox.Show("Thêm công việc thành công!");

                    LoadTaskData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Thêm công việc thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi thêm công việc:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // SỬA CÔNG VIỆC
        private void BtnSua_Click(object? sender, EventArgs e)
        {
            if (maTaskDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn công việc cần sửa!");
                return;
            }

            if (!ValidateInput())
                return;

            try
            {
                CongViecDTO task = GetTaskFromForm();

                if (congViecBUS.Update(task))
                {
                    MessageBox.Show("Cập nhật công việc thành công!");

                    LoadTaskData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Cập nhật công việc thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi cập nhật công việc:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // XÓA CÔNG VIỆC
        private void BtnXoa_Click(object? sender, EventArgs e)
        {
            if (maTaskDangChon <= 0)
            {
                MessageBox.Show("Vui lòng chọn công việc cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa công việc này không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (congViecBUS.Delete(maTaskDangChon))
                {
                    MessageBox.Show("Xóa công việc thành công!");

                    LoadTaskData();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Xóa công việc thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi xóa công việc:\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // CHỌN CÔNG VIỆC TRONG BẢNG
        private void DgvCongViec_CellClick(
            object? sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 ||
                e.RowIndex >= dgvCongViec.Rows.Count)
            {
                return;
            }

            DataGridViewRow row = dgvCongViec.Rows[e.RowIndex];

            if (!dgvCongViec.Columns.Contains("MaTask"))
                return;

            object? ma = row.Cells["MaTask"].Value;

            if (ma == null || ma == DBNull.Value)
                return;

            if (!int.TryParse(ma.ToString(), out int id))
                return;

            maTaskDangChon = id;

            txtTenTask.Text =
                GetCellText(row, "TenTask");

            txtMoTa.Text =
                GetCellText(row, "MoTa");

            SetDatePickerValue(
                dtpHanNop,
                GetCellValue(row, "HanNop"));

            SelectComboItem(
                cboDoUuTien,
                GetCellText(row, "DoUuTien"),
                "Trung bình");

            SelectComboItem(
                cboTrangThai,
                GetCellText(row, "TrangThai"),
                "To Do");
        }

        // LẤY GIÁ TRỊ Ô AN TOÀN
        private object? GetCellValue(
            DataGridViewRow row,
            string columnName)
        {
            if (!dgvCongViec.Columns.Contains(columnName))
                return null;

            return row.Cells[columnName].Value;
        }

        private string GetCellText(
            DataGridViewRow row,
            string columnName)
        {
            return GetCellValue(row, columnName)?.ToString()
                ?? string.Empty;
        }

        // CHỌN GIÁ TRỊ COMBOBOX
        private void SelectComboItem(
            ComboBox combo,
            string value,
            string defaultValue)
        {
            int index = combo.Items.IndexOf(value);

            if (index >= 0)
            {
                combo.SelectedIndex = index;
                return;
            }

            index = combo.Items.IndexOf(defaultValue);

            combo.SelectedIndex = index >= 0 ? index : 0;
        }

        // GÁN NGÀY
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
            LoadProjects();
            LoadTaskData();
        }

        // XÓA DỮ LIỆU TRÊN FORM
        private void ClearForm()
        {
            maTaskDangChon = 0;

            txtTenTask.Clear();
            txtMoTa.Clear();

            dtpHanNop.Checked = false;

            cboDoUuTien.SelectedIndex = 1;
            cboTrangThai.SelectedIndex = 0;

            if (dgvCongViec != null)
                dgvCongViec.ClearSelection();

            txtTenTask.Focus();
        }
    }
}
