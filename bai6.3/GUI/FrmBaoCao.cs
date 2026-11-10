
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ProjectTaskManagement.BUS;

namespace ProjectTaskManagement.GUI
{
    public class FrmBaoCao : Form
    {
        private readonly DuAnBUS duAnBUS = new DuAnBUS();
        private readonly CongViecBUS congViecBUS = new CongViecBUS();

        private Label lblTongDuAn = null!;
        private Label lblTongCongViec = null!;
        private Label lblHoanThanh = null!;

        private DataGridView dgvQuaHan = null!;
        private DataGridView dgvTienDo = null!;

        private ComboBox cboDuAn = null!;
        private Button btnLamMoi = null!;

        public FrmBaoCao()
        {
            InitializeComponent();
            LoadDuAn();
            LoadBaoCao();
        }

        private void InitializeComponent()
        {
            Text = "BÁO CÁO TIẾN ĐỘ DỰ ÁN";
            Size = new Size(1150, 750);
            MinimumSize = new Size(950, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label
            {
                Text = "BÁO CÁO TIẾN ĐỘ DỰ ÁN",
                Location = new Point(25, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59)
            };
            Controls.Add(lblTitle);

            btnLamMoi = new Button
            {
                Text = "Làm mới dữ liệu",
                Location = new Point(920, 18),
                Size = new Size(180, 40),
                BackColor = Color.FromArgb(37, 99, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            btnLamMoi.Click += BtnLamMoi_Click;
            Controls.Add(btnLamMoi);

            Panel pnlDuAn = CreateCard(
                "TỔNG SỐ DỰ ÁN",
                Color.FromArgb(37, 99, 235),
                25, 85);

            Panel pnlCongViec = CreateCard(
                "TỔNG CÔNG VIỆC",
                Color.FromArgb(8, 145, 178),
                390, 85);

            Panel pnlHoanThanh = CreateCard(
                "ĐÃ HOÀN THÀNH",
                Color.FromArgb(22, 163, 74),
                755, 85);

            lblTongDuAn = CreateCardValue(pnlDuAn);
            lblTongCongViec = CreateCardValue(pnlCongViec);
            lblHoanThanh = CreateCardValue(pnlHoanThanh);

            // Chọn dự án để xem tiến độ
            Label lblChonDuAn = new Label
            {
                Text = "Chọn dự án:",
                Location = new Point(25, 235),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            Controls.Add(lblChonDuAn);

            cboDuAn = new ComboBox
            {
                Location = new Point(130, 230),
                Size = new Size(400, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };

            cboDuAn.SelectedIndexChanged += CboDuAn_SelectedIndexChanged;
            Controls.Add(cboDuAn);

            // Tab báo cáo
            TabControl tabBaoCao = new TabControl
            {
                Location = new Point(25, 280),
                Size = new Size(1075, 410),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 10)
            };

            TabPage tabQuaHan = new TabPage("Công việc quá hạn");
            TabPage tabTienDo = new TabPage("Tiến độ thành viên");

            dgvQuaHan = CreateGrid();
            dgvQuaHan.Dock = DockStyle.Fill;

            dgvTienDo = CreateGrid();
            dgvTienDo.Dock = DockStyle.Fill;

            tabQuaHan.Controls.Add(dgvQuaHan);
            tabTienDo.Controls.Add(dgvTienDo);

            tabBaoCao.TabPages.Add(tabQuaHan);
            tabBaoCao.TabPages.Add(tabTienDo);

            Controls.Add(tabBaoCao);
        }

        private Panel CreateCard(string title, Color color, int x, int y)
        {
            Panel pnl = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(330, 125),
                BackColor = color,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            Label lbl = new Label
            {
                Text = title,
                Location = new Point(20, 18),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            pnl.Controls.Add(lbl);
            Controls.Add(pnl);

            return pnl;
        }

        private Label CreateCardValue(Panel pnl)
        {
            Label lbl = new Label
            {
                Text = "0",
                Location = new Point(20, 55),
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 30, FontStyle.Bold)
            };

            pnl.Controls.Add(lbl);
            return lbl;
        }

        private DataGridView CreateGrid()
        {
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
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

            dgv.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 41, 59);

            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 35;

            return dgv;
        }

        // Tải danh sách dự án
        private void LoadDuAn()
        {
            try
            {
                DataTable danhSach = duAnBUS.GetAll();

                cboDuAn.DataSource = danhSach;
                cboDuAn.DisplayMember = "TenDuAn";
                cboDuAn.ValueMember = "MaDuAn";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải danh sách dự án.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Lấy mã dự án đang chọn
        private int GetSelectedProjectId()
        {
            if (cboDuAn.SelectedValue == null ||
                cboDuAn.SelectedValue is DataRowView)
            {
                return 0;
            }

            return Convert.ToInt32(cboDuAn.SelectedValue);
        }

        // Khi chọn dự án khác
        private void CboDuAn_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int maDuAn = GetSelectedProjectId();

            if (maDuAn > 0 && dgvTienDo != null)
            {
                LoadTienDoThanhVien(maDuAn);
            }
        }

        // Tải toàn bộ báo cáo
        private void LoadBaoCao()
        {
            try
            {
                DataTable danhSachDuAn = duAnBUS.GetAll();
                DataTable danhSachCongViec = congViecBUS.GetAll();

                lblTongDuAn.Text = danhSachDuAn.Rows.Count.ToString();
                lblTongCongViec.Text = danhSachCongViec.Rows.Count.ToString();
                lblHoanThanh.Text = congViecBUS.CountCompleted().ToString();

                // Công việc quá hạn
                dgvQuaHan.DataSource = congViecBUS.GetOverdueTasks();

                // Tiến độ theo dự án đang chọn
                int maDuAn = GetSelectedProjectId();

                if (maDuAn > 0)
                {
                    LoadTienDoThanhVien(maDuAn);
                }
                else
                {
                    dgvTienDo.DataSource = null;
                }

                FormatGrid(dgvQuaHan);
                FormatGrid(dgvTienDo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải báo cáo.\nChi tiết: " + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Tải tiến độ thành viên theo mã dự án
        private void LoadTienDoThanhVien(int maDuAn)
        {
            try
            {
                DataTable danhSachTienDo =
                    congViecBUS.GetMemberProgress(maDuAn);

                dgvTienDo.DataSource = danhSachTienDo;

                FormatGrid(dgvTienDo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể tải tiến độ thành viên.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // Định dạng tiêu đề cột
        private void FormatGrid(DataGridView dgv)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.HeaderText = column.Name;
            }

            dgv.ClearSelection();
        }

        // Làm mới báo cáo
        private void BtnLamMoi_Click(object? sender, EventArgs e)
        {
            LoadDuAn();
            LoadBaoCao();

            MessageBox.Show(
                "Đã cập nhật dữ liệu báo cáo!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}
