using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Server.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Server
{
    /// <summary>
    /// Interaction logic for PartiyaInsideUC.xaml
    /// </summary>
    public partial class PartiyaInsideUC : UserControl
    {
        public PartiyaInsideUC()
        {
            InitializeComponent();
        }

        private void DG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DG.SelectedCells.Count > 0 && DG.SelectedCells[0].IsValid)
                {
                    InfoPartiyaKirim t = DG.SelectedItem as InfoPartiyaKirim;
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        var v = (from a in context.Partiyas
                                .Include(x => x.Product)
                                .Include(x => x.Provider)
                                .Include(x => x.Product.Types)
                                .Include(x => x.Product.Massa)
                                 where a.Id == t.Id
                                 select a).FirstOrDefault();
                        if (v != null)
                        {
                            if (v.CountProduct != 0)
                            {
                                ChangeAddProductWindow window = new ChangeAddProductWindow(t.Id);
                                window.ShowDialog();
                                MySearch();
                            }
                            else
                            {
                                MessageBox.Show("Данного товар нет в наличии");
                            }
                        }
                    }

                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error11");
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MySearch();
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
            try
            {
                if (DG.Items.Count > 0)
                {
                    Stream myStream;
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    saveFileDialog1.Filter = "EXCELL FILE (*.xlsx)|*.xlsx";
                    saveFileDialog1.RestoreDirectory = true;
                    string filename;
                    if (saveFileDialog1.ShowDialog() == true)
                    {
                        if ((myStream = saveFileDialog1.OpenFile()) != null)
                        {
                            // Code to write the stream goes here.
                            filename = saveFileDialog1.FileName;
                            myStream.Close();

                            var workbook = new XLWorkbook();
                            IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");
                            worksheet.Columns().AdjustToContents();
                            worksheet.Cell(1, 1).Value = "Отчет приход продукта с " + date1.SelectedDate + " до " + second;
                            worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 10)).Merge();
                            worksheet.Cell(2, 1).Value = "Штрих-код";
                            worksheet.Cell(2, 2).Value = "Товар";
                            worksheet.Cell(2, 3).Value = "Тип";
                            worksheet.Cell(2, 4).Value = "Масса";
                            worksheet.Cell(2, 5).Value = "Цена зак.";
                            worksheet.Cell(2, 6).Value = "Цена про.";
                            worksheet.Cell(2, 7).Value = "Приход";
                            worksheet.Cell(2, 8).Value = "Остаток";
                            worksheet.Cell(2, 9).Value = "Поставщик";
                            worksheet.Cell(2, 10).Value = "Дата";

                            int kk = 3;
                            foreach (InfoPartiyaKirim rv in DG.Items)
                            {
                                worksheet.Cell(kk, 1).Value = rv.Shtrix.ToString();
                                worksheet.Cell(kk, 2).Value = rv.TovarNomi;
                                worksheet.Cell(kk, 3).Value = rv.TovarTuri;
                                worksheet.Cell(kk, 4).Value = rv.TovarMassaTuri;
                                worksheet.Cell(kk, 5).Value = rv.BazaviyNarxi;
                                worksheet.Cell(kk, 6).Value = rv.SotishNarxi;
                                worksheet.Cell(kk, 7).Value = rv.OlibKelingan;
                                worksheet.Cell(kk, 8).Value = rv.Qolgan;
                                worksheet.Cell(kk, 9).Value = rv.YetkazibBeruvchi;
                                worksheet.Cell(kk, 10).Value = rv.Chislo;
                                kk++;
                            }

                            worksheet.Columns("A", "Z").AdjustToContents();

                            workbook.SaveAs(filename);
                            Process cmd = new Process();
                            cmd.StartInfo.FileName = "cmd.exe";
                            cmd.StartInfo.RedirectStandardInput = true;
                            cmd.StartInfo.RedirectStandardOutput = true;
                            cmd.StartInfo.CreateNoWindow = true;
                            cmd.StartInfo.UseShellExecute = false;
                            cmd.Start();

                            cmd.StandardInput.WriteLine(filename);
                            cmd.StandardInput.Flush();
                            cmd.StandardInput.Close();
                            cmd.WaitForExit();
                            //  workbook.

                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error12");
            }

        }
        void MySearch()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
                    if (date1.SelectedDate.ToString() != "" && date2.SelectedDate.ToString() != "")
                    {
                        var v = (from a in context.Partiyas
                                     .Include(x => x.Product)
                                     .Include(x => x.Product.Types)
                                     .Include(x => x.Product.Massa)
                                     .Include(x => x.Provider)
                                 where a.TodayDate >= date1.SelectedDate && a.TodayDate <= second
                                 select new InfoPartiyaKirim()
                                 {
                                     Id = a.Id,
                                     Shtrix = a.Product.Shtrix,
                                     TovarNomi = a.Product.NameOfProduct,
                                     TovarTuri = a.Product.Types.TypeName,
                                     TovarMassaTuri = a.Product.Massa.Name,
                                     BazaviyNarxi = a.BazaPrice,
                                     SotishNarxi = a.SalePrice,
                                     OlibKelingan = a.CountProduct2,
                                     Qolgan = a.CountProduct,
                                     YetkazibBeruvchi = a.Provider.ProviderName,
                                     Chislo = a.TodayDate
                                 }).ToList();
                        v.OrderBy(x => x.Chislo);
                        DG.ItemsSource = v;

                    }
                    else
                    {
                        MessageBox.Show("Ошибка");

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error13");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            date1.SelectedDate = DateTime.Today;
            date2.SelectedDate = DateTime.Today;
        }
    }
}
