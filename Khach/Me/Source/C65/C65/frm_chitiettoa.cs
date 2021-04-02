using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Data.OleDb;
using C65.method;

namespace C65
{
    public partial class frm_chitiettoa : Form
    {
        mt_connection connection = new mt_connection();
        mt_insert mt_insert = new mt_insert();

        mt_sudungchung m = new mt_sudungchung();
        List<object> listmua;
        List<object> listtra;
        List<object> listkhachhang;
        List<object> listthongtintoa;
        string filename = string.Empty;
        string time = string.Empty;

        public frm_chitiettoa(List<object> list_mua, List<object> list_tra, List<object> list_khachhang, List<object> list_thongtintoa)
        {
            InitializeComponent();

            connection.connection_project();

            listmua = list_mua;
            listtra = list_tra;
            listkhachhang = list_khachhang;
            listthongtintoa = list_thongtintoa;

            Show_Toa(list_mua, listViewToaMua, labelToaMua);
            Show_Toa(list_tra, listViewToaTra, labelToaTra);
            Show_ThongTinToa();
        }

        private void buttonAn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //HÀM KHI RÊ CHUỘT VÀO NÚT ẨN
        private void buttonAn_MouseHover(object sender, EventArgs e)
        {
            buttonAn.BackColor = Color.LightCoral;
        }

        //HÀM KHI RÊ CHUỘT RỜI KHỎI NÚT ẨN
        private void buttonAn_MouseLeave(object sender, EventArgs e)
        {
            buttonAn.BackColor = Color.Firebrick;
        }

        //HÀM KHI CLICK NÚT X
        private void buttonX_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //HÀM KHI RÊ CHUỘT VÀO NÚT X
        private void buttonX_MouseHover(object sender, EventArgs e)
        {
            buttonX.BackColor = Color.LightCoral;
        }

        //HÀM KHI RÊ CHUỘT RỜI KHỎI NÚT X
        private void buttonX_MouseLeave(object sender, EventArgs e)
        {
            buttonX.BackColor = Color.Firebrick;
        }

        int mov;
        int movX;
        int movY;
        //HÀM DI CHUYỂN FORM
        private void panelThanhTieuDe_MouseUp(object sender, MouseEventArgs e)
        {
            mov = 0;
        }

        //HÀM DI CHUYỂN FORM
        private void panelThanhTieuDe_MouseMove(object sender, MouseEventArgs e)
        {
            if (mov == 1)
            {
                this.SetDesktopLocation(MousePosition.X - movX, MousePosition.Y - movY);
            }
        }

        //HÀM DI CHUYỂN FORM
        private void panelThanhTieuDe_MouseDown(object sender, MouseEventArgs e)
        {
            mov = 1;
            movX = e.X;
            movY = e.Y;
        }

        private void Show_Toa(List<object> list, ListView listview, Label label)
        {
            foreach (SPKhachChon item in list)
            {
                int dongia = item.getDonGia();
                int sl = item.getSoLuong();
                int thanhtien = item.getThanhTien();

                string[] sp = { "", m.format_N2T(sl), m.format_N2T(dongia), m.format_N2T(thanhtien) };
                var x = new ListViewItem(sp);
                listview.Items.Add(x);
            }

            int sl_sptoa = 0, money = 0;
            for (int i = 0; i < listview.Items.Count; i++)
            {
                sl_sptoa = sl_sptoa + m.format_T2N(listview.Items[i].SubItems[1].Text);
                money = money + m.format_T2N(listview.Items[i].SubItems[3].Text);
            }
            if (sl_sptoa > 0)
            {
                label.Text = m.format_N2T(sl_sptoa) + " SP - " + m.format_N2T(money) + " VNĐ";
            }
            else
            {
                label.Text = "0 SP - 0 VNĐ";
            }
        }





        private void accc(string tenkhach, string sdt, string diachi, string ghichu, string matoa, string ngaylaptoa, string giamgia, string notoacu, string tamtinh, string tongtoa, string khachdua, string tienthua)
        {
        }


