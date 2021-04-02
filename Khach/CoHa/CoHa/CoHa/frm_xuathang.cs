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
    public partial class frm_xuathang : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        mt_sudungchung m = new mt_sudungchung();
        DataTable dt_hang = new DataTable();
        public static bool close = false;


        public frm_xuathang()
        {
            InitializeComponent();

            connection.ConnectionString = connectionSTR;


            dataGridViewDonHang.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dataGridViewDonHang.ColumnHeadersHeight = 40;

            comboboxTenHang();

            creat_dt_hang();
        }

        #region thanh tiêu đề
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
        #endregion thanh tiêu đề


        #region Khởi tạo bảng đơn hàng
        private void creat_dt_hang()
        {
            dt_hang.Columns.Add("STT");
            dt_hang.Columns.Add("Mã Hàng");
            dt_hang.Columns.Add("Tên Hàng");
            dt_hang.Columns.Add("SL");
            dt_hang.Columns.Add("Đơn giá");
            dt_hang.Columns.Add("Thành tiền");
            dt_hang.Columns.Add("Ghi chú");
            dt_hang.Columns.Add("SL gửi");
            dt_hang.Columns.Add("SL giao");
        }
        #endregion Khởi tạo bảng đơn hàng



        private void comboboxTenHang()
        {
            connection.Open();
            string query = "select TenHang from HangHoa where Xoa = false order by TenHang asc";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DataRow dr = dt.NewRow();
            dr["TenHang"] = "--";
            dt.Rows.InsertAt(dr, 0);
            connection.Close();

            comboBoxTenHang.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxTenHang.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxTenHang.DataSource = dt;
            comboBoxTenHang.ValueMember = "TenHang";
            comboBoxTenHang.DisplayMember = "TenHang";
        }

        private void comboBoxTenHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTenHang.SelectedIndex > 0)
            {
                connection.Open();
                string query = "select MaHang, SoLuongTon from HangHoa where Xoa = false and TenHang = '" + comboBoxTenHang.Text + "'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                textBoxMaHang.Text = dt.Rows[0].ItemArray[0].ToString();
                if (dt.Rows[0].ItemArray[1].ToString() != "0") textBoxTonKho.Text = m.format_N2T(m.format_T2N(dt.Rows[0].ItemArray[1].ToString()));
                else textBoxTonKho.Text = "0";
                connection.Close();
            }
            else
            {
                textBoxMaHang.Text = textBoxTonKho.Text = null;
            }
        }

        private void textBoxXuat_TextChanged(object sender, EventArgs e)
        {
            if (textBoxXuat.Text != "" && textBoxXuat.Text != null)
            {
                if (textBoxXuat.Text != "0")
                {
                    if (m.format_T2N(textBoxXuat.Text) > m.format_T2N(textBoxTonKho.Text))
                    {
                        textBoxXuat.Text = textBoxDaGiao.Text = null;
                        MessageBox.Show("Số lượng xuất không lớn hơn số lượng tồn kho !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        textBoxDaGiao.Text = textBoxXuat.Text;
                    }
                }
            }
            else textBoxDaGiao.Text = null;

            m.format_money_validating(textBoxXuat);
        }

        private void textBoxXuat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBoxTonKho_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTonKho.Text.Trim() != null && textBoxTonKho.Text.Trim() != "")
            {
                textBoxXuat.ReadOnly = textBoxDonGia.ReadOnly = textBoxSLGui.ReadOnly = false;
            }
            else
            {
                textBoxXuat.ReadOnly = textBoxDonGia.ReadOnly = textBoxSLGui.ReadOnly = true;
                textBoxXuat.Text = textBoxDonGia.Text = textBoxSLGui.Text = textBoxDaGiao.Text = null;
            }
        }

        private void comboBoxTenHang_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxTenHang.SelectedIndex != -1) comboBoxTenHang.SelectedIndex = -1;
            if (comboBoxTenHang.Text != "")
            {
                connection.Open();
                string query = "select MaHang, SoLuongTon from HangHoa where Xoa = false and TenHang = '" + comboBoxTenHang.Text + "'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    textBoxMaHang.Text = dt.Rows[0].ItemArray[0].ToString();
                    textBoxTonKho.Text = m.format_N2T(m.format_T2N(dt.Rows[0].ItemArray[1].ToString()));
                }
                else
                {
                    textBoxMaHang.Text = textBoxTonKho.Text = null;
                }
                connection.Close();
            }else textBoxMaHang.Text = textBoxTonKho.Text = null;
        }

        private void textBoxDonGia_TextChanged(object sender, EventArgs e)
        {
            m.format_money_validating(textBoxDonGia);
        }

        private void textBoxDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBoxSLGui_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSLGui.Text != "" && textBoxSLGui.Text != null)
            {
                if (textBoxSLGui.Text != "0")
                {
                    if (m.format_T2N(textBoxSLGui.Text) > m.format_T2N(textBoxXuat.Text))
                    {
                        textBoxSLGui.Text = null;
                        MessageBox.Show("Số lượng gửi không lớn hơn số lượng xuất !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        int dagiao = m.format_T2N(textBoxXuat.Text) - m.format_T2N(textBoxSLGui.Text);
                        if (dagiao == 0) textBoxDaGiao.Text = "0";
                        else textBoxDaGiao.Text = m.format_N2T(m.format_T2N(textBoxXuat.Text) - m.format_T2N(textBoxSLGui.Text));
                    }
                }
            }
            else textBoxDaGiao.Text = m.format_N2T(m.format_T2N(textBoxXuat.Text) - 0);
            m.format_money_validating(textBoxSLGui);
        }

        private void textBoxSLGui_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }



        private void update_STT()
        {
            for (int i = 0; i < dt_hang.Rows.Count; i++)
            {
                dt_hang.Rows[i][0] = (i + 1).ToString();
            }
        }

        private void buttonThemMatHang_Click(object sender, EventArgs e)
        {
            if (textBoxTonKho.Text != "" && textBoxXuat.Text != "" && textBoxDonGia.Text != "")
            {
                for (int i = 0; i < dt_hang.Rows.Count; i++)
                {
                    if (textBoxMaHang.Text == dt_hang.Rows[i].ItemArray[1].ToString() && comboBoxTenHang.Text == dt_hang.Rows[i].ItemArray[2].ToString())
                    {
                        MessageBox.Show("Mặt hàng này đã có trong phiếu xuất (STT " + (i+1).ToString() + ")", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                dt_hang.Rows.Add("", textBoxMaHang.Text, comboBoxTenHang.Text, textBoxXuat.Text, textBoxDonGia.Text, (m.format_T2N(textBoxDonGia.Text) * m.format_T2N(textBoxXuat.Text)), textBoxGhiChu.Text, textBoxSLGui.Text, textBoxDaGiao.Text);
                update_STT();

                dataGridViewDonHang.Refresh();
                dataGridViewDonHang.DataSource = dt_hang;

                comboBoxTenHang.SelectedIndex = 0;
                textBoxGhiChu.Text = "";
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin xuất hàng !", "Thông báo", MessageBoxButtons.OK , MessageBoxIcon.Information);
            }
        }


        private void dataGridViewDonHang_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            update_STT();
        }

        private void dataGridViewDonHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewDonHang.CurrentCell.RowIndex;

            if (row >= 0)
            {
                dt_hang.Rows.RemoveAt(row);
                update_STT();
                dataGridViewDonHang.Refresh();
                dataGridViewDonHang.DataSource = dt_hang;
            }
        }

        private void labelTaoPhieu_Click(object sender, EventArgs e)
        {
            if (dt_hang.Rows.Count > 0)
            {
                DataTable dv = dt_hang.Copy();
                frm_xacnhanphieu f = new frm_xacnhanphieu(dv, null);
                this.Hide();
                f.ShowDialog();
                if (close == true)
                {
                    this.Close();
                }
                this.Show();
            }
            else
            {
                MessageBox.Show("Phiếu xuất hàng chưa có sản phẩm !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void labelTaoPhieu_MouseHover(object sender, EventArgs e)
        {
            labelTaoPhieu.Font = new Font(labelTaoPhieu.Font.FontFamily, labelTaoPhieu.Font.Size, FontStyle.Bold | FontStyle.Underline);
            this.Cursor = Cursors.Hand;
        }

        private void labelTaoPhieu_MouseLeave(object sender, EventArgs e)
        {
            labelTaoPhieu.Font = new Font(labelTaoPhieu.Font.FontFamily, labelTaoPhieu.Font.Size, FontStyle.Regular);
            this.Cursor = Cursors.Default;
        }
    }
}
