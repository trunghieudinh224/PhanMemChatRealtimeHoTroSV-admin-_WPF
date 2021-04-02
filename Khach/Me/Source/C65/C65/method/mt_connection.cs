using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C65.method
{
    class mt_connection
    {
        public string connectionSTR = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\database.mdb";
        public OleDbDataAdapter da;
        public OleDbConnection conn = new OleDbConnection();
        public void connection_project()
        {
            conn.ConnectionString = connectionSTR;
        }

        public int login(String taikhoan, String matkhau, String trangthai)
        {
            conn.Open();
            string query = "select count (Username) from TaiKhoan where Username = '" + taikhoan + "' and Password = '" + matkhau + "' and TrangThai = '" + trangthai + "'";
            OleDbCommand cmd = new OleDbCommand(query, conn);
            int result = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            conn.Close();
            return result;
        }
    }
}
