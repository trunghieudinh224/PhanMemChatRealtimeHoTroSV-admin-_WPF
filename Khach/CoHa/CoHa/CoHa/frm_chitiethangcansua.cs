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
    public partial class frm_chitiethangcansua : Form
    {
        public static string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        private OleDbConnection connection = new OleDbConnection();
        mt_sudungchung m = new mt_sudungchung();

        public frm_chitiethangcansua(DataRow[] dr)
        {
            InitializeComponent();

            connection.ConnectionString = connectionSTR;

            show_TT(dr);
        }

        private void show_TT(DataRow[] dr)
        {
            foreach (DataRow row in dr)
            {
                comboBoxTenHang.Text = row["Tên hàng"].ToString();
                connection.Open();
                string query = "select SoLuongTon + " + Convert.ToInt32(row["SL"].ToString()) + " from HangHoa where MaHang = '" + row["Mã hàng"].ToString() + "' and TenHang = '" + row["Tên hàng"].ToString() + "'";
                OleDbCommand cmd = new OleDbCommand(query,connection);
                textBoxTonKho.Text = m.format_N2T(m.format_T2N(cmd.ExecuteScalar().ToString()));
                connection.Close();


                textBoxMaHang.Text = row["Mã hàng"].ToString();
                textBoxXuat.Text = m.format_N2T(m.format_T2N(row["SL"].ToString()));
                textBoxDonGia.Text = m.format_N2T(m.format_T2N(row["Đơn giá"].ToString()));
                textBoxSLGui.Text = m.format_N2T(m.format_T2N(row["SL gửi"].ToString()));
                textBoxDaGiao.Text = m.format_N2T(m.format_T2N(row["SL giao"].ToString()));
                textBoxGhiChu.Text = row["Ghi chú"].ToString();
            }
        }
    }
}
