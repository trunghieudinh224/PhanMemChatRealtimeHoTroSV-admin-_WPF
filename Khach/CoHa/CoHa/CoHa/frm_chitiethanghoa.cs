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
using VanSon.Method;

namespace VanSon
{
    public partial class frm_chitiethanghoa : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        mt_sudungchung m = new mt_sudungchung();
        string mahang = "", tenhang = "";

        public frm_chitiethanghoa(string MaHang, string TenHang)
        {
            InitializeComponent();

            connection.ConnectionString = connectionSTR;

            textBoxMaHang.Text = mahang = MaHang;
            textBoxTenHang.Text = tenhang = TenHang;
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

        private void buttonCapNhat_Click(object sender, EventArgs e)
        {
            if (textBoxMaHang.Text.Trim() != "" && textBoxTenHang.Text.Trim() != "")
            {
                if (textBoxMaHang.Text.Trim() != mahang && textBoxTenHang.Text.Trim() != tenhang)
                {
                    try
                    {
                        connection.Open();
                        string query = "update HangHoa set MaHang = '" + textBoxMaHang.Text.Trim() + "', TenHang = '" + textBoxTenHang.Text.Trim() + "' where MaHang = '" + mahang + "' and TenHang = '" + tenhang + "'" + " and  Xoa = false";
                        OleDbCommand cmd = new OleDbCommand(query, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();


                        m.update_MaHang_TenHang(mahang, tenhang, textBoxMaHang.Text.Trim(), textBoxTenHang.Text.Trim());
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi kết nối, cập nhật thất bại", "Thông báo");
                    }
                    
                }
                
                if (textBoxMaHang.Text.Trim() != mahang)
                {
                    connection.Open();
                    string query_check = "select count(MaHang) from HangHoa where Xoa = false and MaHang = '" + textBoxMaHang.Text.Trim() + "'";
                    OleDbCommand cmd_check = new OleDbCommand(query_check, connection);
                    int check_value = Convert.ToInt32(cmd_check.ExecuteScalar());
                    connection.Close();
                    if (check_value == 0)
                    {
                        try
                        {
                            connection.Open();
                            string query = "update HangHoa set MaHang = '" + textBoxMaHang.Text.Trim() + "' where MaHang = '" + mahang + "' and Xoa = false";
                            OleDbCommand cmd = new OleDbCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                            m.update_MaHang_TenHang(mahang, tenhang, textBoxMaHang.Text.Trim(), textBoxTenHang.Text.Trim());
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Lỗi kết nối, cập nhật thất bại", "Thông báo");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã hàng hoặc tên hàng đã tồn tại", "Thông báo");
                    }

                }

                if (textBoxTenHang.Text.Trim() != tenhang)
                {
                    connection.Open();
                    string query_check = "select count(TenHang) from HangHoa where Xoa = false and MaHang = '" + textBoxTenHang.Text.Trim() + "'";
                    OleDbCommand cmd_check = new OleDbCommand(query_check, connection);
                    int check_value = Convert.ToInt32(cmd_check.ExecuteScalar());
                    connection.Close();
                    if (check_value == 0)
                    {
                        try
                        {
                            connection.Open();
                            string query = "update TenHang set TenHang = '" + textBoxMaHang.Text.Trim() + "' where MaHang = '" + tenhang + "' and Xoa = false";
                            OleDbCommand cmd = new OleDbCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                            m.update_MaHang_TenHang(mahang, tenhang, textBoxMaHang.Text.Trim(), textBoxTenHang.Text.Trim());
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Lỗi kết nối, cập nhật thất bại", "Thông báo");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã hàng hoặc tên hàng đã tồn tại", "Thông báo");
                    }

                }
            }
        }
    }
}
