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

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        void Refresh()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Debts
                             .Include(x => x.DebtInfo)
                             group a by new
                             {
                                 a.DebtInfoId,
                                 a.DebtInfo.FIO,
                                 a.DebtInfo.Address,
                                 a.DebtInfo.Phone,
                             } into gcs
                             select new InfoDebt()
                             {
                                 Id = gcs.Key.DebtInfoId,
                                 FIO = gcs.Key.FIO,
                                 Address = gcs.Key.Address,
                                 Phone = gcs.Key.Phone,
                                 Price = gcs.Sum(x => x.Price)
                             }).ToList();
                    DG.ItemsSource = v;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error4");
            }
        }
        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
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
                            worksheet.Cell(1, 1).Value = "Отчет должники";
                            worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 6)).Merge();
                            worksheet.Cell(2, 1).Value = "Ф.И.О";
                            worksheet.Cell(2, 2).Value = "Адрес";
                            worksheet.Cell(2, 3).Value = "Тел";
                            worksheet.Cell(2, 4).Value = "Сумма";

                            int kk = 3;
                            foreach (InfoDebt rv in DG.Items)
                            {
                                worksheet.Cell(kk, 1).Value = rv.FIO;
                                worksheet.Cell(kk, 2).Value = rv.Address;
                                worksheet.Cell(kk, 3).Value = rv.Phone;
                                worksheet.Cell(kk, 4).Value = rv.Price;
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
                MessageBox.Show("Error5");
            }
        }

        private void DG_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (DG.SelectedCells.Count > 0 && DG.SelectedCells[0].IsValid)
                {
                    InfoDebt t = DG.SelectedItem as InfoDebt;
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        Debt v = (from a in context.Debts
                                     .Include(p => p.DebtInfo)
                                  where a.Id == t.Id
                                  select a).FirstOrDefault();

                        if (v != null)
                        {
                            HistoryDebitorsWindow window = new HistoryDebitorsWindow(v.DebtInfoId);
                            window.ShowDialog();
                            Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Этот должник не найден в базе");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error23");
            }
        }
    }
}
