using Microsoft.EntityFrameworkCore;
using Server.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.Linq;
using System.Windows;
using System.Windows.Input;
using System;
using System.IO;
using Microsoft.Win32;
using ClosedXML.Excel;
using System.Diagnostics;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for ProductUC.xaml
    /// </summary>
    public partial class ProductUC : UserControl
    {
        public ProductUC()
        {
            InitializeComponent();
        }
        private void Refresh_DG()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Partiyas
                             .Include(x => x.Product)
                             .Include(x => x.Product.Massa)
                             .Include(x => x.Product.Types)
                             group a by new
                             {
                                 a.Product.Shtrix,
                                 a.Product.NameOfProduct,
                                 a.Product.Types.TypeName,
                                 a.Product.Massa.Name,
                             } into gcs
                             select new InfoAllProduct()
                             {
                                 Shtrix = gcs.Key.Shtrix,
                                 NameProduct = gcs.Key.NameOfProduct,
                                 TypeName = gcs.Key.TypeName,
                                 MassaName = gcs.Key.Name,
                                 Qoldiq = gcs.Sum(x => Math.Round(Convert.ToDecimal(x.CountProduct), 2)),
                             }).ToList();
                    v.OrderBy(x => x.NameProduct);
                    allProduct_DG.ItemsSource = v;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error14");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh_DG();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ReadShtrixWindow window = new ReadShtrixWindow();
            window.ShowDialog();
            Refresh_DG();
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (allProduct_DG.Items.Count > 0)
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
                            worksheet.Cell(1, 1).Value = "Все продукты. Дата: " + DateTime.Now;
                            worksheet.Cell(1, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

                            worksheet.Range(worksheet.Cell(1, 1), worksheet.Cell(1, 6)).Merge();
                            worksheet.Cell(2, 1).Value = "Штрих-код";
                            worksheet.Cell(2, 2).Value = "Товар";
                            worksheet.Cell(2, 3).Value = "Тип";
                            worksheet.Cell(2, 4).Value = "Массовый тип";
                            worksheet.Cell(2, 5).Value = "Кол-во";

                            int kk = 3;
                            foreach (InfoAllProduct rv in allProduct_DG.Items)
                            {
                                worksheet.Cell(kk, 1).Value = rv.Shtrix;
                                worksheet.Cell(kk, 2).Value = rv.NameProduct;
                                worksheet.Cell(kk, 3).Value = rv.TypeName;
                                worksheet.Cell(kk, 4).Value = rv.MassaName;
                                worksheet.Cell(kk, 5).Value = rv.Qoldiq;
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
                MessageBox.Show("Error15");
            }
        }

        private void allProduct_DG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (allProduct_DG.SelectedCells.Count > 0 && allProduct_DG.SelectedCells[0].IsValid)
                {
                    InfoAllProduct t = allProduct_DG.SelectedItem as InfoAllProduct;
                    AddProductWindow window = new AddProductWindow(t.Shtrix, true);
                    window.ShowDialog();
                    Refresh_DG();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error16");
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh_DG();
        }
    }
}
