using ClosedXML.Excel;
using Microsoft.Win32;
using Server.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for StatistikaProductUC.xaml
    /// </summary>
    public partial class StatistikaProductUC : UserControl
    {
        public StatistikaProductUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            date1.SelectedDate = DateTime.Today;
            date2.SelectedDate = DateTime.Today;
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
                            worksheet.Cell(1, 1).Value = "Отчет о продукте от " + date1.SelectedDate + " до " + second + " дня";
                            worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 6)).Merge();
                            worksheet.Cell(2, 1).Value = "Штрих-код";
                            worksheet.Cell(2, 2).Value = "Товар";
                            worksheet.Cell(2, 3).Value = "Тип масса";
                            worksheet.Cell(2, 4).Value = "Тип прод.";
                            worksheet.Cell(2, 5).Value = "Кол-во";
                            worksheet.Cell(2, 6).Value = "Чистая прибыль";

                            int kk = 3;
                            foreach (InfoStatistika rv in DG.Items)
                            {
                                worksheet.Cell(kk, 1).Value = rv.Shtrix;
                                worksheet.Cell(kk, 2).Value = rv.Tovar;
                                worksheet.Cell(kk, 3).Value = rv.MassaName;
                                worksheet.Cell(kk, 4).Value = rv.TypeName;
                                worksheet.Cell(kk, 5).Value = rv.Miqdor;
                                worksheet.Cell(kk, 6).Value = rv.Foyda;
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
                MessageBox.Show("Error23");
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (date1.SelectedDate != null && date2.SelectedDate != null)
            {
                try
                {
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
                        var v = (from a in context.Solds
                                 where a.dateTimeNow >= date1.SelectedDate && a.dateTimeNow <= second
                                 group a by new { a.NameOfProduct, a.Shtrix, a.MassaName, a.TypeName } into g
                                 orderby g.Key.NameOfProduct
                                 select new InfoStatistika()
                                 {
                                     Shtrix = g.Key.Shtrix,
                                     Tovar = g.Key.NameOfProduct,
                                     MassaName = g.Key.MassaName,
                                     TypeName = g.Key.TypeName,
                                     Miqdor = g.Sum(x => x.CountProduct),
                                     Foyda = g.Sum(x => (x.SalePrice - x.BazaPrice )*x.CountProduct)
                                 }).ToList();
                        labSumma.Text = v.Sum(x => x.Foyda).ToString();
                        DG.ItemsSource = v;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error24");
                }
            }
            else
            {
                MessageBox.Show("Ошибка на даты!");
            }
        }
    }
}
