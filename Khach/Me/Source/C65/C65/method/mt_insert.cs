using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C65.method
{
    class mt_insert
    {
        mt_connection connection = new mt_connection();
        mt_sudungchung m = new mt_sudungchung();
        
        public bool insert_toa(string MaToa, string ThoiGianLapToa, string TenKhach, string SDTKhach, string DiaChi, string GhiChu, int TongToa, int GiamGia, int KhachDua, int TienThua, int KhachNo, int NoKhach, string TrangThai)
        {
            connection.connection_project();
            try
            {
                connection.conn.Open();
                string query = "insert into Toa (MaToa, ThoiGianLapToa, TenKhach, SDTKhach, DiaChi, GhiChu, TongToa, GiamGia, KhachDua, TienThua, KhachNo, NoKhach, TrangThai) values ('" + MaToa + "', '" + ThoiGianLapToa + "', '" + TenKhach + "', '" + SDTKhach + "', '" + DiaChi + "', '" + GhiChu + "', " + TongToa + ", " + GiamGia + ", " + KhachDua + ", " + TienThua + ", " + KhachNo + ", " + NoKhach + ", '" + TrangThai + "')";
                OleDbCommand cmd = new OleDbCommand(query, connection.conn);
                cmd.ExecuteNonQuery();
                connection.conn.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void insert_chitiettoa(ListView listview, string MaToa, bool HangTra)
        {
            connection.connection_project();
            for (int i = 0; i < listview.Items.Count; i++)
            {
                try
                {
                    connection.conn.Open();
                    string query = "insert into ChiTietToa (MaToa, SL, DonGia, ThanhTien, HangTra) values ('" + MaToa + "', " + m.format_T2N(listview.Items[i].SubItems[1].Text) + ", " + m.format_T2N(listview.Items[i].SubItems[2].Text) + ", " + m.format_T2N(listview.Items[i].SubItems[3].Text) + ", " + HangTra + ")";
                    OleDbCommand cmd = new OleDbCommand(query, connection.conn);
                    cmd.ExecuteNonQuery();
                    connection.conn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            
        }
    }
}
