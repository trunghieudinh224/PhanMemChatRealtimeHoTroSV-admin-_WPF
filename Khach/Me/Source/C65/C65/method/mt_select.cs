using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C65.method
{
    class mt_select
    {
        mt_connection connection = new mt_connection();
        public DataTable HienThiToa(string MaToa, string TenKhach, int TongToa, string TuNgay, string DenNgay, string TrangThai)
        {
            string dieukien_tongtoa = "", dieukien_ngay = "";
            if (TongToa != 0) dieukien_tongtoa = "and TongToa like ('%" + TongToa + "%')";
            if (TuNgay != null && DenNgay != null) dieukien_ngay = "and (ThoiGianLapToa >= #" + TuNgay + "# and ThoiGianLapToa <= #" + DenNgay + " 23:59:59#)";

            connection.connection_project();
            connection.conn.Open();
            string query = "select MaToa,ThoiGianLapToa, TenKhach, TongToa, TrangThai from Toa where (MaToa like ('%" + MaToa + "%') and TenKhach like ('%" + TenKhach + "%') " + dieukien_tongtoa + ") " + dieukien_ngay + " and TrangThai like ('%" + TrangThai + "%') order by ThoiGianLapToa desc";
            connection.da = new OleDbDataAdapter(query, connection.conn);
            connection.conn.Close();
            DataTable result = new DataTable();
            connection.da.Fill(result);
            return result;
        }

    }
}
