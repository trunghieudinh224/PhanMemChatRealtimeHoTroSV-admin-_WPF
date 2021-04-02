using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;       
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VanSon.Method
{
    class mt_sudungchung        
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();

        public string format_N2T(int x)
        {
            return x.ToString("#,##,##,##,##,##,##,##,##,##,##,##,##,###");
        }

        public int format_T2N(string x)
        {
            if (x == "")
            {
                x = "0";
            }
            return Convert.ToInt32(x.Trim().Replace(",", "").Replace(".", "").Replace("- ", "").Replace(" ", ""));
        }

        public void format_money_validating(TextBox textbox)
        {
            if (textbox.Text != "")
            {
                textbox.Text = format_N2T(format_T2N(textbox.Text.Trim()));
                textbox.SelectionStart = textbox.Text.Length;
                textbox.SelectionLength = 0;
            }
        }

        public void update_STT(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][0] = (i + 1);
            }
        }

        public void update_MaHang_TenHang(string mahang_cu, string tenhang_cu, string mahang_moi, string tenhang_moi)
        {
            connection.ConnectionString = connectionSTR;

            string[] arr_Bang = { "HangNhap", "HangXuat", "HangGui", "ChiTietPhieuXuatHang", "ChiTietXuatNhapTon"};
            for (int i = 0; i < arr_Bang.Length; i++)
            {
                connection.Open();
                string query = "update "+arr_Bang[i]+" set MaHang = '"+mahang_moi+"', TenHang = '"+tenhang_moi+"' where MaHang = '"+mahang_cu+"' and TenHang = '"+tenhang_moi+"'";
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            
        }
    }
}
