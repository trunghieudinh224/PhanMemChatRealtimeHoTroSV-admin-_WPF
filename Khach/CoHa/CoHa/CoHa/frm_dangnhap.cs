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

namespace CoHa
{
    public partial class frm_dangnhap : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        public static string user = String.Empty;
        public frm_dangnhap()
        {
            connection.ConnectionString = connectionSTR;

            InitializeComponent();
        }

        private void buttonAn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //HÀM KHI RÊ CHUỘT VÀO NÚT ẨN
        private void buttonAn_MouseHover(object sender, EventArgs e)
        {
            buttonAn.BackColor = Color.LightCoral;
        }

        //HÀM KHI RÊ CHUỘT RỜI KHỎI NÚT ẨN
        private void buttonAn_MouseLeave(object sender, EventArgs e)
        {
            buttonAn.BackColor = Color.Firebrick;
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
                connection.Open();
                string query = "select Username, Role from TaiKhoan where Username = '" + textBoxTaiKhoan.Text.Trim() + "' and Password = '" + textBoxMatKhau.Text.Trim() + "' and TrangThai = 'active'";
                da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray[0].ToString() != "" && dt.Rows[0].ItemArray[0].ToString() != null)
                    {
                        user = textBoxTaiKhoan.Text;
                        frm_quanlyxuatnhap f = new frm_quanlyxuatnhap(Convert.ToInt32(dt.Rows[0].ItemArray[1].ToString()));
                        textBoxTaiKhoan.Text = "";
                        textBoxMatKhau.Text = "";
                        this.Hide();
                        f.ShowDialog();
                        this.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu chưa đúng !!!", "Thông báo");
                }
                connection.Close();
            }
            else
            {
                MessageBox.Show("Bạn chưa nhập tài khoản hoặc mật khẩu !!!", "Thông báo");
            }



            //string filePath1 = Application.StartupPath + @"\aa.txt";

            //string[] lines1;
            //string str1;
            //DataTable dt1 = new DataTable();
            //dt1.Columns.Add("Mahang");
            //dt1.Columns.Add("Tenhang");
            //if (System.IO.File.Exists(filePath1))
            //{
            //    lines1 = System.IO.File.ReadAllLines(filePath1);

            //    for (int i = 0; i < lines1.Length; i++)
            //    {
            //        string x = lines1[i];
            //        string[] word = x.Split('\\');

            //        object[] obj = new object[2];
            //        obj[0] = word[1];
            //        obj[1] = word[0];
            //        obj[2] = word[2];
            //        dt1.Rows.Add(obj);

            //        connection.Open();
            //        string query = "insert into HangHoa (MaHang,TenHang,SoLuongTon) values ('" + word[1] + "','" + word[0] + "', " + Convert.ToInt32(word[2]) + ")";
            //        OleDbCommand cmd = new OleDbCommand(query, connection);
            //        cmd.ExecuteNonQuery();
            //        connection.Close();

            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Không tìm thấy file danh mục", "Thông báo");
            //}


        }
    }
}
