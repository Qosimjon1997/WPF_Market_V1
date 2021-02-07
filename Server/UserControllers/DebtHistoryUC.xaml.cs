using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for DebtHistoryUC.xaml
    /// </summary>
    public partial class DebtHistoryUC : UserControl
    {
        public DebtHistoryUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            date1.SelectedDate = DateTime.Today;
            date2.SelectedDate = DateTime.Today;
            combTypeDebt.Items.Add("Все");
            combTypeDebt.Items.Add("Оплачено");
            combTypeDebt.Items.Add("Не оплачено");

            combTypeDebt.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                if (combTypeDebt.SelectedIndex == 0)
                {
                    var v = (from a in context.Debts
                         .Include(x => x.DebtInfo)
                             where date1.SelectedDate <= a.dateTimeFrom && a.dateTimeFrom <= second
                             select new InfoDebt()
                             {
                                 Id = a.Id,
                                 FIO = a.DebtInfo.FIO,
                                 Address = a.DebtInfo.Address,
                                 Phone = a.DebtInfo.Phone,
                                 dateTimeFrom = a.dateTimeFrom,
                                 dateTimeLastPay = a.dateTimePay,
                                 dateTimeUntil = a.dateTimeUntil,
                                 Price = a.Price
                             }).ToList();
                    DG.ItemsSource = v;
                }
                else if (combTypeDebt.SelectedIndex == 1)
                {
                    var v = (from a in context.Debts
                         .Include(x => x.DebtInfo)
                             where date1.SelectedDate <= a.dateTimeFrom && a.dateTimeFrom <= second && a.Price == 0
                             select new InfoDebt()
                             {
                                 Id = a.Id,
                                 FIO = a.DebtInfo.FIO,
                                 Address = a.DebtInfo.Address,
                                 Phone = a.DebtInfo.Phone,
                                 dateTimeFrom = a.dateTimeFrom,
                                 dateTimeLastPay = a.dateTimePay,
                                 dateTimeUntil = a.dateTimeUntil,
                                 Price = a.Price
                             }).ToList();
                    DG.ItemsSource = v;
                }
                else
                {
                    var v = (from a in context.Debts
                         .Include(x => x.DebtInfo)
                             where date1.SelectedDate <= a.dateTimeFrom && a.dateTimeFrom <= second && a.Price != 0
                             select new InfoDebt()
                             {
                                 Id = a.Id,
                                 FIO = a.DebtInfo.FIO,
                                 Address = a.DebtInfo.Address,
                                 Phone = a.DebtInfo.Phone,
                                 dateTimeFrom = a.dateTimeFrom,
                                 dateTimeLastPay = a.dateTimePay,
                                 dateTimeUntil = a.dateTimeUntil,
                                 Price = a.Price
                             }).ToList();
                    DG.ItemsSource = v;
                }

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
                            worksheet.Cell(1, 1).Value = "Отчет с " + date1.SelectedDate + " по " + second + "по видам задолженности " + combTypeDebt.SelectedItem;
                            worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 6)).Merge();
                            worksheet.Cell(2, 1).Value = "Ф.И.О";
                            worksheet.Cell(2, 2).Value = "Адрес";
                            worksheet.Cell(2, 3).Value = "Тел";
                            worksheet.Cell(2, 4).Value = "С";
                            worksheet.Cell(2, 5).Value = "До";
                            worksheet.Cell(2, 6).Value = "Последний платеж";
                            worksheet.Cell(2, 7).Value = "Сумма";

                            int kk = 3;
                            foreach (InfoDebt rv in DG.Items)
                            {
                                worksheet.Cell(kk, 1).Value = rv.FIO;
                                worksheet.Cell(kk, 2).Value = rv.Address;
                                worksheet.Cell(kk, 3).Value = rv.Phone;
                                worksheet.Cell(kk, 4).Value = rv.dateTimeFrom;
                                worksheet.Cell(kk, 5).Value = rv.dateTimeUntil;
                                worksheet.Cell(kk, 6).Value = rv.dateTimeLastPay;
                                worksheet.Cell(kk, 7).Value = rv.Price;
                                kk++;
                            }

                            worksheet.Columns("A", "Z").AdjustToContents();
                            //  string sql = "SELECT Dorilars.Id as id,Partiyas.Id as t_id,  TovarNomi as Nomi, IshlabChiqaruvchi as Ishlab_chiqaruvchi,Nomi as Turi,Doza,(BazaviyNarx) as Olish_Narxi,SotiladiganNarx as Sotiladigan_Narx,NechtaKeldiDona/PachkadaNechta as Necha_Pachka,NechtaKeldiDona%PachkadaNechta as Necha_Dona,PachkadaNechta as Pachkada_soni, Shtrix as Shtrix_Kod, CONVERT(varchar, KelganSana, 3) as kelgan_sana From Dorilars,Partiyas,DorilarTuris where Dorilars.DorilarTuriId=DorilarTuris.Id and Dorilars.id=Partiyas.DorilarId;";

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
