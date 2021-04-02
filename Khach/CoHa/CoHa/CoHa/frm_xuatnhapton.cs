using CoHa;
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
    public partial class frm_xuatnhapton : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        mt_sudungchung m = new mt_sudungchung();
        mt_xuatnhapton mt_xnt = new mt_xuatnhapton();
        mt_export_pdf_excel_file mt_export = new mt_export_pdf_excel_file();
        DataTable dt_total = new DataTable();

        public frm_xuatnhapton()
        {
            InitializeComponent();

            connection.ConnectionString = connectionSTR;

            check_XNT_exist_database();
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



        private void check_XNT_exist_database()
        {
            string thang = dateTimePickerXNT.Value.ToString("MM/yyyy");

            if (mt_xnt.check_TonDau(thang) == 0)
            {
                if (mt_xnt.creat_XNT(thang) == true)
                {
                    mt_xnt.create_ChiTietXNT(thang, mt_xnt.get_ChiTietXNT_thangmoi(thang));
                }
                else
                {
                    MessageBox.Show("Lỗi kết nối", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            dataGridViewXNT.DataSource = mt_xnt.showData(thang);
        }


        private void dateTimePickerXNT_ValueChanged(object sender, EventArgs e)
        {
            DateTime time_chon = dateTimePickerXNT.Value;
            DateTime time_hientai = DateTime.Now;

            if (time_chon > time_hientai)
            {
                dateTimePickerXNT.Value = time_hientai;
            }
            else
            {
                check_XNT_exist_database();
            }
        }


        private DataTable DatagridviewBaoCao_Created(string[] array, DataTable dt)
        {
            for (int i = 0; i < array.Length; i++)
            {
                dt.Columns[i].ColumnName = array[i].ToString();
            }
            return dt;
        }


        private void buttonInBaoCaoNhapHang_Click(object sender, EventArgs e)
        {
            string[] column_name = { "STT", "Mã hàng", "Tên hàng","Tồn ĐK", "Nhập", "Xuất", "Gửi", "Tồn CK" };
            int[] arr_size = { 8, 23, 43, 12, 12, 12, 12, 12 };
            DataTable dv = dt_total.Copy();

            object[] arr_position = { OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center };
            mt_export.Excel(DatagridviewBaoCao_Created(column_name, dv), "XUẤT NHẬP TỒN ", dateTimePickerXNT.Value.ToString("MM/yyyy"), arr_size, arr_position);
        }
    }
}