        private void Show_ThongTinToa()
        {
          
            labelTenKhach.Text = listkhachhang[1].ToString();
            labelSDT.Text = listkhachhang[0].ToString();
            labelDiaChi.Text = listkhachhang[2].ToString();
            labelGhiChu.Text = listkhachhang[3].ToString();

            time = DateTime.Now.ToString("hh:MM:ss");
            labelMaToa.Text = DateTime.Now.ToString("ddMMyyyyhhmmss");
            labelNgayLap.Text = listthongtintoa[0].ToString();
            if (listthongtintoa[5].ToString() == "")
            {
                labelGiamGia.Text = "- 0";
            }else labelGiamGia.Text = "- " + listthongtintoa[5].ToString();
            if (listthongtintoa[9].ToString() == "")
            {
                labelNoToaCu.Text = "0";
            }
            else labelNoToaCu.Text = listthongtintoa[9].ToString();
            labelTamTinh.Text = m.format_N2T(m.format_T2N(listthongtintoa[6].ToString()) + m.format_T2N(listthongtintoa[5].ToString()) - m.format_T2N(listthongtintoa[9].ToString()));
            labelTongToa.Text = listthongtintoa[6].ToString();
            if (listthongtintoa[7].ToString() == "")
            {
                labelKhachDua.Text = "0";
            }
            else labelKhachDua.Text = listthongtintoa[7].ToString();
            if (m.format_T2N(listthongtintoa[8].ToString()) >= 0)
            {
                labelTienThua_Text.Text = "Tiền thừa:";
                if (listthongtintoa[8].ToString() == "")
                {
                    labelTienThua.Text = "0" ;
                }
                else
                { 
                    labelTienThua.Text = listthongtintoa[8].ToString();
                }
            }
            else
            {
                labelTienThua_Text.Text = "Khách nợ:";
                labelTienThua.Text = listthongtintoa[8].ToString();
            }
        }

