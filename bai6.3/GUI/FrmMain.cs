
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectTaskManagement.GUI
{
    public class FrmMain : Form
    {
        private Panel pnlMenu = null!;
        private Panel pnlHeader = null!;
        private Panel pnlContent = null!;
        private Label lblTitle = null!;

        public FrmMain()
        {
            InitializeMainForm();
        }

        private void InitializeMainForm()
        {
            Text = "HỆ THỐNG QUẢN LÝ CÔNG VIỆC";
            Size = new Size(1200, 750);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(245, 247, 250);
            MinimumSize = new Size(1000, 650);

            // MENU BÊN TRÁI
            pnlMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 230,
                BackColor = Color.FromArgb(30, 41, 59)
            };
            Controls.Add(pnlMenu);

            Label lblLogo = new Label
            {
                Text = "PROJECT MANAGER",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 90,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlMenu.Controls.Add(lblLogo);

            AddMenuButton("Tổng quan", 110, ShowDashboard);
            AddMenuButton("Quản lý dự án", 170,
       () => new FrmDuAn().ShowDialog(this));
            AddMenuButton("Quản lý công việc", 230,
     () => new FrmCongViec().ShowDialog(this));
            AddMenuButton("Quản lý thành viên", 290,
     () => new FrmThanhVien().ShowDialog(this));
            AddMenuButton("Phân công công việc", 350,
     () => new FrmPhanCong().ShowDialog(this));
            AddMenuButton("Báo cáo tiến độ", 410,
      () => new FrmBaoCao().ShowDialog(this));

            // HEADER
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 75,
                BackColor = Color.White
            };
            Controls.Add(pnlHeader);

            lblTitle = new Label
            {
                Text = "TỔNG QUAN HỆ THỐNG",
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(30, 22)
            };
            pnlHeader.Controls.Add(lblTitle);

            // KHU VỰC NỘI DUNG
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(25),
                BackColor = Color.FromArgb(245, 247, 250)
            };
            Controls.Add(pnlContent);

            pnlHeader.BringToFront();
            pnlMenu.BringToFront();

            ShowDashboard();
        }

        // TẠO NÚT MENU
        private void AddMenuButton(string text, int top, Action action)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(10, top),
                Size = new Size(210, 48),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(51, 65, 85);

            btn.Click += (sender, e) => action();

            pnlMenu.Controls.Add(btn);
        }

        // MÀN HÌNH TỔNG QUAN
        private void ShowDashboard()
        {
            lblTitle.Text = "TỔNG QUAN HỆ THỐNG";
            pnlContent.Controls.Clear();

            Label lblWelcome = new Label
            {
                Text = "Chào mừng đến với hệ thống quản lý dự án!",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                AutoSize = true,
                Location = new Point(25, 25)
            };

            pnlContent.Controls.Add(lblWelcome);

            AddDashboardCard(
                "QUẢN LÝ DỰ ÁN",
                "Theo dõi thông tin và trạng thái các dự án",
                25, 100);

            AddDashboardCard(
                "QUẢN LÝ CÔNG VIỆC",
                "Tạo, cập nhật và theo dõi công việc",
                390, 100);

            AddDashboardCard(
                "QUẢN LÝ THÀNH VIÊN",
                "Quản lý thông tin thành viên dự án",
                25, 270);

            AddDashboardCard(
                "BÁO CÁO TIẾN ĐỘ",
                "Theo dõi tiến độ và công việc quá hạn",
                390, 270);
        }

        // TẠO THẺ THÔNG TIN
        private void AddDashboardCard(
            string title,
            string description,
            int x,
            int y)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(330, 135),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblCardTitle = new Label
            {
                Text = title,
                Location = new Point(18, 18),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(37, 99, 235)
            };

            Label lblDescription = new Label
            {
                Text = description,
                Location = new Point(18, 55),
                Size = new Size(290, 55),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(71, 85, 105)
            };

            card.Controls.Add(lblCardTitle);
            card.Controls.Add(lblDescription);

            pnlContent.Controls.Add(card);
        }

        // MÀN HÌNH TẠM THỜI
        private void ShowComingSoon(string screenName)
        {
            lblTitle.Text = screenName.ToUpper();
            pnlContent.Controls.Clear();

            Label lbl = new Label
            {
                Text = screenName +
                       "\n\nGiao diện này sẽ được xây dựng ở bước tiếp theo.",
                Font = new Font("Segoe UI", 15, FontStyle.Regular),
                ForeColor = Color.FromArgb(51, 65, 85),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            pnlContent.Controls.Add(lbl);
        }
    }
}
