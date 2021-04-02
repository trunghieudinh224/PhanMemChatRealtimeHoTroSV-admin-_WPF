using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace VanSon.Method
{
    class mt_export_pdf_excel_file
    {
        mt_sudungchung m = new mt_sudungchung();
        private iTextSharp.text.Font text_ghichu = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 12.5f, iTextSharp.text.Font.UNDERLINE);
        private iTextSharp.text.Font text = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 13f, iTextSharp.text.Font.NORMAL);
        private iTextSharp.text.Font text_tieude = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 18f, iTextSharp.text.Font.BOLD);
        private iTextSharp.text.Font text_tieude_table = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 13f, iTextSharp.text.Font.BOLD);

        #region PDF
        public void PDF_BaoCaoHangHoa(string filename, DataGridView dtg)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (!File.Exists(filename))
            {
                Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfdoc, new FileStream(filename, FileMode.Create));

                PdfPTable table_tieude = new PdfPTable(3);
                table_tieude.TotalWidth = 100f;
                float[] width_table_tieude = new float[] { 25f, 50f, 25f };
                System.Drawing.Image pic = System.Drawing.Image.FromFile(System.Windows.Forms.Application.StartupPath + "//logo_vanson.png");
                iTextSharp.text.Image iTextPic = iTextSharp.text.Image.GetInstance(pic, System.Drawing.Imaging.ImageFormat.Png);
                iTextPic.ScalePercent(3f);  
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_tieude, null, iTextPic, 0, BaseColor.WHITE);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_tieude, "BÁO CÁO TỒN KHO", null, 1, BaseColor.WHITE);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_ghichu, "Ngày tạo: " + date, null, 2, BaseColor.WHITE);

                Paragraph line_kc = new Paragraph("  ", text_tieude);

                PdfPTable BangHangHoa = new PdfPTable(dtg.Columns.Count);
                BangHangHoa.DefaultCell.Padding = 2;
                BangHangHoa.WidthPercentage = 100;
                BangHangHoa.HorizontalAlignment = Element.ALIGN_CENTER;
                BangHangHoa.DefaultCell.BorderWidth = 1;
                BangHangHoa.DefaultCell.BorderColor = iTextSharp.text.BaseColor.BLACK;
                BangHangHoa.TotalWidth = 100f;
                float[] widths = new float[] { 15f, 33f, 13f, 13f, 13f, 13f };
                BangHangHoa.SetWidths(widths);
                if (dtg.Rows.Count > 0)
                {
                    foreach (DataGridViewColumn column in dtg.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, text_tieude_table));

                        cell.HorizontalAlignment = 1;
                        cell.Padding = 2f;
                        cell.PaddingBottom = 5f;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);    

                        BangHangHoa.AddCell(cell);

                    }

                    foreach (DataGridViewRow row in dtg.Rows)
                    {
                        for (int i = 0; i < dtg.Columns.Count; i++)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(row.Cells[i].Value.ToString(), text));
                            cell.Padding = 2f;
                            cell.PaddingTop = 5f;
                            cell.PaddingBottom = 5f;
                            if (i == 0 || i == 1)
                            {
                                cell.HorizontalAlignment = 0;
                            }
                            else if (i == 2 || i == 4)
                            {
                                cell.HorizontalAlignment = 1;
                            }
                            else
                            {
                                cell.HorizontalAlignment = 2;
                            }
                            BangHangHoa.AddCell(cell);
                        }
                    }
                }

                Paragraph nguoilapphieu = new Paragraph("Người lập phiếu", text_tieude_table);
                nguoilapphieu.Alignment = 2;
                nguoilapphieu.IndentationRight = 10f;
                nguoilapphieu.SetLeading(6, 1);

                pdfdoc.Open();
                pdfdoc.Add(table_tieude);
                pdfdoc.Add(line_kc);
                pdfdoc.Add(BangHangHoa);
                pdfdoc.Add(nguoilapphieu);
                pdfdoc.Close();
            }
        }

        void pdf_cell(int rowspan, float totalwidth, float[] width_table, PdfPTable table, iTextSharp.text.Font text_tieude, String text, iTextSharp.text.Image img, int align, BaseColor color)
        {
            table.WidthPercentage = 100;
            table.TotalWidth = totalwidth;
            table.SetWidths(width_table);
            table.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell();
            if (img != null)
            {
                cell = new PdfPCell(img);
            }
            else cell = new PdfPCell(new Phrase(text, text_tieude));
            cell.BorderWidth = 1;
            cell.BorderColor = color;
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
            cell.Rowspan = rowspan;
            table.AddCell(cell);
        }

        public void PDF_BaoCaoHangNhap_Ngay(string filename, System.Data.DataTable dt)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (!File.Exists(filename))
            {
                Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfdoc, new FileStream(filename, FileMode.Create));

                PdfPTable table_tieude = new PdfPTable(3);
                table_tieude.TotalWidth = 100f;
                float[] width_table_tieude = new float[] { 25f, 50f, 25f };
                System.Drawing.Image pic = System.Drawing.Image.FromFile(System.Windows.Forms.Application.StartupPath + "//logo_vanson.png");
                iTextSharp.text.Image iTextPic = iTextSharp.text.Image.GetInstance(pic, System.Drawing.Imaging.ImageFormat.Png);
                iTextPic.ScalePercent(3f);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_tieude, null, iTextPic, 0, BaseColor.WHITE);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_tieude, "BÁO CÁO NHẬP HÀNG", null, 1, BaseColor.WHITE);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_ghichu, "Ngày tạo: " + date, null, 2, BaseColor.WHITE);

                Paragraph line_kc = new Paragraph("  ", text_tieude);

                PdfPTable BangThongTinKhach = new PdfPTable(dt.Columns.Count);
                BangThongTinKhach.DefaultCell.Padding = 2;
                BangThongTinKhach.WidthPercentage = 100;
                BangThongTinKhach.HorizontalAlignment = Element.ALIGN_CENTER;
                BangThongTinKhach.DefaultCell.BorderWidth = 1;
                BangThongTinKhach.DefaultCell.BorderColor = iTextSharp.text.BaseColor.BLACK;
                BangThongTinKhach.TotalWidth = 100f;
                float[] widths = new float[] { 17f, 15f, 32f, 15f, 19f, 14f };
                BangThongTinKhach.SetWidths(widths);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, text_tieude_table));

                        cell.HorizontalAlignment = 1;
                        cell.Padding = 2f;
                        cell.PaddingBottom = 5f;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);

                        BangThongTinKhach.AddCell(cell);

                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string value_convert = row.ItemArray[i].ToString();

                            if (i == 3 || i == 4)
                            {
                                int value = Convert.ToInt32(row.ItemArray[i].ToString());
                                value_convert = m.format_N2T(value);
                            }

                            PdfPCell cell = new PdfPCell(new Phrase(value_convert, text));
                            cell.Padding = 2f;
                            cell.PaddingTop = 5f;
                            cell.PaddingBottom = 5f;

                            if (i == 1 || i == 2)
                            {
                                cell.HorizontalAlignment = 0;
                            }
                            else if (i == 0 || i == 5)
                            {
                                cell.HorizontalAlignment = 1;
                            }
                            else if (i == 3 || i == 4)
                            {
                                cell.HorizontalAlignment = 2;
                            }
                            BangThongTinKhach.AddCell(cell);
                        }
                    }
                }
                Paragraph nguoilapphieu = new Paragraph("Người lập phiếu", text_tieude_table);
                nguoilapphieu.Alignment = 2;
                nguoilapphieu.IndentationRight = 10f;
                nguoilapphieu.SetLeading(6, 1);

                pdfdoc.Open();
                pdfdoc.Add(table_tieude);
                pdfdoc.Add(line_kc);
                pdfdoc.Add(BangThongTinKhach);
                pdfdoc.Add(nguoilapphieu);
                pdfdoc.Close();
            }
        }

        public void PDF_BaoCaoHangNhap_Thang(string filename, System.Data.DataTable dt, string thang)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (!File.Exists(filename))
            {
                Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfdoc, new FileStream(filename, FileMode.Create));

                PdfPTable table_tieude = new PdfPTable(3);
                table_tieude.TotalWidth = 100f;
                float[] width_table_tieude = new float[] { 25f, 50f, 25f };
                System.Drawing.Image pic = System.Drawing.Image.FromFile(System.Windows.Forms.Application.StartupPath + "//logo_vanson.png");
                iTextSharp.text.Image iTextPic = iTextSharp.text.Image.GetInstance(pic, System.Drawing.Imaging.ImageFormat.Png);
                iTextPic.ScalePercent(3f);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_tieude, null, iTextPic, 0, BaseColor.WHITE);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_tieude, "BÁO CÁO NHẬP HÀNG", null, 1, BaseColor.WHITE);
                pdf_cell(0, 100f, width_table_tieude, table_tieude, text_ghichu, "Ngày tạo: " + date, null, 2, BaseColor.WHITE);
                Paragraph thoigian = new Paragraph("(" + thang + ")", text);
                thoigian.Alignment = 1;
                Paragraph line_kc = new Paragraph("  ", text_tieude);

                PdfPTable BangThongTinKhach = new PdfPTable(dt.Columns.Count);
                BangThongTinKhach.DefaultCell.Padding = 2;
                BangThongTinKhach.WidthPercentage = 100;
                BangThongTinKhach.HorizontalAlignment = Element.ALIGN_CENTER;
                BangThongTinKhach.DefaultCell.BorderWidth = 1;
                BangThongTinKhach.DefaultCell.BorderColor = iTextSharp.text.BaseColor.BLACK;
                BangThongTinKhach.TotalWidth = 100f;
                float[] widths = new float[] { 17f, 15f, 32f, 15f, 19f, 14f };
                BangThongTinKhach.SetWidths(widths);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn column in dt.Columns)
                    {

                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, text_tieude_table));

                        cell.HorizontalAlignment = 1;
                        cell.Padding = 2f;
                        cell.PaddingBottom = 5f;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(204, 204, 204);

                        BangThongTinKhach.AddCell(cell);

                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string value_convert = row.ItemArray[i].ToString();

                            if (i == 3 || i == 4)
                            {
                                int value = Convert.ToInt32(row.ItemArray[i].ToString());
                                value_convert = m.format_N2T(value);
                            }

                            PdfPCell cell = new PdfPCell(new Phrase(value_convert, text));
                            cell.Padding = 2f;
                            cell.PaddingTop = 5f;
                            cell.PaddingBottom = 5f;

                            if (i == 1 || i == 2)
                            {
                                cell.HorizontalAlignment = 0;
                            }
                            else if (i == 0 || i == 5)
                            {
                                cell.HorizontalAlignment = 1;
                            }
                            else if (i == 3 || i == 4)
                            {
                                cell.HorizontalAlignment = 2;
                            }
                            BangThongTinKhach.AddCell(cell);
                        }
                    }
                }
                Paragraph nguoilapphieu = new Paragraph("Người lập phiếu", text_tieude_table);
                nguoilapphieu.Alignment = 2;
                nguoilapphieu.IndentationRight = 10f;
                nguoilapphieu.SetLeading(6, 1);

                pdfdoc.Open();
                pdfdoc.Add(table_tieude);
                pdfdoc.Add(thoigian);
                pdfdoc.Add(line_kc);
                pdfdoc.Add(BangThongTinKhach);
                pdfdoc.Add(nguoilapphieu);
                pdfdoc.Close();
            }
        }


        public void PDF_PhieuXuatHang(string filename, System.Data.DataTable dt_hang, string[] arr, bool money)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (!File.Exists(filename))
            {
                Document pdfdoc = new Document(PageSize.A4, 10, 10, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfdoc, new FileStream(filename, FileMode.Create));
                iTextSharp.text.Font text_px = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 12f, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font text_table_px = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 10f, iTextSharp.text.Font.NORMAL);
                iTextSharp.text.Font text_table_tieude_px = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 11f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font text_indam_px = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 12f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font text_tieude_px = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 15f, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font text_ghichu_px = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 12f, iTextSharp.text.Font.UNDERLINE | iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font kc_px = new iTextSharp.text.Font(BaseFont.CreateFont(System.Windows.Forms.Application.StartupPath + @"\vuArial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED), 4f, iTextSharp.text.Font.UNDERLINE);



                PdfPTable table_tieude = new PdfPTable(3);
                table_tieude.WidthPercentage = 100;
                table_tieude.TotalWidth = 100f;
                float[] width_table_tieude = new float[] { 25f, 50f, 25f };
                table_tieude.SetWidths(width_table_tieude);
                table_tieude.HorizontalAlignment = Element.ALIGN_CENTER;

                System.Drawing.Image pic = System.Drawing.Image.FromFile(System.Windows.Forms.Application.StartupPath + "//logo_vanson.png");
                iTextSharp.text.Image iTextPic = iTextSharp.text.Image.GetInstance(pic, System.Drawing.Imaging.ImageFormat.Png);
                iTextPic.ScalePercent(3.5f);
                pdf_cell(table_tieude, null, iTextPic, null, 0, 0, 0);

                PdfPTable table_center = new PdfPTable(1);
                pdf_cell(table_center, "PHIẾU ĐẶT HÀNG", null, text_tieude_px, 1, 0, 0);
                PdfPCell cell_center = new PdfPCell(table_center);
                cell_center.Padding = 0f;
                cell_center.BorderColor = BaseColor.WHITE;
                table_tieude.AddCell(cell_center);

                PdfPTable table_right = new PdfPTable(1);
                pdf_cell(table_right, "Mã phiếu: " + arr[0], null, text_ghichu_px, 2, 0, 5);
                pdf_cell(table_right, "Ngày: " + date, null, text_px, 2, 0, 0);
                PdfPCell cell_right = new PdfPCell(table_right);
                cell_right.Padding = 0f;
                cell_right.BorderColor = BaseColor.WHITE;
                table_tieude.AddCell(cell_right);



                PdfPTable BangThongTinKhach = new PdfPTable(2);
                BangThongTinKhach.WidthPercentage = 100;
                BangThongTinKhach.TotalWidth = 100f;
                float[] width_BangThongTinKhach = new float[] { 20f, 80f };
                BangThongTinKhach.SetWidths(width_BangThongTinKhach);
                BangThongTinKhach.HorizontalAlignment = Element.ALIGN_CENTER;

                pdf_cell(BangThongTinKhach, "Tên khách hàng:", null, text_ghichu_px, 0, 10, 4);
                pdf_cell(BangThongTinKhach, arr[1], null, text_px, 0, 10, 4);
                pdf_cell(BangThongTinKhach, "Địa chỉ khách:", null, text_ghichu_px, 0, 5, 4);
                pdf_cell(BangThongTinKhach, arr[2], null, text_px, 0, 5, 4);
                pdf_cell(BangThongTinKhach, "Số điện thoại:", null, text_ghichu_px, 0, 5, 4);
                pdf_cell(BangThongTinKhach, arr[3], null, text_px, 0, 5, 4);
                pdf_cell(BangThongTinKhach, "Diễn giải:", null, text_ghichu_px, 0, 5, 4);
                pdf_cell(BangThongTinKhach, arr[4], null, text_px, 0, 5, 4);
                pdf_cell(BangThongTinKhach, "Thanh toán:", null, text_ghichu_px, 0, 5, 4);
                pdf_cell(BangThongTinKhach, arr[5], null, text_px, 0, 5, 4);


                Paragraph line_kc = new Paragraph("  ", kc_px);

                System.Data.DataTable dt = new System.Data.DataTable();
                dt = dt_hang.Copy();
                dt.Columns.Add("ĐVT").SetOrdinal(3);
                dt.Columns.Remove("SL gửi");
                dt.Columns.Remove("SL giao");


                PdfPTable BangMuaHang = new PdfPTable(8);
                BangMuaHang.DefaultCell.Padding = 2;
                BangMuaHang.WidthPercentage = 100;
                BangMuaHang.HorizontalAlignment = Element.ALIGN_CENTER;
                BangMuaHang.DefaultCell.BorderWidth = 1;
                BangMuaHang.DefaultCell.BorderColor = iTextSharp.text.BaseColor.BLACK;
                BangMuaHang.TotalWidth = 100f;
                float[] widths = new float[] { 5f, 16f, 30f, 7f, 6f, 11f, 14f, 11f };
                BangMuaHang.SetWidths(widths);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, text_table_tieude_px));
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 187, 187);

                        cell.HorizontalAlignment = 1;
                        cell.Padding = 2f;
                        cell.PaddingBottom = 5f;

                        BangMuaHang.AddCell(cell);

                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string value_convert = "";
                            if (dt.Rows[i].ItemArray[j] == null)
                            {
                                value_convert = "";
                            }
                            value_convert = dt.Rows[i].ItemArray[j].ToString();

                            if (j == 4 || j == 5 || j == 6)
                            {
                                if (j == 5 && money == false)
                                {
                                    value_convert = "-";
                                }
                                else if (j == 6 && money == false)
                                {
                                    value_convert = "-";
                                }
                                else if (dt.Rows[i].ItemArray[j] == "")
                                {
                                    value_convert = "0";
                                }
                                else
                                {
                                    int value = Convert.ToInt32(m.format_T2N(dt.Rows[i].ItemArray[j].ToString()));
                                    value_convert = m.format_N2T(value);
                                }

                            }
                            else if (j == 3) value_convert = "Thùng";

                            PdfPCell cell = new PdfPCell(new Phrase(value_convert, text_table_px));
                            cell.Padding = 4f;
                            cell.PaddingBottom = 5f;
                            cell.VerticalAlignment = 1;

                            if (j == 1 || j == 2 || j == 7)
                            {
                                cell.HorizontalAlignment = 0;
                            }
                            else if (j == 0 || j == 3)
                            {
                                cell.HorizontalAlignment = 1;
                            }
                            else if (j == 4 || j == 5 || j == 6)
                            {
                                cell.HorizontalAlignment = 2;
                            }
                            BangMuaHang.AddCell(cell);
                        }
                    }
                }


                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j == 2)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase("TỔNG", text_table_tieude_px));
                        cell.HorizontalAlignment = 1;
                        cell.VerticalAlignment = 1;
                        cell.Padding = 4f;
                        cell.PaddingBottom = 5f;
                        BangMuaHang.AddCell(cell);
                    }
                    else if (j == 4)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(arr[6], text_table_tieude_px));
                        cell.HorizontalAlignment = 2;
                        cell.VerticalAlignment = 1;
                        cell.Padding = 4f;
                        cell.PaddingBottom = 5f;
                        BangMuaHang.AddCell(cell);
                    }
                    else if (j == 6)
                    {
                        PdfPCell cell;
                        if (money == false)
                        {
                            cell = new PdfPCell(new Phrase("-", text_table_tieude_px));
                            cell.HorizontalAlignment = 2;
                            cell.VerticalAlignment = 1;
                            cell.Padding = 4f;
                            cell.PaddingBottom = 5f;
                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(arr[7], text_table_tieude_px));
                            cell.HorizontalAlignment = 2;
                            cell.VerticalAlignment = 1;
                            cell.Padding = 4f;
                            cell.PaddingBottom = 5f;
                        }

                        BangMuaHang.AddCell(cell);
                    }
                    else
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(" ", text_table_tieude_px));
                        cell.HorizontalAlignment = 2;
                        cell.VerticalAlignment = 1;
                        cell.Padding = 4f;
                        cell.PaddingBottom = 5f;
                        BangMuaHang.AddCell(cell);
                    }
                }

                PdfPTable BangKyTen = new PdfPTable(5);
                BangKyTen.DefaultCell.Padding = 2;
                BangKyTen.WidthPercentage = 100;
                BangKyTen.HorizontalAlignment = Element.ALIGN_CENTER;
                BangKyTen.DefaultCell.BorderColor = iTextSharp.text.BaseColor.WHITE;
                BangKyTen.TotalWidth = 100f;
                float[] widths_BangKyTen = new float[] { 15f, 20f, 25f, 20f, 20f };
                BangKyTen.SetWidths(widths_BangKyTen);
                pdf_cell(BangKyTen, "Người nhận", null, text_px, 1, 5, 0);
                pdf_cell(BangKyTen, "Bảo vệ", null, text_px, 1, 5, 0);
                pdf_cell(BangKyTen, "Người vận chuyển", null, text_px, 1, 5, 0);
                pdf_cell(BangKyTen, "Thủ kho", null, text_px, 1, 5, 0);
                pdf_cell(BangKyTen, "Người nhập", null, text_px, 1, 5, 0);


                pdfdoc.Open();
                pdfdoc.Add(table_tieude);
                pdfdoc.Add(BangThongTinKhach);
                pdfdoc.Add(line_kc);
                pdfdoc.Add(BangMuaHang);
                pdfdoc.Add(line_kc);
                pdfdoc.Add(BangKyTen);
                pdfdoc.Close();
            }
        }

        void pdf_cell(PdfPTable table, string text, iTextSharp.text.Image img, iTextSharp.text.Font font, int align, int paddingtop, int paddingbottom)
        {
            PdfPCell cell = new PdfPCell();
            if (img != null)
            {
                cell = new PdfPCell(img);
            }
            else cell = new PdfPCell(new Phrase(text, font));
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
        #endregion PDF


        #region Excel
        public void Excel(System.Data.DataTable dt, string title, string ngay, int[] arr_size, object[] arr_position)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = "";
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(dialog.FileName))
                {
                    path = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("Vui lòng đặt tên khác, file đã tồn tại");
                    return;
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Đường dẫn k hợp lệ");
                return;
            }

            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Properties.Title = title + ngay;
                    p.Workbook.Worksheets.Add(title + ngay);
                    ExcelWorksheet ws = p.Workbook.Worksheets[0];

                    ws.Name = title + "\n" + ngay.Replace("/", "-");
                    ws.Cells.Style.Font.Size = 13;
                    ws.Cells.Style.Font.Name = "Times New Roman";


                    ws.Cells[1, 1, 2, dt.Columns.Count].Value = title + ngay;
                    ws.Cells[1, 1, 2, dt.Columns.Count].Merge = true;
                    ws.Cells[1, 1, 2, dt.Columns.Count].Style.Font.Bold = true;
                    ws.Cells[1, 1, 2, dt.Columns.Count].Style.Font.Size = 18;
                    ws.Cells[1, 1, 2, dt.Columns.Count].Style.Font.Color.SetColor(Color.Red);
                    ws.Cells[1, 1, 2, dt.Columns.Count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[1, 1, 2, dt.Columns.Count].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    var border_title = ws.Cells[1, 1, 2, dt.Columns.Count].Style.Border;
                    border_title.Bottom.Style = border_title.Top.Style = border_title.Right.Style = border_title.Left.Style = ExcelBorderStyle.Thick;
                    ws.Row(1).Height = 20;
                    ws.Row(3).Height = 27;
                    for (int i = 0; i < arr_size.Length; i++)
                    {
                        ws.Column(i+1).Width = arr_size[i];
                    }

                    for (int i = 4; i < dt.Rows.Count + 3; i++)
                    {
                        ws.Row(i).Height = 22;
                    }

                    int colIndex = 1;
                    int rowIndex = 3;

                    foreach (DataColumn item in dt.Columns)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 13;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                        var border = cell.Style.Border;
                        border.Bottom.Style = border.Top.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thick;

                        cell.Value = item.ColumnName;
                        colIndex++;
                    }


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        rowIndex++;
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            var border = ws.Cells[rowIndex, j + 1].Style.Border;
                            ws.Cells[rowIndex, j + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            border.Bottom.Style = border.Top.Style = border.Right.Style = border.Left.Style = ExcelBorderStyle.Thin;
                            ws.Cells[rowIndex, j + 1].Value = dt.Rows[i].ItemArray[j].ToString();

                            ws.Cells[rowIndex, j + 1].Style.HorizontalAlignment = (OfficeOpenXml.Style.ExcelHorizontalAlignment)arr_position[j];
                        }
                    }

                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(path, bin);
                }
                MessageBox.Show("xuất thành công");

            }
            catch (Exception e)
            {
                MessageBox.Show("Có lỗi khi lưu file");
            }
        }



        #endregion Excel
    }
}
