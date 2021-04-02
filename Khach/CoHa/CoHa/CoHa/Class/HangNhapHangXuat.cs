using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoHa
{
    class HangNhapHangXuat
    {
        private String MaHang;
        private String TenHang;
        private int SL;
        private String ThoiGian;

        public HangNhapHangXuat(String maHang, String tenHang, int sL, String thoiGian)
        {
            MaHang = maHang;
            TenHang = tenHang;
            SL = sL;
            ThoiGian = thoiGian;
        }

        public String getMaHang()
        {
            return MaHang;
        }

        public void setMaHang(String maHang)
        {
            MaHang = maHang;
        }

        public String getTenHang()
        {
            return TenHang;
        }

        public void setTenHang(String tenHang)
        {
            TenHang = tenHang;
        }

        public int getSL()
        {
            return SL;
        }

        public void setSL(int sL)
        {
            SL = sL;
        }

        public String getThoiGian()
        {
            return ThoiGian;
        }

        public void setThoiGian(String thoiGian)
        {
            ThoiGian = thoiGian;
        }

    }
}
