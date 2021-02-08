using ClosedXML.Excel;
using Microsoft.Win32;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for SrokUtganUC.xaml
    /// </summary>
    public partial class SrokUtganUC : UserControl
    {
        public SrokUtganUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            date1.SelectedDate = DateTime.Today;
            date2.SelectedDate = DateTime.Today;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.InvalidGoods
                             where date1.SelectedDate <= a.Sana && a.Sana <= second
                             select a).ToList();

                    var SUMMA = (from a in context.InvalidGoods
                                 where date1.SelectedDate <= a.Sana && a.Sana <= second
                                 select a.BuyPrice).Sum();
                    labSumma.Text = SUMMA.ToString();

                    DG.ItemsSource = v;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
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
                            worksheet.Cell(1, 1).Value = "Отчет с " + date1.SelectedDate + " по " + second + "по просроченные товаровы";
                            worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 6)).Merge();
                            worksheet.Cell(2, 1).Value = "Штрих-код";
                            worksheet.Cell(2, 2).Value = "Товар";
                            worksheet.Cell(2, 3).Value = "Тип";
                            worksheet.Cell(2, 4).Value = "Масс.тип";
                            worksheet.Cell(2, 5).Value = "Цена покуплен";
                            worksheet.Cell(2, 6).Value = "кг/шт.";
                            worksheet.Cell(2, 7).Value = "Дате";

                            int kk = 3;
                            foreach (InvalidGoods rv in DG.Items)
                            {
                                worksheet.Cell(kk, 1).Value = rv.Shtrix;
                                worksheet.Cell(kk, 2).Value = rv.NameOfProduct;
                                worksheet.Cell(kk, 3).Value = rv.TypeName;
                                worksheet.Cell(kk, 4).Value = rv.MassaName;
                                worksheet.Cell(kk, 5).Value = rv.BuyPrice;
                                worksheet.Cell(kk, 6).Value = rv.CountProduct;
                                worksheet.Cell(kk, 7).Value = rv.Sana;
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
                MessageBox.Show(err.Message);
            }
        }
    }
}
