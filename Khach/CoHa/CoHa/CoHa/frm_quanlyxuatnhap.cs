using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VanSon.Method;
using VanSon;
using System.Drawing.Drawing2D;

namespace CoHa
{
    public partial class frm_quanlyxuatnhap : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        mt_sudungchung m = new mt_sudungchung();
        mt_xuatnhapton mt_xnt = new mt_xuatnhapton();
        mt_export_pdf_excel_file mt_export = new mt_export_pdf_excel_file();
        List<HangNhapHangXuat> SP_Chon_Nhap = new List<HangNhapHangXuat>();
        List<HangNhapHangXuat> SP_Chon_Xuat = new List<HangNhapHangXuat>();
        private String tenkhach = string.Empty;
        public int count_success = 0;
        string tooltip_message = "";

        public frm_quanlyxuatnhap(int role)
        {
            InitializeComponent();

            connection.ConnectionString = connectionSTR;

            checkrole(role);

            HienThi_Tooltip_Button();

            HangHoa();

            NhapHang();

            XuatHang_KeToan();

            XuatHang_ThuKho();

            HangGui();
        }


        #region checkrole
        private void checkrole(int role)
        {
            if (role == 1)
            {
                tabControlNhapXuat.TabPages.Remove(tabPageXuatHang_SL);
            }
            else if (role == 2)
            {
                tabControlNhapXuat.TabPages.Remove(tabPageXuatHang);
            }
        }
        #endregion checkrole


        #region Tooltip
        private void HienThi_Tooltip_Button()
        {
            settooltip_button(buttonXNT, "In hàng tồn hiện tại");
            settooltip_button(buttonInBaoCaoNhapHang, "In báo cáo hàng nhập");
            settooltip_button(buttonInBaoCaoXuatHang, "In báo cáo xuất hàng");
            settooltip_button(buttonInBaoCaoXuatHang_ThuKho, "In báo cáo xuất hàng");
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


        #region tabcontrol
        private void tabControlNhapXuat_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (TabPage tab in tabControlNhapXuat.TabPages)
            {
                if ((tab.Name).Equals("tabPageHangHoa"))
                {
                    ShowThongTinHangHoa();
                }
                else if ((tab.Name).Equals("tabPageNhapHang"))
                {
                    ShowThongTin_HangNhap();
                }
                else if ((tab.Name).Equals("tabPageXuatHang"))
                {
                    XuatHang_KeToan();
                }
                else if ((tab.Name).Equals("tabPageXuatHang_SL"))
                {
                    ShowThongTin_XuatHang_ThuKho();
                }
                else
                {
                    ShowThongTinHangGui();
                }
            }
        }
        #endregion tabcontrol


        #region tìm kiếm
        private void Search(String query, DataGridView dtg)
        {
            connection.Open();
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dtg.DataSource = dt;
            connection.Close();
        }
        #endregion tìm kiếm



        #region Xoa
        private bool Xoa(List<HangNhapHangXuat> SP_Chon, string mess_xoa, string loai, PictureBox ptb, Timer tm, bool update_slt)
        {
            if (SP_Chon.Count > 0)
            {
                String time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                DialogResult dlr = MessageBox.Show("Bạn có chắc muốn xóa "+mess_xoa+" này", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dlr == DialogResult.OK)
                {
                    connection.Open();
                    string query = "update Hang"+loai+" set Xoa = true, ThoiGianXoa = '" + time + "' where ThoiGian" + loai + " like ('%" + SP_Chon[0].getThoiGian() + "%') and MaHang = '" + SP_Chon[0].getMaHang() + "' and  TenHang = '" + SP_Chon[0].getTenHang() + "'";
                    OleDbCommand cmd = new OleDbCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    ptb.Visible = true;
                    tm.Start();
                    mt_xnt.update_SoLuongTon_KhaDung_HangHoa(SP_Chon[0].getMaHang(), SP_Chon[0].getTenHang(), SP_Chon[0].getSL(), update_slt);
                    ShowThongTin_HangNhap();
                }
                return true;
            }
            else
            {
                return false;
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa !!!", "Thông báo");
            }
        }
        #endregion Xoa


