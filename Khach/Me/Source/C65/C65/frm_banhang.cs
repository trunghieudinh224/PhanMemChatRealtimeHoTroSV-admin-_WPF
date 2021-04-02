using C65.method;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C65
{
    public partial class frm_banhang : Form
    {
        mt_sudungchung m = new mt_sudungchung();
        List<SPKhachChon> SPList_ToaMua = new List<SPKhachChon>();
        List<SPKhachChon> SPList_ToaTra = new List<SPKhachChon>();
        public static bool reset = false;

        public frm_banhang()
        {
            InitializeComponent();
            checkBoxMua.Checked = true;
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

        private void textBoxDonGia_TextChanged(object sender, EventArgs e)
        {
            format_money_validating(textBoxDonGia);
        }

        private void textBoxDonGia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Them_SP();
                textBoxSL.Focus();
            }
        }

        private void textBoxSL_TextChanged(object sender, EventArgs e)
        {
            format_money_validating(textBoxSL);
        }

        private void textBoxSL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxDonGia.Focus();
            }
        }

        private void textBoxGiamGia_TextChanged(object sender, EventArgs e)
        {
            format_money_validating(textBoxGiamGia);

            TongTien();
            TienThua();
        }

        private void textBoxNoToaCu_TextChanged(object sender, EventArgs e)
        {
            format_money_validating(textBoxNoToaCu);

            TongTien();
            TienThua();
        }

        private void textBoxTienKhachTra_TextChanged(object sender, EventArgs e)
        {
            //labelTienThua_Text.Visible = labelTienThua.Visible = labelTienThua_VND.Visible = true;

            format_money_validating(textBoxTienKhachTra);
            TienThua();
        }

        private void buttonThemSP_Click(object sender, EventArgs e)
        {
            Them_SP();
        }

        private void Them_SP()
        {
            if (m.format_T2N(textBoxDonGia.Text.Trim()) > 0 && m.format_T2N(textBoxSL.Text.Trim()) > 0)
            {
                int dongia = m.format_T2N(textBoxDonGia.Text.Trim()) * 1000;
                int sl = m.format_T2N(textBoxSL.Text.Trim());
                int thanhtien = (dongia * sl);

                if (checkBoxMua.Checked == true)
                {
                    string[] sp = { "", m.format_N2T(sl), m.format_N2T(dongia), m.format_N2T(thanhtien) };
                    var x = new ListViewItem(sp);
                    listViewToaMua.Items.Add(x);
                    getToa(listViewToaMua, SPList_ToaMua);
                }
                else
                {
                    string[] sp = { "", m.format_N2T(sl), m.format_N2T(dongia), m.format_N2T(thanhtien) };
                    var x = new ListViewItem(sp);
                    listViewToaTra.Items.Add(x);
                    getToa(listViewToaTra, SPList_ToaTra);
                }

                textBoxDonGia.Text = textBoxSL.Text = "";
                textBoxSL.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đơn giá và số lượng !!!", "Thông báo");
            }
        }

        private void checkBoxMua_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMua.Checked == true)
            {
                checkBoxTra.Checked = false;
            }
            else
            {
                checkBoxTra.Checked = true;
            }
        }

        private void checkBoxTra_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTra.Checked == true)
            {
                checkBoxMua.Checked = false;
            }
            else
            {
                checkBoxMua.Checked = true;
            }
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

        private void textBoxSL_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxGiamGia_KeyPress(object sender, KeyPressEventArgs e)
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
               
        private void textBoxNoToaCu_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxTienKhachTra_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxSDT_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxSDT_KeyDown(object sender, KeyEventArgs e)
        {
            focus(e, textBoxTen);
        }
        private void textBoxTen_KeyDown(object sender, KeyEventArgs e)
        {
            focus(e, textBoxDiaChi);
        }

        private void textBoxDiaChi_KeyDown(object sender, KeyEventArgs e)
        {
            focus(e, textBoxGhiChu);
        }

        private void textBoxGhiChu_KeyDown(object sender, KeyEventArgs e)
        {
            focus(e, textBoxGiamGia);
        }

        private void textBoxGiamGia_KeyDown(object sender, KeyEventArgs e)
        {
            focus(e, textBoxNoToaCu);
        }

        private void textBoxNoToaCu_KeyDown(object sender, KeyEventArgs e)
        {
            focus(e, textBoxTienKhachTra);
        }

        private void textBoxTienKhachTra_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TaoToa();
            }
        }

        void focus(KeyEventArgs e, TextBox txb)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txb.Focus();
            }
        }

        private void listViewToaMua_DoubleClick(object sender, EventArgs e)
        {
            listViewToaMua.SelectedItems[0].Remove();
            getToa(listViewToaMua, SPList_ToaMua);
        }

        private void listViewToaTra_DoubleClick(object sender, EventArgs e)
        {
            listViewToaTra.SelectedItems[0].Remove();
            getToa(listViewToaTra, SPList_ToaTra);
        }

        private void getToa(ListView listview, List<SPKhachChon> list)
        {
            list.Clear();
            for (int i = 0; i < listview.Items.Count; i++)
            {
                list.Add(new SPKhachChon(m.format_T2N(listview.Items[i].SubItems[1].Text), m.format_T2N(listview.Items[i].SubItems[2].Text), m.format_T2N(listview.Items[i].SubItems[3].Text)));
            }
            labelTongSL.Text = m.format_N2T(Tong_SL_Mua_Tra(listViewToaMua)) + " - " + m.format_N2T(Tong_SL_Mua_Tra(listViewToaTra));
            labelMuaTra.Text = m.format_N2T(Tong_Tien_Mua_Tra(listViewToaMua)) + " - " + m.format_N2T(Tong_Tien_Mua_Tra(listViewToaTra));
            TongTien();
            TienThua();
        }

        private void format_money_validating(TextBox textbox)
        {
            if (textbox.Text != "")
            {
                textbox.Text = m.format_N2T(Convert.ToInt32(m.format_T2N(textbox.Text.Trim())));
                textbox.SelectionStart = textbox.Text.Length;
                textbox.SelectionLength = 0;
            }
        }

        private int Tong_SL_Mua_Tra(ListView listview)
        {
            int value = 0;
            for (int i = 0; i < listview.Items.Count; i++)
            {
                value = value + m.format_T2N(listview.Items[i].SubItems[1].Text);
            }
            return value;
        }

        private int Tong_Tien_Mua_Tra(ListView listview)
        {
            int value = 0;
            for (int i = 0; i < listview.Items.Count; i++)
            {
                value = value + m.format_T2N(listview.Items[i].SubItems[3].Text);
            }
            return value;
        }

        private void TongTien()
        {
            labelTongTien.Text = m.format_N2T((Tong_Tien_Mua_Tra(listViewToaMua) + m.format_T2N(textBoxNoToaCu.Text.Trim())) - Tong_Tien_Mua_Tra(listViewToaTra) - m.format_T2N(textBoxGiamGia.Text.Trim() ));
        }

        private void TienThua()
        {
            if (labelTienMatKhachTra.Text == "")
            {
                labelTienMatKhachTra.Text = "0";
            }
            int tienthua = m.format_T2N(textBoxTienKhachTra.Text) - m.format_T2N(labelTongTien.Text);
            if (tienthua >= 0)
            {
                labelTienThua_Text.Text = "Tiền thừa:";
            }
            else
            {
                labelTienThua_Text.Text = "Khách nợ:";
            }

            labelTienThua.Text = m.format_N2T(tienthua);
        }

        private void textBoxGiamGia_MouseMove(object sender, MouseEventArgs e)
        {
            textBoxGiamGia.SelectionLength = 0;
        }

        private void buttonTaoToa_Click(object sender, EventArgs e)
        {
            TaoToa();
        }

        private void TaoToa()
        {
            if (listViewToaMua.Items.Count > 0 || listViewToaTra.Items.Count > 0)
            {
                List<Object> list_mua = new List<object>(SPList_ToaMua);
                List<Object> list_tra = new List<object>(SPList_ToaTra);
                List<Object> list_khachhang = new List<object> { textBoxSDT.Text, textBoxTen.Text, textBoxDiaChi.Text, textBoxGhiChu.Text };
                List<Object> list_thongtintoa = new List<object> { dateTimePickerNgay.Value.ToString("dd/MM/yyyy"), m.format_N2T(Tong_SL_Mua_Tra(listViewToaMua)), m.format_N2T(Tong_Tien_Mua_Tra(listViewToaMua)), m.format_N2T(Tong_SL_Mua_Tra(listViewToaTra)), m.format_N2T(Tong_Tien_Mua_Tra(listViewToaTra)), m.format_N2T(m.format_T2N(textBoxGiamGia.Text)), m.format_N2T(m.format_T2N(labelTongTien.Text)), m.format_N2T(m.format_T2N(textBoxTienKhachTra.Text)), m.format_N2T(m.format_T2N(labelTienThua.Text)), m.format_N2T(m.format_T2N(textBoxNoToaCu.Text)) };
                frm_chitiettoa frm = new frm_chitiettoa(list_mua, list_tra, list_khachhang, list_thongtintoa);
                frm.ShowDialog();

                if (reset == true)
                {
                    Reset();
                    reset = false;
                }
            }
            else
            {
                MessageBox.Show("Toa hàng phải có ít nhất 1 sản phẩm !!!", "Thông báo");
            }
        }

        private void Reset()
        {
            SPList_ToaMua.Clear();
            SPList_ToaTra.Clear();
            listViewToaMua.Items.Clear();
            listViewToaTra.Items.Clear();
            textBoxSDT.Text = textBoxTen.Text = textBoxDiaChi.Text = textBoxGhiChu.Text = textBoxGiamGia.Text = textBoxNoToaCu.Text = textBoxTienKhachTra.Text = "";
            labelTongSL.Text = labelMuaTra.Text = "0 - 0";
            labelTongTien.Text = labelTienThua.Text = "";
        }
    }
}
