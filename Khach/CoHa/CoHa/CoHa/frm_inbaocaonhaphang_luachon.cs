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
    public partial class frm_inbaocaonhaphang_luachon : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        mt_export_pdf_excel_file mt_export = new mt_export_pdf_excel_file();
        string choice = "";

        public frm_inbaocaonhaphang_luachon(String luachon)
        {
            InitializeComponent();

            connection.ConnectionString = connectionSTR;

            choice = luachon;
            if (choice == "XNT")
            {
                comboBoxLoaiBaoCao.Items.RemoveAt(0);
            }

            comboBoxLoaiBaoCao.SelectedIndex = 0;
        }

        private DataTable DatagridviewBaoCao_Created(string [] array, DataTable dt)
        {
            for (int i = 0; i < array.Length; i++)
            {
                dt.Columns[i].ColumnName = array[i].ToString();
            }
            return dt;
        }

        private void comboBoxDonViTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLoaiBaoCao.SelectedIndex == 0)
            {
                panelNgay.Visible = true;
                panelThang.Visible = false;
            }
            else
            {
                panelNgay.Visible = false;
                panelThang.Visible = true;
            }
        }

        private void buttonIn_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string ngay = dateTimePickerNgay.Value.ToString("dd/MM/yyyy");
            string thang = dateTimePickerTu.Value.ToString("MM/yyyy");

            if (choice == "nhập")
            {
                string[] mang_headertext = { "Thời gian", "Mã hàng", "Tên hàng", "SL nhập", "ĐVT" };
                int[] arr_size = { 27, 23, 43, 15, 20 };
                object[] arr_position = { OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center };

                if (comboBoxLoaiBaoCao.Text == "Báo cáo ngày")
                {
                    connection.Open();
                    string query = "select ThoiGianNhap, MaHang, TenHang, SoLuongNhap, DonViTinh from HangNhap where Xoa = false and (ThoiGianNhap >= #" + ngay + " 00:00:00# and ThoiGianNhap <= #" + ngay + " 23:59:59#) order by ThoiGianNhap asc";
                    da = new OleDbDataAdapter(query, connection);
                    da.Fill(dt);
                    connection.Close();
                    mt_export.Excel(DatagridviewBaoCao_Created(mang_headertext, dt), "HÀNG NHẬP NGÀY ", ngay, arr_size, arr_position);
                }
                else
                {
                    connection.Open();
                    string query = "select ThoiGianNhap, MaHang, TenHang, SoLuongNhap, DonViTinh from HangNhap where Xoa = false and Month(ThoiGianNhap) = " + thang.Substring(0, 2) + " and Year(ThoiGianNhap) = " + thang.Substring(3, 4) + " order by ThoiGianNhap asc";
                    da = new OleDbDataAdapter(query, connection);
                    da.Fill(dt);
                    connection.Close();
                    mt_export.Excel(DatagridviewBaoCao_Created(mang_headertext, dt), "HÀNG NHẬP THÁNG ", thang, arr_size, arr_position);
                }
            }
            else if (choice == "xuất")
            {
                string[] mang_headertext = { "Thời gian", "Tên khách", "Mã hàng", "Tên hàng", "SL nhập", "ĐVT" };
                int[] arr_size = { 27, 22, 23, 43, 15, 20 };
                object[] arr_position = { OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center };
                if (comboBoxLoaiBaoCao.Text == "Báo cáo ngày")
                {
                    connection.Open();
                    string query = "select ThoiGianXuat, TenKhach, MaHang, TenHang, DonViTinh, SoLuongXuat from HangXuat where Xoa = false and (ThoiGianXuat >= #" + ngay + " 00:00:00# and ThoiGianXuat <= #" + ngay + " 23:59:59#) order by TenKhach asc";
                    da = new OleDbDataAdapter(query, connection);
                    da.Fill(dt);
                    connection.Close();
                    mt_export.Excel(DatagridviewBaoCao_Created(mang_headertext, dt), "HÀNG XUẤT NGÀY ", ngay, arr_size, arr_position);
                }
                else
                {
                    connection.Open();
                    string query = "select ThoiGianXuat, TenKhach, MaHang, TenHang, DonViTinh, SoLuongXuat from HangXuat where Xoa = false and Month(ThoiGianXuat) = " + thang.Substring(0, 2) + " and Year(ThoiGianXuat) = " + thang.Substring(3, 4) + " order by ThoiGianXuat asc";
                    da = new OleDbDataAdapter(query, connection);
                    da.Fill(dt);
                    connection.Close();
                    mt_export.Excel(DatagridviewBaoCao_Created(mang_headertext, dt), "HÀNG XUẤT THÁNG ", thang, arr_size, arr_position);
                }
            }
            else
            {
                string[] mang_headertext = { "STT", "Mã hàng", "Tên hàng", "Tồn ĐK", "Nhập", "Xuất", "Tồn CK" };
                int[] arr_size = { 6, 23, 43, 12, 12, 12, 12 };
                object[] arr_position = { OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Left, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center, OfficeOpenXml.Style.ExcelHorizontalAlignment.Center , OfficeOpenXml.Style.ExcelHorizontalAlignment.Center};

                connection.Open();
                string query = "select MaHang, TenHang, TonDK, Nhap, Xuat, TonCK from ChiTietXuatNhapTon, XuatNhapTon where Month(XuatNhapTon.ThoiGian) = " + thang.Substring(0, 2) + " and Year(XuatNhapTon.ThoiGian) = " + thang.Substring(3, 4) + " and Xoa = false ";
                da = new OleDbDataAdapter(query, connection);
                da.Fill(dt);
                connection.Close();
                mt_export.Excel(DatagridviewBaoCao_Created(mang_headertext, dt), "XUẤT NHẬP TỒN THÁNG", thang, arr_size, arr_position);
            }



         
        }
    }
}