        #region Hàng hóa
        private void HangHoa()
        {
            ShowThongTinHangHoa();
        }

        private void ShowThongTinHangHoa()
        {
            connection.Open();
            string query = "select MaHang, TenHang, SoLuongTon, KhaDung, DonViTinh from HangHoa where Xoa = false order by TenHang asc";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            connection.Close();
            dataGridViewHangHoa.DataSource = dt;
        }

        private void dataGridViewHangHoa_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewHangHoa.CurrentCell.RowIndex;

            if (row >= 0)
            {
                frm_chitiethanghoa f = new frm_chitiethanghoa(dataGridViewHangHoa.Rows[row].Cells[0].Value.ToString(), dataGridViewHangHoa.Rows[row].Cells[1].Value.ToString());
                f.ShowDialog();
                ShowThongTinHangHoa();
            }
        }


        private bool check_ma_ten(string column_select, string mahang, string tenhang)
        {
            connection.Open();
            string query = "select count(MaHang) from HangHoa where '"+ column_select + "' = '"+mahang+"'";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            int value = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            connection.Close();
            if (value > 0)
            {
                return false;
            }
            else return true;
            
        }

        private void ThemSanPham()
        {
            if (textBoxMaSP_HangHoa.Text.Trim() != "" && textBoxTenSP_HangHoa.Text.Trim() != "")
            {
                if (check_ma_ten("MaHang", textBoxMaSP_HangHoa.Text.Trim(), textBoxTenSP_HangHoa.Text.Trim()) == true && check_ma_ten("TenHang", textBoxMaSP_HangHoa.Text.Trim(), textBoxTenSP_HangHoa.Text.Trim()))
                {
                    string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    connection.Open();
                    string query = "insert into HangHoa (MaHang,TenHang,DonViTinh,NgayTao,NguoiTao) values ('" + textBoxMaSP_HangHoa.Text + "', '" + textBoxTenSP_HangHoa.Text + "', 'Thùng', '" + time + "', '" + frm_dangnhap.user + "')";
                    OleDbCommand cmd = new OleDbCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    pictureBoxThongBaoHangHoa.Visible = true;
                    timer_HangHoa.Start();
                    ShowThongTinHangHoa();
                }
                else
                {
                    MessageBox.Show("Mã hàng hoặc tên hàng đã tồn tại !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timer_HangHoa_Tick(object sender, EventArgs e)
        {
            count_success++;
            if (count_success == 2)
            {
                timer_HangHoa.Stop();
                pictureBoxThongBaoHangHoa.Visible = false;
                timer_HangHoa.Enabled = false;
                count_success = 0;
            }
        }

        private void buttonThemSP_HangHoa_Click(object sender, EventArgs e)
        {
            ThemSanPham();
        }


        private void textBoxTimKiemHangHoa_TextChanged(object sender, EventArgs e)
        {
            string value_search = textBoxTimKiemHangHoa.Text;
            string query = "select MaHang, TenHang, SoLuongTon from HangHoa where Xoa = false and (MaHang like ('%" + value_search + "%') or TenHang like ('%" + value_search + "%') or SoLuongTon like ('%" + value_search + "%') )";
            Search(query, dataGridViewHangHoa);
        }


        private void buttonXNT_Click(object sender, EventArgs e)
        {
            frm_xuatnhapton f = new frm_xuatnhapton();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        #endregion Hàng hóa



        #region Nhập hàng
        private void NhapHang()
        {
            ShowThongTin_HangNhap();

            comboboxSanPham();
        }

        private void ShowThongTin_HangNhap()
        {
            connection.Open();

            string query = "select ThoiGianNhap, MaHang, TenHang, SoLuongNhap, DonViTinh from HangNhap where Xoa = false order by ThoiGianNhap desc";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            design_column_datagridview(dataGridViewHangNhap, dt);
            if (dt.Rows.Count > 0)
            {
                SP_Chon_Nhap.Clear();
                SP_Chon_Nhap.Add(new HangNhapHangXuat(dt.Rows[0].ItemArray[1].ToString() , dt.Rows[0].ItemArray[2].ToString(), Convert.ToInt32(dt.Rows[0].ItemArray[3].ToString()), dt.Rows[0].ItemArray[0].ToString()));
            }
            connection.Close();
        }

        private void design_column_datagridview(DataGridView dtg, DataTable dt)
        {
            dtg.DataSource = dt;
            dtg.ColumnHeadersHeight = 60;
            dtg.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dtg.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dtg.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dtg.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dtg.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void comboboxSanPham()
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

            comboBoxTenSP.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxTenSP.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboBoxTenSP.DataSource = dt;
            comboBoxTenSP.ValueMember = "TenHang";
            comboBoxTenSP.DisplayMember = "TenHang";
        }

        private void buttonThem_Click(object sender, EventArgs e)
        {
            if (this.comboBoxTenSP.Text.Trim() != ""  && m.format_T2N(textBoxSoLuongNhap.Text.Trim()) > 0)
            {
                connection.Open();
                string query = "insert into HangNhap (MaHang,TenHang,SoLuongNhap,DonViTinh,ThoiGianNhap) values ('" + textBoxMaSP.Text.Trim() + "', '" + comboBoxTenSP.Text.Trim() + "', " + m.format_T2N(textBoxSoLuongNhap.Text.Trim()) + ", 'Thùng', '" + dateTimePickerNgayNhap.Value.ToString("dd/MM/yyyy HH:mm:ss") + "')";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                mt_xnt.update_SoLuongTon_KhaDung_HangHoa(textBoxMaSP.Text.Trim(), comboBoxTenSP.Text.Trim(), m.format_T2N(textBoxSoLuongNhap.Text.Trim()), true);
                pictureBoxThongBaoNhapHang.Visible = true;
                timer_NhapHang.Start();
                reset_HangNhap();
                ShowThongTin_HangNhap();
            }
            else
            {
                MessageBox.Show("Vui lòng cung cấp đầy đủ thông tin sản phẩm !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void reset_HangNhap()
        {
            textBoxMaSP.Text = comboBoxTenSP.Text = textBoxSoLuongNhap.Text = "";   
        }

        private void textBoxTimKiemSP_TextChanged(object sender, EventArgs e)
        {
            string value_search = textBoxTimKiemSP.Text.Trim();
            string query = "select ThoiGianNhap, MaHang, TenHang, SoLuongNhap, DonViTinh from HangNhap where Xoa = false and (ThoiGianNhap like ('%" + value_search + "%') or MaHang like ('%" + value_search + "%') or TenHang like ('%" + value_search + "%') or SoLuongNhap like ('%" + value_search + "%') or DonViTinh like ('%" + value_search + "%'))  order by ThoiGianNhap desc";
            Search(query, dataGridViewHangNhap);
        }

        private void dataGridViewHangNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewHangNhap.CurrentCell.RowIndex;
            SP_Chon_Nhap.Clear();


            if (row >= 0)
            {
                SP_Chon_Nhap.Add(new HangNhapHangXuat(dataGridViewHangNhap.Rows[row].Cells[1].Value.ToString(), dataGridViewHangNhap.Rows[row].Cells[2].Value.ToString(), Convert.ToInt32(dataGridViewHangNhap.Rows[row].Cells[3].Value.ToString()), dataGridViewHangNhap.Rows[row].Cells[0].Value.ToString()));
            }
        }

        private void buttonXoa_Click(object sender, EventArgs e)
        {
            if (!Xoa(SP_Chon_Nhap, "phiếu nhập", "Nhap", pictureBoxThongBaoNhapHang, timer_NhapHang, false)) MessageBox.Show("Vui lòng chọn phiếu nhập cần xóa !!!", "Thông báo");
            ShowThongTin_HangNhap();
        }

        private void timer_NhapHang_Tick(object sender, EventArgs e)
        {
            count_success++;
            if (count_success == 2)
            {
                timer_NhapHang.Stop();
                pictureBoxThongBaoNhapHang.Visible = false;
                timer_NhapHang.Enabled = false;
                count_success = 0;
            }
        }

        private void buttonInBaoCaoNhapHang_Click(object sender, EventArgs e)
        {
            frm_inbaocaonhaphang_luachon f = new frm_inbaocaonhaphang_luachon("nhập");
            f.ShowDialog();
        }

        private void textBoxSLTon_KeyPress(object sender, KeyPressEventArgs e)
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
       
        private void textBoxSLTon_TextChanged(object sender, EventArgs e)
        {
            m.format_money_validating(textBoxSoLuongNhap);
        }

        private void comboBoxTenSP_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxTenSP.SelectedIndex > 0)
            {
                connection.Open();
                string query = "select MaHang from HangHoa where Xoa =  false and TenHang = '" + comboBoxTenSP.Text.Trim() + "'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                string masp = cmd.ExecuteScalar().ToString();
                textBoxMaSP.Text = masp;
                connection.Close();
            }
        }

        private void comboBoxTenSP_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxTenSP.SelectedIndex != -1) comboBoxTenSP.SelectedIndex = -1;
            if (comboBoxTenSP.Text.Trim() != "")
            {
                connection.Open();
                string query = "select MaHang, SoLuongTon from HangHoa where Xoa = false and TenHang = '" + comboBoxTenSP.Text.Trim() + "'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    textBoxMaSP.Text = dt.Rows[0].ItemArray[0].ToString();
                }

                connection.Close();
            }
            else textBoxMaSP.Text = null;
        }

        #endregion Nhập hàng



        #region Xuất hàng (kế toán)
        private void XuatHang_KeToan()
        {
            ThongTinPhieuXuatHang();
        }

        private void ThongTinPhieuXuatHang()
        {
            connection.Open();
            string query = "select ThoiGianXuat, MaPhieuXuatHang, TenKhach, TongTien, DonHangGui, ThanhToan from PhieuXuatHang where Xoa = false order by ThoiGianXuat desc";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            connection.Close();
        }

        private void buttonTaoDonHang_Click(object sender, EventArgs e)
        {
            frm_xuathang f = new frm_xuathang();
            this.Hide();
            f.ShowDialog();
            this.Show();
            ThongTinPhieuXuatHang();
        }

        private void textBoxTimPhieuXuat_TextChanged(object sender, EventArgs e)
        {
            string value_search = textBoxTimPhieuXuat.Text.Trim();
            string query = "select ThoiGianXuat, MaPhieuXuatHang, TenKhach, TongTien, DonHangGui, ThanhToan from PhieuXuatHang where Xoa = false and (ThoiGianXuat like ('%" + value_search + "%') or MaPhieuXuatHang like ('%" + value_search + "%') or TongTien like ('%" + value_search + "%') or ThanhToan like ('%" + value_search + "%'))  order by ThoiGianXuat desc";
            Search(query, dataGridViewXuatHang);
        }

        private void buttonInBaoCaoXuatHang_Click(object sender, EventArgs e)
        {
            frm_inbaocaonhaphang_luachon f = new frm_inbaocaonhaphang_luachon("xuất");
            f.ShowDialog();
        }

        private void dataGridViewXuatHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewXuatHang.CurrentCell.RowIndex;

            if (row >= 0)
            {
                connection.Open();
                string query = "select * from PhieuXuatHang where Xoa = false and MaPhieuXuatHang = '"+dataGridViewXuatHang.Rows[row].Cells[1].Value.ToString()+"'";
                da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                connection.Close();
                frm_xacnhanphieu f = new frm_xacnhanphieu(null, dt);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
        }
        #endregion Xuất hàng (kế toán)



        #region Xuất hàng (thủ kho)
        private void XuatHang_ThuKho()
        {
            ShowThongTin_XuatHang_ThuKho();

            comboboxSanPham_Xuat_ThuKho();
        }

        private void ShowThongTin_XuatHang_ThuKho()
        {
            connection.Open();
            string query = "select ThoiGianXuat, TenKhach, MaHang, TenHang, SoLuongXuat, DonViTinh from HangXuat where Xoa = false order by ThoiGianXuat desc";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            connection.Close();

            string mahang = "";
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0].ItemArray[2] == null || dt.Rows[0].ItemArray[2] == "")
                {
                    mahang = "";
                }

                SP_Chon_Xuat.Add(new HangNhapHangXuat(mahang, dt.Rows[0].ItemArray[3].ToString(), Convert.ToInt32(dt.Rows[0].ItemArray[4].ToString()), dt.Rows[0].ItemArray[0].ToString()));
            }
            dataGridViewHangXuat_ThKho.DataSource = dt;
        }

        private void comboboxSanPham_Xuat_ThuKho()
        {
            connection.Open();
            string query = "select TenHang from HangHoa where Xoa = false  order by TenHang asc";
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
                string query = "select MaHang from HangHoa where Xoa =  false and TenHang = '" + comboBoxTenHang.Text.Trim() + "'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                string masp = cmd.ExecuteScalar().ToString();
                textBoxMaHang.Text = masp;
                connection.Close();
            }

            textBoxSLT_Xuat.Text = m.format_N2T(Check_SL_Ton()); 
        }

        private int Check_SL_Ton()
        {
            if (comboBoxTenHang.SelectedIndex > 0)
            {
                connection.Open();
                string query = "select SoLuongTon from HangHoa where Xoa = false and TenHang = '" + comboBoxTenHang.Text.Trim() + "' and MaHang = '" + textBoxMaHang.Text.Trim() + "'";
                OleDbCommand cmdLayMaSP = new OleDbCommand(query, connection);
                int slt = Convert.ToInt32(cmdLayMaSP.ExecuteScalar().ToString());
                connection.Close();
                return slt;
            }
            else return 0;
        }

        private void comboBoxTenHang_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxTenHang.SelectedIndex != -1) comboBoxTenHang.SelectedIndex = -1;
            if (comboBoxTenHang.Text.Trim() != "")
            {
                connection.Open();
                string query = "select MaHang, SoLuongTon from HangHoa where Xoa = false and TenHang = '" + comboBoxTenHang.Text.Trim() + "'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                da = new OleDbDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    textBoxMaHang.Text = dt.Rows[0].ItemArray[0].ToString();
                    textBoxSLT_Xuat.Text = m.format_N2T(m.format_T2N(dt.Rows[0].ItemArray[1].ToString()));
                }
                else
                {
                    textBoxMaHang.Text = textBoxSLT_Xuat.Text = null;
                }
                connection.Close();
            }
            else textBoxMaHang.Text = textBoxSLT_Xuat.Text = null;
        }

        private void textBoxSL_Xuat_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSL_Xuat.Text != "")
            {
                if (Convert.ToInt32(textBoxSL_Xuat.Text) <= Convert.ToInt32(textBoxSLT_Xuat.Text))
                {
                    m.format_money_validating(textBoxSL_Xuat);
                }
                else
                {
                    MessageBox.Show("Số lượng xuất không lớn hơn số lượng tồn", "Thông báo");
                    textBoxSL_Xuat.Text = "";
                }
            }
        }

        private void textBoxSL_Xuat_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dataGridViewHangXuat_ThuKho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewHangXuat_ThKho.CurrentCell.RowIndex;
            SP_Chon_Xuat.Clear();

            if (row >= 0)
            {
                SP_Chon_Xuat.Add(new HangNhapHangXuat(dataGridViewHangXuat_ThKho.Rows[row].Cells[2].Value.ToString(), dataGridViewHangXuat_ThKho.Rows[row].Cells[3].Value.ToString(), Convert.ToInt32(dataGridViewHangXuat_ThKho.Rows[row].Cells[4].Value.ToString()), dataGridViewHangXuat_ThKho.Rows[row].Cells[0].Value.ToString()));
            }
        }

        private void textBoxTimKiem_Xuat_TextChanged(object sender, EventArgs e)
        {
            string value_search = textBoxTimKiem_Xuat.Text.Trim();
            string query = "select ThoiGianXuat, TenKhach, MaHang, TenHang, SoLuongXuat from HangXuat where Xoa = false and (ThoiGianXuat like ('%" + value_search + "%') or TenKhach like ('%" + value_search + "%') or MaHang like ('%" + value_search + "%') or TenHang like ('%" + value_search + "%') or SoLuongXuat like ('%" + value_search + "%'))  order by ThoiGianXuat desc";
            Search(query, dataGridViewHangXuat_ThKho);
        }

        private void buttonThem_Xuat_Click(object sender, EventArgs e)
        {
            if (textBoxMaHang.Text != "" && this.comboBoxTenHang.Text != "" && m.format_T2N(textBoxSL_Xuat.Text) > 0)
            {
                connection.Open();
                string query = "insert into HangXuat (TenKhach, MaHang,TenHang,SoLuongXuat,DonViTinh,ThoiGianXuat) values ('" + textBoxTenKhach.Text + "', '" + textBoxMaHang.Text + "', '" + comboBoxTenHang.Text + "', " + m.format_T2N(textBoxSL_Xuat.Text) + ", 'Thùng', '" + dateTimePickerXuat.Value.ToString("dd/MM/yyyy HH:mm:ss") + "')";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                mt_xnt.update_SoLuongTon_KhaDung_HangHoa(textBoxMaHang.Text.Trim(), comboBoxTenHang.Text.Trim(), m.format_T2N(textBoxSL_Xuat.Text.Trim()), false);
                pictureBoxThongBaoXuatHang.Visible = true;
                timer_XuatHang.Start();
                reset_HangXuat();
                ShowThongTin_XuatHang_ThuKho();
            }
            else
            {
                MessageBox.Show("Vui lòng cung cấp đầy đủ thông tin sản phẩm !!!", "Thông báo");
            }
        }

        private void timer_XuatHang_Tick(object sender, EventArgs e)
        {
            count_success++;
            if (count_success == 2)
            {
                timer_XuatHang.Stop();
                pictureBoxThongBaoXuatHang.Visible = false;
                timer_NhapHang.Enabled = false;
                count_success = 0;
            }
        }

        public void reset_HangXuat()
        {
            textBoxTenKhach.Text = textBoxMaHang.Text = comboBoxTenHang.Text = textBoxSLT_Xuat.Text = textBoxSL_Xuat.Text = "";
        }

        private void buttonXoa_Xuat_Click(object sender, EventArgs e)
        {
            if (!Xoa(SP_Chon_Xuat, "phiếu xuất", "Xuat", pictureBoxThongBaoXuatHang, timer_XuatHang, true)) MessageBox.Show("Vui lòng chọn phiếu xuất cần xóa !!!", "Thông báo");
            ShowThongTin_XuatHang_ThuKho();
        }

        private void buttonInBaoCaoXuatHang_ThuKho_Click(object sender, EventArgs e)
        {
            frm_inbaocaonhaphang_luachon f = new frm_inbaocaonhaphang_luachon("xuất");
            f.ShowDialog();
        }

        #endregion Xuất hàng (thủ kho)



        #region Hàng gửi
        private void HangGui()
        {
            ShowThongTinHangGui();
        }

        private void ShowThongTinHangGui()
        {
            connection.Open();

            string query = "select ThoiGianGui, MaPhieuXuatHang, TenKhach, MaHang, TenHang,  DonViTinh, SoLuongGui from HangGui where Xoa = false order by ThoiGianGui desc";
            da = new OleDbDataAdapter(query, connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dt.Columns.Remove("ThoiGianGui");
            dataGridViewHangGui.DataSource = dt;
            connection.Close();
        }

        



        #endregion Hàng gửi

        //public void docfile()
        //{
        //    string filePath = Application.StartupPath + @"\txt_danhmuc.txt";

        //    string[] lines;
        //    string str;
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("SanPham");

        //    dt.Rows.Add("--");
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        lines = System.IO.File.ReadAllLines(filePath);
        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            dt.Rows.Add(lines[i]);
        //            comboBoxTenSP.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        //            comboBoxTenSP.AutoCompleteSource = AutoCompleteSource.ListItems;

        //            comboBoxTenSP.DataSource = dt;
        //            comboBoxTenSP.ValueMember = "SanPham";
        //            comboBoxTenSP.DisplayMember = "SanPham";

        //            connection.Open();
        //            string query = "insert into HangHoa (MaHang,TenHang) values ('" + lines[i] + "', '')";
        //            OleDbCommand cmd = new OleDbCommand(query, connection);
        //            cmd.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Không tìm thấy file danh mục", "Thông báo");
        //    }
        //}
    }
}
