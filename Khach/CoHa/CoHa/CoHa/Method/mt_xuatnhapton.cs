using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanSon.Connection;

namespace VanSon.Method
{
    class mt_xuatnhapton
    {
        connection_database mt_conn = new connection_database();
        mt_sudungchung m = new mt_sudungchung();

        public bool update_SoLuongTon_KhaDung_HangHoa(string MaHang, string TenHang, int sl, bool add)
        {
            mt_conn.connection_project();
            string query = ""; bool return_value = true;
            if (add == true) query = "update HangHoa set SoLuongTon = SoLuongTon + " + sl + ", KhaDung = KhaDung + " + sl + " where Xoa = false and MaHang = '" + MaHang + "' and TenHang = '" + TenHang + "'";
            else query = "update HangHoa set SoLuongTon = SoLuongTon - " + sl + ", KhaDung = KhaDung - " + sl + " where Xoa = false and MaHang = '" + MaHang + "' and TenHang = '" + TenHang + "'";

            try
            {
                mt_conn.conn.Open();
                OleDbCommand cmdget = new OleDbCommand(query, mt_conn.conn);
                cmdget.ExecuteNonQuery();
                mt_conn.conn.Close();
            }
            catch (Exception e)
            {
                return_value = false;
            }
            return return_value;
        }


        public int check_TonDau(string thang)
        {
            mt_conn.connection_project();
            mt_conn.conn.Open();
            string querycheck_exist = "select count(*) from XuatNhapTon where ThoiGian = #"+thang+"#";
            OleDbCommand cmd = new OleDbCommand(querycheck_exist, mt_conn.conn);
            int value = Convert.ToInt32(cmd.ExecuteScalar());
            mt_conn.conn.Close();
            return value;
        }

        public bool creat_XNT(string thang)
        {
            mt_conn.connection_project();
            bool value = true;

            mt_conn.conn.Open();
            try
            {
                string tenfile = "XUẤT NHẬP TỒN THÁNG " + thang;
                string query = "insert into XuatNhapTon (TenFile, ThoiGian, NgayTao) values ('" + tenfile + "', '" + thang + "', '" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "')";
                OleDbCommand cmd = new OleDbCommand(query, mt_conn.conn);
                cmd.ExecuteNonQuery();
                value = true;
            }
            catch (Exception e)
            {
                value = false;
            }
            mt_conn.conn.Close();
            return value;
        }

        public DataTable get_ChiTietXNT_thangmoi(string thanghientai)
        {
            mt_conn.connection_project();
            DateTime thang_truoc = Convert.ToDateTime(thanghientai).AddMonths(-1);
            DataTable dt = new DataTable();
            //string query = "select (ChiTietXuatNhapTon.TonCK - HangGui.SoLuongGui) as SL  from ChiTietXuatNhapTon, HangGui where HangGui.Xoa = ChiTietXuatNhapTon.Xoa and HangGui.MaHang = ChiTietXuatNhapTon.MaHang";
            //string query = "SELECT ChiTietXuatNhapTon.MaHang, ChiTietXuatNhapTon.TenHang, ChiTietXuatNhapTon.TonCK , m.SoLuongNhap, SUM(m.SoLuongGui), (ChiTietXuatNhapTon.TonCK - SUM(m.SoLuongGui)) as SLhientai FROM (SELECT HangGui.MaHang, HangGui.TenHang, SoLuongGui, TonCK, SoLuongNhap FROM HangNhap, HangGui, ChiTietXuatNhapTon where Month(ThoiGian = 02) and  ChiTietXuatNhapTon.MaHang = HangGui.MaHang = HangNhap.MaHang and ChiTietXuatNhapTon.TenHang = HangGui.TenHang = HangNhap.TenHang ) AS m INNER JOIN ChiTietXuatNhapTon ON  m.MaHang = ChiTietXuatNhapTon.MaHang and m.TenHang = ChiTietXuatNhapTon.TenHang group by ChiTietXuatNhapTon.TenHang, ChiTietXuatNhapTon.MaHang, ChiTietXuatNhapTon.TonCK ORDER BY ChiTietXuatNhapTon.TenHang asc";
            //string query = "SELECT ChiTietXuatNhapTon.MaHang, ChiTietXuatNhapTon.TenHang, SoLuongNhap,  SoLuongGui FROM HangNhap, HangGui, ChiTietXuatNhapTon where ChiTietXuatNhapTon.TenHang = HangNhap.TenHang and ChiTietXuatNhapTon.TenHang = HangGui.TenHang group by ChiTietXuatNhapTon.TenHang, ChiTietXuatNhapTon.MaHang, SoLuongNhap, SoLuongGui";

            mt_conn.conn.Open();
            string query_first_month = "SELECT * from ChiTietXuatNhapTon where ThoiGian = #" + thang_truoc.ToString("MM/yyyy") + "#";
            mt_conn.da = new OleDbDataAdapter(query_first_month, mt_conn.conn);
            mt_conn.da.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                dt.Reset();
                string query = "SELECT null as [STT], MaHang as [Mã hàng], TenHang as [Tên hàng], KhaDung as [Tồn ĐK], 0 as [Nhập], 0 as [Xuất], 0 as [Gửi], KhaDung as [Tồn CK], KhaDung as [Khả dụng] from ChiTietXuatNhapTon where ThoiGian = #" + thang_truoc.ToString("MM/yyyy") + "#";
                mt_conn.da = new OleDbDataAdapter(query, mt_conn.conn);
                mt_conn.da.Fill(dt);
            }
            else
            {
                dt.Reset();
                string query = "SELECT null as [STT], MaHang as [Mã hàng], TenHang as [Tên hàng], SoLuongTon as [Tồn ĐK], 0 as [Nhập], 0 as [Xuất], 0 as [Gửi], SoLuongTon as [Tồn CK], SoLuongTon as [Khả dụng] from HangHoa";
                mt_conn.da = new OleDbDataAdapter(query, mt_conn.conn);
                mt_conn.da.Fill(dt);
            }

