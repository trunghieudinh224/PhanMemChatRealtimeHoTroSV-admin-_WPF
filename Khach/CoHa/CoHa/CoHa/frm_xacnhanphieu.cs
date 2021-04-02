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
    public partial class frm_xacnhanphieu : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        mt_sudungchung m = new mt_sudungchung();
        mt_export_pdf_excel_file m_report = new mt_export_pdf_excel_file();
        DataTable dt_hang = new DataTable();
        DataTable dt_hang_gui = new DataTable();
        DataTable dt_before_change = new DataTable();
        string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        string tooltip_message = "";
        string[] arr_info = new string[8];

        public frm_xacnhanphieu(DataTable dt_notexist, DataTable dt_exist)
        {
            InitializeComponent();

            connection.ConnectionString = connectionSTR;

            if (dt_notexist != null)
            {
                dt_hang = dt_notexist.Copy();

                dataGridViewDonHang.DataSource = dt_hang;

                HienThi_Tooltip_Button();

                show_ThanhToan(dt_notexist);

                get_HangGui(dt_notexist);
            }
            else
            {
                show_TT(dt_exist);
            }
        }


        #region Tooltip
        private void HienThi_Tooltip_Button()
        {
            settooltip_button(buttonInLienKhachHang, "Liên cho khách hàng");
            settooltip_button(buttonInLienThuKho, "Liên cho thủ kho");
        }

        private void settooltip_button(Button btn, string mess)
        {
            tooltip_message = mess;
            System.Windows.Forms.ToolTip ToolTip = new System.Windows.Forms.ToolTip();
            ToolTip.SetToolTip(btn, tooltip_message);
            ToolTip.OwnerDraw = true;
            ToolTip.Draw += new DrawToolTipEventHandler(ToolTip_Draw);
            ToolTip.Popup += new PopupEventHandler(ToolTip_Popup);
        }

        void ToolTip_Popup(object sender, PopupEventArgs e)
        {
            System.Drawing.Font f = new System.Drawing.Font("Arial", 13.0f, FontStyle.Bold);
            e.ToolTipSize = TextRenderer.MeasureText(tooltip_message, f);
        }

        void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            System.Drawing.Font f = new System.Drawing.Font("Arial", 13.0f, FontStyle.Bold);
            e.DrawBackground();
            e.DrawBorder();
            tooltip_message = e.ToolTipText;
            e.Graphics.DrawString(e.ToolTipText, f, Brushes.Black, new PointF(9, 0));
        }
        #endregion Tooltip



        #region thanh tiêu đề
     
        //HÀM KHI CLICK NÚT X
        private void buttonX_Click(object sender, EventArgs e)
        {
            frm_xuathang.close = true;
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



        #region Cập nhật XNT
        private void check_XNT_database()
        {
            string thang = DateTime.Now.ToString("MM/yyyy");

            connection.Open();
            string querycheck_exist = "select CapNhatTonDau from XuatNhapTon where (Month(ThoiGian) = " + thang.Substring(0, 2) + " and Year(ThoiGian) = " + thang.Substring(3, 4) + ")";
            da = new OleDbDataAdapter(querycheck_exist, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            connection.Close();

            if (dt.Rows.Count == 0)
            {
                create_XNT(thang);
                create_update_tondau_XNT(thang, create_XNT_default(), false);       //tạo bảng xuất nhập tồn có tồn đầu
            }
            else
            {
                if (Convert.ToBoolean(dt.Rows[0].ItemArray[0].ToString()) == false)
                {
                    create_update_tondau_XNT(thang, create_XNT_default(), true);
                }
            }

        }

        private void create_XNT(string thang)
        {
            string tenfile = "XUẤT NHẬP TỒN THÁNG " + thang;

            connection.Open();
            string query = "insert into XuatNhapTon (TenFile, ThoiGian, NgayTao) values ('" + tenfile + "', '" + thang + "', '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "')";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private DataTable create_XNT_default()
        {
            connection.Open();
            string query = "select MaHang, TenHang, SoLuongTon from HangHoa where Xoa = false order by TenHang asc";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            connection.Close();
            return dt;
        }

        private void create_update_tondau_XNT(string thang, DataTable dt, bool update)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                connection.Open();
                string query = "";
                if (update == false)
                {
                    query = "insert into ChiTietXuatNhapTon (ThoiGian, MaHang, TenHang, TonDK) values ('" + thang + "', '" + dt.Rows[i].ItemArray[0].ToString() + "', '" + dt.Rows[i].ItemArray[1].ToString() + "', '" + dt.Rows[i].ItemArray[2].ToString() + "')";
                }
                else query = "update ChiTietXuatNhapTon set TonDK = " + Convert.ToInt32(dt.Rows[i].ItemArray[2].ToString()) + " where MaHang = '" + dt.Rows[i].ItemArray[0].ToString() + "' and TenHang = '" + dt.Rows[i].ItemArray[1].ToString() + "'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();

                string query_update = "update XuatNhapTon set CapNhatTonDau = true, ThoiGianCapNhatTonDau = '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "' where Month(ThoiGian) = " + thang.Substring(0, 2) + " and Year(ThoiGian) = " + thang.Substring(3, 4) + "";
                OleDbCommand cmd_update = new OleDbCommand(query_update, connection);
                cmd_update.ExecuteNonQuery();
                connection.Close();
            }
        }

        private void update_cotnhap_XNT(string thang, string cot, string mahang, string tenhang)
        {
            //lấy tổng hàng nhập của tháng của tên hàng đó
            connection.Open();
            string query = "select sum(SoLuong" + cot + ") from Hang" + cot + " where Xoa = false and MaHang = '" + mahang + "' and TenHang = '" + tenhang + "' and (Month(ThoiGian" + cot + ") = " + thang.Substring(0, 2) + " and Year(ThoiGian" + cot + ") = " + thang.Substring(3, 4) + ")";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            int sl = 0;
            sl = Convert.ToInt32(cmd.ExecuteScalar().ToString());


            //cập nhật vào cột nhập-xuất-gửi của chi tiết XNT
            string query_update = "update ChiTietXuatNhapTon set " + cot + " = " + sl + " where (Month(ThoiGian) = " + thang.Substring(0, 2) + " and Year(ThoiGian) = " + thang.Substring(3, 4) + ") and Xoa = false and MaHang = '" + mahang + "' and TenHang = '" + tenhang + "'";
            OleDbCommand cmdget_update = new OleDbCommand(query_update, connection);
            cmdget_update.ExecuteNonQuery();
            connection.Close();
        }


        #endregion Cập nhật XNT



        #region dt_notexist
        private void show_ThanhToan(DataTable dt)
        {
            int value = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                value = value + Convert.ToInt32(dt.Rows[i].ItemArray[5].ToString());
            }

            labelTongCong.Text = m.format_N2T(value);
        }

        private void get_HangGui(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].ItemArray[6] != null && dt.Rows[i].ItemArray[6] != "")
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            dt_hang_gui = dt.Copy();
        }

        private void textBoxKhachTra_TextChanged(object sender, EventArgs e)
        {
            int khachtra = 0;
            if (textBoxKhachTra.Text == "" || textBoxKhachTra.Text == null)
            {
                labelThanhToan.Text = "Khách nợ";
            }
            else
            {
                int value = m.format_T2N(labelTongCong.Text) - m.format_T2N(textBoxKhachTra.Text);
                if (value <= 0)
                {
                    labelThanhToan.Text = arr_info[5] = "Thanh toán đủ";
                }else labelThanhToan.Text = arr_info[5] = "Khách nợ";
            }

            m.format_money_validating(textBoxKhachTra);
        }

        private void textBoxKhachTra_KeyPress(object sender, KeyPressEventArgs e)
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


        private bool check_donhanggui()
        {
            if (dt_hang_gui.Rows.Count > 0)
            {
                return true;
            }
            else return false;
        }

        private void buttonHoanThanh_Click(object sender, EventArgs e)
        {
            if (textBoxTenKhach.Text.Trim() != null && textBoxTenKhach.Text.Trim() != "")
            {
                int khachtra = 0; int sum_sl = 0, sum_thanhtien = 0;
                if (textBoxKhachTra.Text != null && textBoxKhachTra.Text != "") khachtra = m.format_T2N(textBoxKhachTra.Text);

                string maphieu = "";
                connection.Open();
                string query_get = "SELECT TOP 1 * FROM PhieuXuatHang ORDER BY Id DESC";
                da = new OleDbDataAdapter(query_get, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    maphieu = "1";
                }
                else maphieu = (Convert.ToInt32(dt.Rows[0].ItemArray[2].ToString()) + 1).ToString();

                int length = maphieu.Length;
                for (int i = 0; i < (6 - length); i++)
                {
                    maphieu = "0" + maphieu;
                }
                connection.Close();



                if (Insert_PhieuXuatHang(time, maphieu, khachtra))
                {
                    for (int i = 0; i < dt_hang.Rows.Count; i++)
                    {
                        sum_sl = Convert.ToInt32(dt_hang.Rows[i].ItemArray[3]);
                        sum_thanhtien = Convert.ToInt32(dt_hang.Rows[i].ItemArray[5]);

                        if (Insert_ChiTietPhieuXuatHang(maphieu, i) == false)
                        {
                            MessageBox.Show("Lỗi kết nối, tạo phiếu xuất hàng thất bại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    CapNhat_Gui_Xuat_table(maphieu);

                    MessageBox.Show("Tạo phiếu xuất hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonHoanThanh.Enabled = textBoxTenKhach.Enabled = textBoxDiaChi.Enabled = textBoxSDT.Enabled = textBoxKhachTra.Enabled = false;
                    buttonInLienKhachHang.Enabled = buttonInLienThuKho.Enabled = true;
                } else return;

                arr_info[0] = maphieu; arr_info[1] = textBoxTenKhach.Text.Trim(); arr_info[2] = textBoxDiaChi.Text.Trim(); arr_info[3] = textBoxSDT.Text.Trim();
                if (dt_hang_gui.Rows.Count > 0) arr_info[4] = "Phiếu xuất có hàng gửi"; else arr_info[4] = "";
                arr_info[6] = m.format_N2T(sum_sl); arr_info[7] = m.format_N2T(sum_thanhtien);

            }
        }


        private bool Insert_PhieuXuatHang(string time, string maphieu, int khachtra)
        {
            try
            {
                connection.Open();
                string query = "insert into PhieuXuatHang (ThoiGianXuat, MaPhieuXuatHang, TenKhach, DiaChi, SDT, TongTien, KhachTra, ThanhToan, DonHangGui ) values ('" + time + "', '" + maphieu + "', '" + textBoxTenKhach.Text.Trim() + "', '" + textBoxDiaChi.Text + "', '" + textBoxSDT.Text + "', " + m.format_T2N(labelTongCong.Text) + ", " + khachtra + ", '" + labelThanhToan.Text + "', " + check_donhanggui() + ")";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch
            {
                MessageBox.Show("Lỗi kết nối, tạo phiếu xuất hàng thất bại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }


        private bool Insert_ChiTietPhieuXuatHang(string maphieu, int vitri)
        {
            //string ghichu = "";
            //if (dataGridViewDonHang.Rows[vitri].Cells[6].Value != null && dataGridViewDonHang.Rows[vitri].Cells[6].Value != "") slgui = Convert.ToInt32(dataGridViewDonHang.Rows[vitri].Cells[6].Value.ToString());
            //if (dt_hang.Rows[vitri].ItemArray[6] != null) ghichu = dt_hang.Rows[vitri].ItemArray[6].ToString(); else ghichu = "";

            try
            {
                connection.Open();
                string query = "insert into ChiTietPhieuXuatHang (MaPhieuXuatHang, MaHang, TenHang, DonViTinh, SoLuong, DonGia, ThanhTien, SlGui, SLGiao, GhiChu) values ('" + maphieu + "', '" + dt_hang.Rows[vitri].ItemArray[1].ToString() + "', '" + dt_hang.Rows[vitri].ItemArray[2].ToString() + "', 'Thùng', " + m.format_T2N(dt_hang.Rows[vitri].ItemArray[3].ToString()) + ", " + m.format_T2N(dt_hang.Rows[vitri].ItemArray[4].ToString()) + ", " + m.format_T2N(dt_hang.Rows[vitri].ItemArray[5].ToString()) + ", " + m.format_T2N(dt_hang.Rows[vitri].ItemArray[7].ToString()) + ", " + m.format_T2N(dt_hang.Rows[vitri].ItemArray[8].ToString()) + ", '" + dt_hang.Rows[vitri].ItemArray[6].ToString() + "')";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                connection.Close();
                return false;
            }
        }

        private void Insert_HangGui(string time, int vitri, string maphieu)
        {
            connection.Open();
            string query = "insert into HangGui (MaHang, TenHang, SoLuongGui, DonViTinh, ThoiGianGui, MaPhieuXuatHang, TenKhach) values ('" + dt_hang.Rows[vitri].ItemArray[1].ToString() + "', '" + dt_hang.Rows[vitri].ItemArray[2].ToString() + "', " + m.format_T2N(dt_hang.Rows[vitri].ItemArray[7].ToString()) + ", 'Thùng', '" + time + "', '" + maphieu + "', '" + textBoxTenKhach.Text.Trim() + "')";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        private void buttonInLienKhachHang_Click(object sender, EventArgs e)
        {
            string filename = Application.StartupPath + "\\PhieuXuatHang\\Khach\\" + @"\PhieuXuatHang_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            m_report.PDF_PhieuXuatHang(filename, dt_hang, arr_info, true);
            System.Diagnostics.Process.Start(filename);
        }

        private void buttonInLienThuKho_Click(object sender, EventArgs e)
        {
            string filename = Application.StartupPath + "\\PhieuXuatHang\\ThuKho\\" + @"\PhieuXuatHang_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            m_report.PDF_PhieuXuatHang(filename, dt_hang, arr_info, false);
            System.Diagnostics.Process.Start(filename);
        }


        private void CapNhat_Gui_Xuat_table(string maphieu)
        {
            check_XNT_database();

            for (int i = 0; i < dt_hang.Rows.Count; i++)
            {
                if (dt_hang.Rows[i].ItemArray[7] != null && dt_hang.Rows[i].ItemArray[7] != "")
                {
                    Insert_HangGui(time, i, maphieu);
                    update_cotnhap_XNT(DateTime.Now.ToString("MM/yyyy"), "Gui", dt_hang.Rows[i].ItemArray[1].ToString(), dt_hang.Rows[i].ItemArray[2].ToString());
                }

                connection.Open();
                string query = "insert into HangXuat (TenKhach, MaHang, TenHang, SoLuongXuat, DonViTinh, ThoiGianXuat) values ('" + textBoxTenKhach.Text.Trim() + "', '" + dt_hang.Rows[i].ItemArray[1].ToString() + "', '" + dt_hang.Rows[i].ItemArray[2].ToString() + "', " + m.format_T2N(dt_hang.Rows[i].ItemArray[3].ToString()) + ", 'Thùng', '" + time + "')";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();

                update_cotnhap_XNT(DateTime.Now.ToString("MM/yyyy"), "Xuat", dt_hang.Rows[i].ItemArray[1].ToString(), dt_hang.Rows[i].ItemArray[2].ToString());


                capnhat_HangHoa(i);
            }
        }

        private void capnhat_HangHoa (int vitri)
        {
            connection.Open();
            string query_get = "select SoLuongTon from HangHoa where Xoa = false and MaHang = '" + dt_hang.Rows[vitri].ItemArray[1].ToString() + "' and '" + dt_hang.Rows[vitri].ItemArray[2].ToString() + "'";
            OleDbCommand cmd_get = new OleDbCommand(query_get, connection);
            int sl = Convert.ToInt32(cmd_get.ExecuteScalar().ToString());

            int sl_tu_bang = 0;
            if (dt_hang.Rows[vitri].ItemArray[8] != null && dt_hang.Rows[vitri].ItemArray[8] != "") sl_tu_bang = Convert.ToInt32(dt_hang.Rows[vitri].ItemArray[8].ToString()); else sl_tu_bang = 0;

            string query = "update HangHoa set SoLuongTon = " + (sl - sl_tu_bang) + " where Xoa = false and MaHang = '" + dt_hang.Rows[vitri].ItemArray[1].ToString() + "' and '" + dt_hang.Rows[vitri].ItemArray[2].ToString() + "'";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.ExecuteNonQuery();


            string query_update_tonCK = "update ChiTietXuatNhapTon set TonCK = " + (sl - sl_tu_bang) + " where (Month(ThoiGian) = " + (DateTime.Now.ToString("MM/yyyy")).Substring(0, 2) + " and Year(ThoiGian) = " + (DateTime.Now.ToString("MM/yyyy")).Substring(3, 4) + ") and Xoa = false and MaHang = '" + dt_hang.Rows[vitri].ItemArray[1].ToString() + "' and '" + dt_hang.Rows[vitri].ItemArray[2].ToString() + "'";
            OleDbCommand cmdget_update_tonCK = new OleDbCommand(query_update_tonCK, connection);
            cmdget_update_tonCK.ExecuteNonQuery();
            connection.Close();
        }
        #endregion dt_notexist



        #region dt_exist
        private void show_TT(DataTable dt_exist)
        {
            labelMaPhieu.Text = "#" + dt_exist.Rows[0].ItemArray[2].ToString();
            textBoxTenKhach.Text = dt_exist.Rows[0].ItemArray[3].ToString();
            textBoxDiaChi.Text = dt_exist.Rows[0].ItemArray[4].ToString();
            textBoxSDT.Text = dt_exist.Rows[0].ItemArray[5].ToString();

            labelTongCong.Text = m.format_N2T(Convert.ToInt32(dt_exist.Rows[0].ItemArray[6].ToString()));
            textBoxKhachTra.Text = m.format_N2T(Convert.ToInt32(dt_exist.Rows[0].ItemArray[7].ToString()));
            labelThanhToan.Text = dt_exist.Rows[0].ItemArray[8].ToString();

            connection.Open();
            string query = "select MaHang as [Mã hàng], TenHang as [Tên hàng], SoLuong as [SL], DonGia as [Đơn giá], ThanhTien as [Thành tiền], SLGui as [SL gửi], SLGiao as [SL Giao], GhiChu as [Ghi chú] from ChiTietPhieuXuatHang where Xoa = false and MaPhieuXuatHang = '" + dt_exist.Rows[0].ItemArray[2].ToString() + "'";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            update_STT(dt);
            dt_before_change = dt.Copy();
            dataGridViewDonHang.DataSource = dt;
            connection.Close();

            dataGridViewDonHang.MultiSelect = false;
            dataGridViewDonHang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void update_STT(DataTable dt)
        {
            DataColumnCollection columns = dt.Columns;
            if (!columns.Contains("STT"))
            {
                dt.Columns.Add("STT").SetOrdinal(0);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][0] = (i + 1).ToString();
                }
            }
        }


        private void dataGridViewDonHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewDonHang.CurrentCell.RowIndex;

            if (row >= 0)
            {
                //frm_chitiethangcansua f = new frm_chitiethangcansua(dr);
                //f.ShowDialog();
            }
        }



        #endregion dt_existq


    }
}