        public void PDFToaHang()
        {
            filename = Application.StartupPath + "//Toa//" + labelMaToa.Text + ".pdf";
            if (!File.Exists(filename))
            {
                Document pdfdoc = new Document(PageSize.A7, 2, 2, -1f, -6f);
                PdfWriter writer = PdfWriter.GetInstance(pdfdoc, new FileStream(filename, FileMode.Create));
                BaseFont bf = BaseFont.CreateFont(Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                iTextSharp.text.Font text_tieude = new iTextSharp.text.Font(bf, 13.5f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font text = new iTextSharp.text.Font(bf, 12.5f, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font text_table = new iTextSharp.text.Font(bf, 14f, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font text_info = new iTextSharp.text.Font(bf, 12f, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font tensap = new iTextSharp.text.Font(bf, 15f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font total = new iTextSharp.text.Font(bf, 4f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font kc = new iTextSharp.text.Font(bf, 9.5f, iTextSharp.text.Font.BOLD);

                System.Drawing.Image pic = System.Drawing.Image.FromFile("D:\\Khach\\Me\\pic\\logo_toa.png");
                iTextSharp.text.Image iTextPic = iTextSharp.text.Image.GetInstance(pic, System.Drawing.Imaging.ImageFormat.Png);
                iTextPic.ScalePercent(21f);
                iTextPic.Alignment = 1;
                Paragraph C65 = new Paragraph("C65-66 LẦU 1", tensap);
                C65.Alignment = 1;
                Paragraph line1_c65 = new Paragraph("STK: 060072745445 Sacombank", text);
                Paragraph line2_c65 = new Paragraph("Chi Nhánh Bình Đăng, Quận 8", text);
                Paragraph line3_c65 = new Paragraph("Chủ TK: Từ Thị Tuyết Mai", text);
                Paragraph line4_c65 = new Paragraph("SĐT: 0903.972.674 (Mai)", text);
                Paragraph line5_c65 = new Paragraph("         0909.717.933 (Mai)", text);
                Paragraph line6_c65 = new Paragraph("- - - - - - - - - - - - - - - - - - - - - - - - - - -", text);
                line6_c65.SetLeading(12, 1);
                line6_c65.Alignment = 1;

                Paragraph hoadonbanhang = new Paragraph("HÓA ĐƠN BÁN HÀNG", text_tieude);
                hoadonbanhang.SetLeading(17, 1);
                hoadonbanhang.Alignment = 1;
                //PdfPTable table_time = new PdfPTable(2);
                //table_time.TotalWidth = 100f;
                //float[] width_table_time = new float[] { 65f, 35f };
                //pdf_cell(100f, width_table_time, table_time, text, "Ngày: " + labelNgayLap.Text, 0);
                //pdf_cell(100f, width_table_time, table_time, text_info, "Giờ: " + time, 2);

                Paragraph hd_dong1 = new Paragraph("Ngày: " + labelNgayLap.Text, text);
                Paragraph hd_dong2 = new Paragraph("Mã toa: " + labelMaToa.Text, text);
                Paragraph hd_dong3 = new Paragraph("SĐT: " + labelSDT.Text, text);
                Paragraph hd_dong4 = new Paragraph("Tên khách: " + labelTenKhach.Text, text);

                PdfPTable pdftbmua = new PdfPTable(listViewToaMua.Columns.Count);
                pdftbmua.DefaultCell.Padding = 2;
                pdftbmua.WidthPercentage = 100;
                pdftbmua.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftbmua.DefaultCell.BorderWidth = 0;
                pdftbmua.DefaultCell.BorderColor = iTextSharp.text.BaseColor.WHITE;
                pdftbmua.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                pdftbmua.TotalWidth = 100f;
                float[] widths = new float[] { 0f, 20f, 35f, 45f };
                pdftbmua.SetWidths(widths);
                if (listViewToaMua.Items.Count > 0)
                {
                    foreach (ColumnHeader column in listViewToaMua.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.Text, text_tieude));
                        cell.BorderWidth = 0;
                        cell.BorderWidthBottom = 1f;
                        cell.PaddingBottom = 7f;
                        cell.HorizontalAlignment = 1;

                        if (column.Text == "Đơn giá")
                        {
                            cell.HorizontalAlignment = 2;
                        }
                        else if (column.Text == "Thành tiền")
                        {
                            cell.HorizontalAlignment = 2;
                        }
                        else if (column.Text == "SL")
                        {
                            cell.HorizontalAlignment = 1;
                        }

                        pdftbmua.AddCell(cell);

                    }

                    foreach (ListViewItem row in listViewToaMua.Items)
                    {
                        for (int i = 0; i < listViewToaMua.Columns.Count; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(row.SubItems[i].Text, text_table));
                            cell.BorderWidth = 1;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.PaddingBottom = 4f;
                            if (i == 1)
                            {
                                cell.HorizontalAlignment = 1;
                            }
                            else if (i > 1)
                            {
                                cell.HorizontalAlignment = 2;
                            }
                            pdftbmua.AddCell(cell);
                        }
                    }
                }

                PdfPTable pdftbtra = new PdfPTable(listViewToaTra.Columns.Count);
                pdftbtra.DefaultCell.Padding = 2;
                pdftbtra.WidthPercentage = 100;
                pdftbtra.HorizontalAlignment = Element.ALIGN_CENTER;
                pdftbtra.DefaultCell.BorderWidth = 0;
                pdftbtra.DefaultCell.BorderColor = iTextSharp.text.BaseColor.WHITE;
                pdftbtra.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                pdftbtra.TotalWidth = 100f;
                pdftbtra.SetWidths(widths);
                if (listViewToaTra.Items.Count > 0)
                {
                    foreach (ColumnHeader column in listViewToaTra.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.Text, text_tieude));
                        cell.BorderWidth = 0;
                        cell.BorderWidthBottom = 1f;
                        cell.PaddingBottom = 7f;
                        cell.HorizontalAlignment = 1;

                        if (column.Text == "Đơn giá")
                        {
                            cell.HorizontalAlignment = 2;
                        }
                        else if (column.Text == "Thành tiền")
                        {
                            cell.HorizontalAlignment = 2;
                        }
                        else if (column.Text == "SL")
                        {
                            cell.HorizontalAlignment = 1;
                        }

                        pdftbtra.AddCell(cell);
                    }

                    foreach (ListViewItem row in listViewToaTra.Items)
                    {
                        for (int i = 0; i < listViewToaTra.Columns.Count; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(row.SubItems[i].Text, text_table));
                            cell.BorderWidth = 1;
                            cell.BorderColor = BaseColor.WHITE;
                            cell.PaddingBottom = 4f;
                            if (i == 1)
                            {
                                cell.HorizontalAlignment = 1;
                            }
                            else if (i > 1)
                            {
                                cell.HorizontalAlignment = 2;
                            }
                            pdftbtra.AddCell(cell);
                        }
                    }
                }
                Paragraph kc_dong = new Paragraph("  ", kc);
                Paragraph kc_bang = new Paragraph("  ", total);





                pdfdoc.Open();
                pdfdoc.Add(iTextPic);
                pdfdoc.Add(C65);
                pdfdoc.Add(line1_c65);
                pdfdoc.Add(line2_c65);
                pdfdoc.Add(line3_c65);
                pdfdoc.Add(line4_c65);
                pdfdoc.Add(line5_c65);
                pdfdoc.Add(line6_c65);
                pdfdoc.Add(hoadonbanhang);
                pdfdoc.Add(hd_dong1);
                //pdfdoc.Add(table_time);
                pdfdoc.Add(hd_dong2);
                pdfdoc.Add(hd_dong3);
                pdfdoc.Add(hd_dong4);

                if (listViewToaMua.Items.Count > 0)
                {
                    PdfPTable table_total = new PdfPTable(2);
                    table_total.TotalWidth = 100f;
                    float[] width_table_total = new float[] { 22f, 78f };
                    pdf_cell(100f, 4f, 4f, width_table_total, table_total, text_tieude, "MUA:", 0);
                    pdf_cell(100f, 4f, 4f, width_table_total, table_total, text, labelToaMua.Text.Replace(" VNĐ", "").Replace("SP", "(sp)"), 2);
                    pdfdoc.Add(kc_dong);
                    pdfdoc.Add(table_total);
                    pdfdoc.Add(pdftbmua);
                }
                if (listViewToaTra.Items.Count > 0)
                {
                    PdfPTable table_total = new PdfPTable(2);
                    table_total.TotalWidth = 100f;
                    float[] width_table_total = new float[] { 22f, 78f };
                    pdf_cell(100f, 0f, 4f, width_table_total, table_total, text_tieude, "TRẢ:", 0);
                    pdf_cell(100f, 0f, 4f, width_table_total, table_total, text, labelToaTra.Text.Replace(" VNĐ","").Replace("SP", "(sp)"), 2);
                    pdfdoc.Add(kc_dong);
                    pdfdoc.Add(kc_bang);
                    pdfdoc.Add(table_total);
                    pdfdoc.Add(pdftbtra);
                }


                Paragraph linethanhtoan = new Paragraph("- - - - - - - - - - - - - - - - - - - - - - - - - - -", text);
                linethanhtoan.Alignment = 1;
                linethanhtoan.SetLeading(6, 1);
                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 100f;
                float[] width_table = new float[] { 37f, 63f };
                pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, "Tạm tính:", 0);
                pdf_cell(100f, 0f, 5f, width_table, table, text_table, labelTamTinh.Text + " VNĐ", 2);
                pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, "Giảm giá:", 0);
                if (m.format_T2N(labelGiamGia.Text) != 0)
                {
                    pdf_cell(100f, 0f, 5f, width_table, table, text_table, labelGiamGia.Text + " VNĐ", 2);
                }
                else pdf_cell(100f, 0f, 5f, width_table, table, text_table, "0 VNĐ", 2);
                pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, "Nợ toa cũ:", 0);
                pdf_cell(100f, 0f, 5f, width_table, table, text_table, labelNoToaCu.Text + " VNĐ", 2);
                pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, "Tổng toa:", 0);
                pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, labelTongToa.Text + " VNĐ", 2);
                pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, "Khách đưa:", 0);
                pdf_cell(100f, 0f, 5f, width_table, table, text_table, labelKhachDua.Text + " VNĐ", 2);
                string trangthai = "";
                if (m.format_T2N(labelTienThua.Text) >= 0)
                {
                    pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, "Tiền thừa:", 0);
                    if (labelTienThua.Text == "")
                    {
                        pdf_cell(100f, 0f, 5f, width_table, table, text_table, "0 VNĐ", 2);
                    }
                    else pdf_cell(100f, 0f, 5f, width_table, table, text_table, labelTienThua.Text + " VNĐ", 2);
                }
                else
                {
                    pdf_cell(100f, 0f, 5f, width_table, table, text_tieude, "Nợ:", 0);
                    pdf_cell(100f, 0f, 5f, width_table, table, text_table, labelTienThua.Text + " VNĐ", 2);
                }

                Paragraph line_camon = new Paragraph("- - - - - - - - - - - - - - - - - - - - - - - - - - -", text);
                line_camon.Alignment = 1;
                line_camon.SetLeading(6, 1);
                Paragraph camon = new Paragraph("Cám ơn quý khách đã ủng hộ", text);
                camon.Alignment = 1;
                Paragraph thankyou = new Paragraph("THANK YOU", text);
                thankyou.Alignment = 1;
                camon.SetLeading(6, 1);

                pdfdoc.Add(linethanhtoan);
                pdfdoc.Add(kc_dong);
                pdfdoc.Add(table);
                pdfdoc.Add(line_camon);
                pdfdoc.Add(camon);
                pdfdoc.Add(thankyou);

                pdfdoc.Close();
            }
            //SendToDirectPrint(filename);
            //System.Diagnostics.Process.Start(filename);
        }

        void pdf_cell(float totalwidth, float paddingtop, float paddingbottom, float[] width_table, PdfPTable table, iTextSharp.text.Font text_tieude, String text, int align)
        {
            table.WidthPercentage = 100;
            table.TotalWidth = totalwidth;
            table.SetWidths(width_table);
            table.HorizontalAlignment = Element.ALIGN_BOTTOM;

            PdfPCell cell = new PdfPCell(new Phrase(text, text_tieude));
            cell.BorderWidth = 1;
            cell.BorderColor = BaseColor.WHITE;
            cell.PaddingTop = paddingtop;
            cell.PaddingBottom = paddingbottom;
            if (align == 0)
            {
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
            }
            else if (align == 1)
            {
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
            }
            else 
            {
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            }
            table.AddCell(cell);
        }

        private void SendToDirectPrint(string filename)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = filename;
            info.CreateNoWindow = true;

            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            //if (p.HasExited == false)
            //{
            //    p.WaitForExit(10000);
            //}

            //p.WaitForInputIdle();
            //if (false == p.CloseMainWindow())
            //    p.Kill();
        }

        private void buttonLuuToa_Click(object sender, EventArgs e)
        {
            buttonAn.Visible = buttonX.Visible = false;
            int nokhach = 0, khachno = 0;
            string trangthai = "Hoàn thành";

            if (labelTienThua_Text.Text == "Khách nợ")
            {
                khachno = m.format_T2N(labelTienThua.Text);
                trangthai = "Khách nợ";
            }
            else if(labelTienThua_Text.Text == "Nợ khách")
            {
                trangthai = "Nợ khách";
            }

            bool result = mt_insert.insert_toa(labelMaToa.Text, labelNgayLap.Text + " " + time, labelTenKhach.Text, labelSDT.Text, labelDiaChi.Text, labelGhiChu.Text, m.format_T2N(labelTongToa.Text), m.format_T2N(labelGiamGia.Text), m.format_T2N(labelKhachDua.Text), m.format_T2N(labelTienThua.Text), khachno, nokhach, trangthai);
            if (result == false)
            {
                MessageBox.Show("Lưu toa không thành công !!!", "Thông báo");
            }
            else
            {
                buttonLuuToa.Visible = false;
                mt_insert.insert_chitiettoa(listViewToaMua, labelMaToa.Text, false);
                mt_insert.insert_chitiettoa(listViewToaTra, labelMaToa.Text, true);
            }
        }

        private void buttonIn_Click(object sender, EventArgs e)
        {
            PDFToaHang();
        }

        private void buttonKetToa_Click(object sender, EventArgs e)
        {
            frm_banhang.reset = true;
            this.Close();
        }


    }
}
