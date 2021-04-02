using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C65
{
    class SPKhachChon
    {
        private int DonGia;
        private int SoLuong;
        private int ThanhTien;

        public SPKhachChon( int soLuong, int donGia, int thanhTien)
        {
            SoLuong = soLuong;
            DonGia = donGia;
            ThanhTien = thanhTien;
        }

        public int getDonGia()
        {
            return DonGia;
        }

        public void setDonGia(int donGia)
        {
            DonGia = donGia;
        }

        public int getSoLuong()
        {
            return SoLuong;
        }

        public void setSoLuong(int soLuong)
        {
            SoLuong = soLuong;
        }

        public int getThanhTien()
        {
            return ThanhTien;
        }

        public void setThanhTien(int thanhTien)
        {
            ThanhTien = thanhTien;
        }
    }
}