            mt_conn.conn.Close();
            return dt;
        }


        public void create_ChiTietXNT(string thang, DataTable dt)
        {
            mt_conn.connection_project();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                mt_conn.conn.Open();
                string query = "insert into ChiTietXuatNhapTon (ThoiGian, MaHang, TenHang, TonDK, TonCK, KhaDung) values ('" + thang + "', '"+dt.Rows[i].ItemArray[1].ToString()+ "', '" + dt.Rows[i].ItemArray[2].ToString() + "', " + Convert.ToInt32(dt.Rows[i].ItemArray[3].ToString()) + ", " + Convert.ToInt32(dt.Rows[i].ItemArray[7].ToString()) + ", " + Convert.ToInt32(dt.Rows[i].ItemArray[8].ToString()) + ")";
                OleDbCommand cmd = new OleDbCommand(query, mt_conn.conn);
                cmd.ExecuteNonQuery();
                mt_conn.conn.Close();
            }
        }


        public DataTable showData(string thang)
        {
            mt_conn.connection_project();
            mt_conn.conn.Open();
            string query = "select 0 as STT, MaHang as [Mã hàng], TenHang as [Tên hàng], TonDK as [Tồn DK], 0 as [Nhập], 0 as [Xuất], 0 as [Gửi], TonCK as [Tồn CK], KhaDung as [Khả dụng] from ChiTietXuatNhapTon where ThoiGian = #" + thang + "#";
            mt_conn.da = new OleDbDataAdapter(query, mt_conn.conn);
            DataTable dt = new DataTable();
            mt_conn.da.Fill(dt);
            m.update_STT(dt);
            mt_conn.conn.Close();

            string[] arr = { "Nhap", "Xuat", "Gui" };
            for (int j = 0; j < arr.Length; j++)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][j+4] = get_Nhap_Xuat_Gui(arr[j], thang, dt.Rows[i].ItemArray[2].ToString());
                    dt.Rows[i][7] = Convert.ToInt32(dt.Rows[i].ItemArray[3].ToString()) + Convert.ToInt32(dt.Rows[i].ItemArray[4].ToString()) - Convert.ToInt32(dt.Rows[i].ItemArray[5].ToString()) + Convert.ToInt32(dt.Rows[i].ItemArray[6].ToString());
                    dt.Rows[i][8] = Convert.ToInt32(dt.Rows[i].ItemArray[7].ToString()) - Convert.ToInt32(dt.Rows[i].ItemArray[6].ToString());
                }
            }
            return dt;
        }


        public int get_Nhap_Xuat_Gui(string nxg, string thang, string tenhang)
        {
            int value_get = 0;
            mt_conn.connection_project();
            mt_conn.conn.Open();
            string query_get = "select sum(SoLuong"+ nxg + ") from Hang" + nxg + " where  Xoa = false and TenHang = '" + tenhang+ "' and Month(ThoiGian" + nxg + ") = " + thang.Substring(0,2)+"";
            OleDbCommand cmd_get = new OleDbCommand(query_get, mt_conn.conn);
            var obj_rs = cmd_get.ExecuteScalar();
            if (obj_rs.ToString() == "") value_get = 0; else value_get = Convert.ToInt32(cmd_get.ExecuteScalar());
            mt_conn.conn.Close();

            return value_get;
        }
    }
}
