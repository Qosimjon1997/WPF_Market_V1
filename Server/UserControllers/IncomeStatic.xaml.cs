using Server.Data;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System;
using System.IO;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.Diagnostics;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for IncomeStatic.xaml
    /// </summary>
    public partial class IncomeStatic : UserControl
    {
        public IncomeStatic()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Workers
                         select a).ToList();
                combWorker.Items.Add("Все");
                combWorker.SelectedIndex = 0;
                foreach (Worker w in v)
                {
                    combWorker.Items.Add(w.FIO);
                }
            }
            date1.SelectedDate = DateTime.Today;
            date2.SelectedDate = DateTime.Today;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                if (combWorker.SelectedItem != null && date1.SelectedDate.ToString() != "" && date2.SelectedDate.ToString() != "")
                {
                    if (combWorker.SelectedItem.ToString() == "Все")
                    {
                        var v = (from a in context.Incomes
                                 where a.DateTimeNow >= date1.SelectedDate && a.DateTimeNow <= second
                                 select a).ToList();
                        DG_Income.ItemsSource = v;

                        decimal allSumm = 0;
                        decimal allSummWithSkidka = 0;
                        decimal cas = 0;
                        decimal plas = 0;
                        decimal qarzberish = 0;
                        decimal qarzqaytish = 0;
                        decimal vazvr = 0;
                        foreach (Income i in v)
                        {
                            allSumm += i.SaleProductPrice;
                            allSummWithSkidka += i.SaleProductWithDiscountPrice;
                            cas += i.CashIncome;
                            plas += i.PlasticIncome;
                            qarzberish += i.DebtOut;
                            qarzqaytish += i.DebtIncome;
                            vazvr += i.Vozvrat;
                        }
                        labAllSumma.Text = allSumm.ToString();
                        labAllSummaWithSkidka.Text = allSummWithSkidka.ToString();
                        labNaxt.Text = cas.ToString();
                        labPlastik.Text = plas.ToString();
                        labQarzga.Text = qarzberish.ToString();
                        labQarzdan.Text = qarzqaytish.ToString();
                        labVazvrat.Text = vazvr.ToString();
                    }
                    else
                    {
                        var v = (from a in context.Incomes
                                 .Include(x => x.Worker)
                                 where a.DateTimeNow >= date1.SelectedDate && a.DateTimeNow <= second && a.Worker.FIO == combWorker.SelectedItem.ToString()
                                 select a).ToList();
                        DG_Income.ItemsSource = v;
                        decimal allSumm = 0;
                        decimal allSummWithSkidka = 0;
                        decimal cas = 0;
                        decimal plas = 0;
                        decimal qarzberish = 0;
                        decimal qarzqaytish = 0;
                        decimal vazvr = 0;
                        foreach (Income i in v)
                        {
                            allSumm += i.SaleProductPrice;
                            allSummWithSkidka += i.SaleProductWithDiscountPrice;
                            cas += i.CashIncome;
                            plas += i.PlasticIncome;
                            qarzberish += i.DebtOut;
                            qarzqaytish += i.DebtIncome;
                            vazvr += i.Vozvrat;
                        }
                        labAllSumma.Text = allSumm.ToString();
                        labAllSummaWithSkidka.Text = allSummWithSkidka.ToString();
                        labNaxt.Text = cas.ToString();
                        labPlastik.Text = plas.ToString();
                        labQarzga.Text = qarzberish.ToString();
                        labQarzdan.Text = qarzqaytish.ToString();
                        labVazvrat.Text = vazvr.ToString();
                    }

                }
                else
                {
                    MessageBox.Show("Ошибка");

                }

            }
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
            try
            {
                if (DG_Income.Items.Count > 0)
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
                            worksheet.Cell(1, 1).Value = "Отчет рабочего " + combWorker.SelectedItem + " с " + date1.SelectedDate + " по " + second;
                            worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 6)).Merge();
                            worksheet.Cell(2, 1).Value = "Цена товара";
                            worksheet.Cell(2, 2).Value = "Цена товара со скидкой";
                            worksheet.Cell(2, 3).Value = "Наличными";
                            worksheet.Cell(2, 4).Value = "В пластику";
                            worksheet.Cell(2, 5).Value = "В долгу";
                            worksheet.Cell(2, 6).Value = "Из долга";
                            worksheet.Cell(2, 7).Value = "Возврат";
                            worksheet.Cell(2, 8).Value = "Дата";

                            int kk = 3;
                            foreach (Income rv in DG_Income.Items)
                            {
                                worksheet.Cell(kk, 1).Value = rv.SaleProductPrice;
                                worksheet.Cell(kk, 2).Value = rv.SaleProductWithDiscountPrice;
                                worksheet.Cell(kk, 3).Value = rv.CashIncome;
                                worksheet.Cell(kk, 4).Value = rv.PlasticIncome;
                                worksheet.Cell(kk, 5).Value = rv.DebtOut;
                                worksheet.Cell(kk, 6).Value = rv.DebtIncome;
                                worksheet.Cell(kk, 7).Value = rv.Vozvrat;
                                worksheet.Cell(kk, 8).Value = rv.DateTimeNow;
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
