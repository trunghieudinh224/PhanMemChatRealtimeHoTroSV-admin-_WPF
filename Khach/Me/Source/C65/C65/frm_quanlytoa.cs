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
    public partial class frm_quanlytoa : Form
    {
        mt_connection connection = new mt_connection();
        mt_sudungchung m = new mt_sudungchung();
        mt_select mt_select = new mt_select();
        string trangthai = "";

        public frm_quanlytoa()
        {
            InitializeComponent();
            //connection.connection_project();

            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        public void HienThiToa(String tungay, String denngay)
        {
            dataGridViewDanhSachToa.DataSource = mt_select.HienThiToa(textBoxMaToa.Text.Trim(), textBoxTenKH.Text.Trim(), m.format_T2N(textBoxTongToa.Text.Trim()), tungay, denngay, trangthai);
            if (dataGridViewDanhSachToa.Rows.Count < 9)
            {
                dataGridViewDanhSachToa.Columns[2].Width = 130;
                dataGridViewDanhSachToa.Columns[4].Width = 119;
            }
        }

        private void textBoxMaToa_TextChanged(object sender, EventArgs e)
        {
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void textBoxTenKH_TextChanged(object sender, EventArgs e)
        {
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void textBoxTongToa_TextChanged(object sender, EventArgs e)
        {
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void dateTimePickerTuNgay_ValueChanged(object sender, EventArgs e)
        {
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void dateTimePickerDenNgay_ValueChanged(object sender, EventArgs e)
        {
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void checkBoxHienTatCaToa_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHienTatCaToa.Checked == true)
            {
                HienThiToa(null, null);
            }
            else HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }


        private void radioButtonMacDinh_CheckedChanged(object sender, EventArgs e)
        {
            trangthai = "";
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void radioButtonDaThanhToan_CheckedChanged(object sender, EventArgs e)
        {
            trangthai = "Hoàn thành";
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void radioButtonKhachNo_CheckedChanged(object sender, EventArgs e)
        {
            trangthai = "Khách nợ";
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }

        private void radioButtonNoKhach_CheckedChanged(object sender, EventArgs e)
        {
            trangthai = "Nợ Khách";
            HienThiToa(dateTimePickerTuNgay.Value.ToString("MM/dd/yyyy"), dateTimePickerDenNgay.Value.ToString("MM/dd/yyyy"));
        }





        //Hàm đổi tiếng việt có dấu thành không dấu
        //public static string convertToUnSign3(string s)
        //{
        //    Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
        //    string temp = s.Normalize(NormalizationForm.FormD);
        //    return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        //}
    }
}
