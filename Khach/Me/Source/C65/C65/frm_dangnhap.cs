using C65.method;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C65
{
    public partial class frm_dangnhap : Form
    {
        mt_connection connection = new mt_connection();

        public frm_dangnhap()
        {
            //connection.conn.ConnectionString = connection.connectionSTR;
            connection.connection_project();
            InitializeComponent();

        }

        //HÀM KHI CLICK NÚT X
        private void buttonX_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //HÀM KHI RÊ CHUỘT VÀO NÚT X
        private void buttonX_MouseHover(object sender, EventArgs e)
        {
            buttonX.BackColor = Color.LightCoral;
        }

        //HÀM KHI RÊ CHUỘT RỜI KHỎI NÚT X
        private void buttonX_MouseLeave(object sender, EventArgs e)
        {
            buttonX.BackColor = Color.Firebrick;
        }

        int mov;
        int movX;
        int movY;
        //HÀM DI CHUYỂN FORM
        private void panelThanhTieuDe_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        //HÀM DI CHUYỂN FORM
        private void panelThanhTieuDe_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        //HÀM DI CHUYỂN FORM
        private void panelThanhTieuDe_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void buttonDangNhap_Click(object sender, EventArgs e)
        {
            if (textBoxTaiKhoan.Text.Trim() != "" && textBoxMatKhau.Text.Trim() != "")
            {
                try
                {
                    if (connection.login(textBoxTaiKhoan.Text.Trim(), textBoxMatKhau.Text.Trim(), "active") > 0)
                    {
                        frm_banhang f = new frm_banhang();
                        textBoxTaiKhoan.Text = "";
                        textBoxMatKhau.Text = "";
                        this.Hide();
                        f.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản hoặc mật khẩu chưa đúng !!!", "Thông báo");
                    }
                }
                catch
                {
                    MessageBox.Show("Lỗi kết nối !!!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập tài khoản hoặc mật khẩu !!!", "Thông báo");
            }
        }
    }
}
