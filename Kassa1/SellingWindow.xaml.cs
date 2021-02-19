using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for SellingWindow.xaml
    /// </summary>
    public partial class SellingWindow : Window
    {
        private decimal _AllSum;

        public SellingWindow(decimal AllSum)
        {
            InitializeComponent();
            _AllSum = AllSum;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNaqt.Text = (Convert.ToInt32(_AllSum)).ToString();
            labSUMM.Text = _AllSum.ToString();
            Refresh_DG();

        }

        private void btnAddNewDebtor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.DebtInfos
                             where a.FIO == txtFIO.Text
                             select a).FirstOrDefault();
                    if (v == null)
                    {
                        DebtInfo d = new DebtInfo()
                        {
                            FIO = txtFIO.Text,
                            Address = txtAddress.Text,
                            Phone = txtTel.Text
                        };
                        context.DebtInfos.Add(d);
                        context.SaveChanges();
                        txtFIO.Text = txtAddress.Text = txtTel.Text = "";
                        txtFIO_Qarzdor.Text = d.FIO;
                        Refresh_DG();
                    }
                    else
                    {
                        MessageBox.Show("Этот человек зарегистрирован");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error19");
            }
        }
        private void Refresh_DG()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.DebtInfos
                             select a).ToList();
                    DG_AllDebtor.ItemsSource = v;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error20");
            }
            
        }

        #region Check chiqarish
        public void Print()
        {
            var doc = new PrintDocument();
            var paperSize = new PaperSize("Custom", 520, 5000);
            doc.DefaultPageSettings.PaperSize = paperSize;
            doc.PrintPage += new PrintPageEventHandler(ProvideContent);
            doc.Print();
        }

        public void ProvideContent(object sender, PrintPageEventArgs e)
        {
            const int FIRST_COL_PAD = 8;
            const int SECOND_COL_PAD = 10;
            const int THIRD_COL_PAD = 12;


            var sb = new StringBuilder();
            //replace with item.Branch
            sb.AppendLine("OK IMRONSHOX SHOXJAXON");
            sb.AppendLine("Samarqand,Samarqand shahar");
            sb.AppendLine("Qorasuv maskani 77/3");
            sb.AppendLine(" ");
            sb.AppendLine(("Тел:+998 97 914-78-87"));
            sb.AppendLine(" ");
            sb.Append(("Дата:").PadRight(8));
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(" ");
            sb.Append(("Товар").PadLeft(FIRST_COL_PAD));
            sb.Append(("Кол-во").PadLeft(SECOND_COL_PAD));
            sb.AppendLine(("Цена").PadLeft(THIRD_COL_PAD));
            sb.AppendLine("-".PadRight(60, '-'));

            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Kassas
                         .Include(x => x.Partiya.Product.Massa)
                         .Include(x => x.Partiya.Product.Types)
                         .Include(x => x.Partiya.Product)
                         where a.WorkerID == Properties.Settings.Default.WorkerId
                         select a).ToList();
                if (v.Count > 0)
                {
                    foreach (Kassa k in v)
                    {
                        string q = k.Partiya.Product.NameOfProduct;
                        if (q.Length > 10)
                        {
                            sb.AppendLine(q.Substring(0, 10));
                            sb.Append(q.Substring(11).PadLeft(FIRST_COL_PAD));
                        }
                        else
                        {
                            sb.Append(q.PadLeft(FIRST_COL_PAD));
                        }
                        //sb.Append(q.PadLeft(FIRST_COL_PAD));
                        sb.Append((k.CountProduct.ToString()).PadLeft(SECOND_COL_PAD));
                        sb.AppendLine((k.AllPrice.ToString().PadLeft(THIRD_COL_PAD)));
                        sb.AppendLine(" ");
                    }
                }
            }
            sb.AppendLine("-".PadRight(60, '-'));
            sb.AppendLine(" ");
            sb.AppendLine("Наличные:" + String.Format("{0:0.00}", Convert.ToDecimal(txtNaqt.Text)));
            sb.AppendLine(" ");
            sb.AppendLine("Пластик:" + String.Format("{0:0.00}", Convert.ToDecimal(txtPlastik.Text)));
            sb.AppendLine(" ");
            sb.AppendLine("Долг:" + String.Format("{0:0.00}", Convert.ToDecimal(txtQarz.Text)));
            sb.AppendLine(" ");
            sb.AppendLine("Общий сумма:" + _AllSum);
            sb.AppendLine(" ");
            sb.AppendLine(" ");
            sb.AppendLine(" ");
            sb.AppendLine(" ");
            sb.AppendLine(" ");
            sb.AppendLine(" ");

            var printText = new PrintText(sb.ToString(), new Font(System.Drawing.FontFamily.GenericMonospace, 7, System.Drawing.FontStyle.Bold));
            Graphics graphics = e.Graphics;
            int startX = 0;
            int startY = 0;
            int Offset = 20;

            graphics.DrawString(printText.Text, printText.Font, new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
        }
        #endregion


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    #region
                    if ((Convert.ToInt32(txtPlastik.Text) + Convert.ToInt32(txtNaqt.Text) + Convert.ToInt32(txtQarz.Text)) == Convert.ToInt32(_AllSum))
                    {
                        if (checkQarz.IsChecked == true)
                        {
                            if (txtQarz.Text != "0" && txtQarz.Text != "")
                            {
                                var q = (from a in context.DebtInfos
                                         where a.FIO == txtFIO_Qarzdor.Text
                                         select a).FirstOrDefault();
                                if (q != null)
                                {
                                    Income i = new Income()
                                    {
                                        CashIncome = Convert.ToDecimal(txtNaqt.Text),
                                        PlasticIncome = Convert.ToDecimal(txtPlastik.Text),
                                        DateTimeNow = DateTime.Now,
                                        DebtIncome = 0,
                                        DebtOut = Convert.ToDecimal(txtQarz.Text),
                                        SaleProductPrice = Convert.ToDecimal(_AllSum),
                                        Vozvrat = 0,
                                        WorkerId = Properties.Settings.Default.WorkerId
                                    };
                                    Debt d = new Debt()
                                    {
                                        timeNow = DateTime.Now,
                                        Price = -1*Convert.ToDecimal(txtQarz.Text),
                                        DebtInfoId = q.Id
                                    };

                                    context.Incomes.Add(i);
                                    context.Debts.Add(d);

                                    var o = (from b in context.Kassas
                                     .Include(x => x.Partiya)
                                     .Include(x => x.Partiya.Product)
                                     .Include(x => x.Partiya.Product.Massa)
                                     .Include(x => x.Partiya.Product.Types)
                                     .Include(x => x.Partiya.Provider)
                                             where b.WorkerID == Properties.Settings.Default.WorkerId
                                             select b).ToList();

                                    /*
                                     Check chiqarish uchun kod
                                     */
                                    if (checkPrintCheck.IsChecked == true)
                                    {
                                        Print();
                                    }

                                    /*           */
                                    foreach (var t in o)
                                    {
                                        Sold s = new Sold()
                                        {
                                            ProductId = t.Partiya.Product.Id,
                                            Shtrix = t.Partiya.Product.Shtrix,
                                            NameOfProduct = t.Partiya.Product.NameOfProduct,

                                            PartiyaId = t.Partiya.Id,
                                            BazaPrice = t.Partiya.BazaPrice,
                                            SalePrice = t.Partiya.SalePrice,
                                            CountProduct = t.CountProduct,

                                            ProviderId = t.Partiya.Provider.Id,
                                            ProviderName = t.Partiya.Provider.ProviderName,

                                            MassaId = t.Partiya.Product.Massa.Id,
                                            MassaName = t.Partiya.Product.Massa.Name,

                                            TypeId = t.Partiya.Product.Types.Id,
                                            TypeName = t.Partiya.Product.Types.TypeName,

                                            AllSumma = t.AllPrice,

                                            WorkerId = Properties.Settings.Default.WorkerId,

                                            dateTimeNow = DateTime.Now
                                        };
                                        context.Solds.Add(s);
                                        var y = (from c in context.Partiyas
                                                 .Include(x => x.Product)
                                                 where t.Partiya.Id == c.Id
                                                 select c).FirstOrDefault();
                                        y.CountProduct -= t.CountProduct;
                                        context.Kassas.Remove(t);
                                    }

                                    MainWindow m = new MainWindow();
                                    context.SaveChanges();
                                    this.Close();

                                }

                            }
                            else
                            {
                                MessageBox.Show("Ошибка с долгом!");
                            }
                        }
                        else
                        {
                            Income i = new Income()
                            {
                                CashIncome = Convert.ToDecimal(txtNaqt.Text),
                                PlasticIncome = Convert.ToDecimal(txtPlastik.Text),
                                DateTimeNow = DateTime.Now,
                                DebtIncome = 0,
                                DebtOut = Convert.ToDecimal(txtQarz.Text),
                                SaleProductPrice = Convert.ToDecimal(_AllSum),
                                Vozvrat = 0,
                                WorkerId = Properties.Settings.Default.WorkerId
                            };
                            context.Incomes.Add(i);

                            var o = (from b in context.Kassas
                                     .Include(x => x.Partiya)
                                     .Include(x => x.Partiya.Product)
                                     .Include(x => x.Partiya.Product.Massa)
                                     .Include(x => x.Partiya.Product.Types)
                                     .Include(x => x.Partiya.Provider)
                                     where b.WorkerID == Properties.Settings.Default.WorkerId
                                     select b).ToList();
                            /*
                             Check chiqarish uchun kod
                             */
                            if (checkPrintCheck.IsChecked == true)
                            {
                                Print();
                            }
                            /*      */
                            foreach (var t in o)
                            {
                                Sold s = new Sold()
                                {
                                    ProductId = t.Partiya.Product.Id,
                                    Shtrix = t.Partiya.Product.Shtrix,
                                    NameOfProduct = t.Partiya.Product.NameOfProduct,

                                    PartiyaId = t.Partiya.Id,
                                    BazaPrice = t.Partiya.BazaPrice,
                                    SalePrice = t.Partiya.SalePrice,
                                    CountProduct = t.CountProduct,

                                    ProviderId = t.Partiya.Provider.Id,
                                    ProviderName = t.Partiya.Provider.ProviderName,

                                    MassaId = t.Partiya.Product.Massa.Id,
                                    MassaName = t.Partiya.Product.Massa.Name,

                                    TypeId = t.Partiya.Product.Types.Id,
                                    TypeName = t.Partiya.Product.Types.TypeName,

                                    AllSumma = t.AllPrice,

                                    WorkerId = Properties.Settings.Default.WorkerId,

                                    dateTimeNow = DateTime.Now
                                };
                                context.Solds.Add(s);
                                var y = (from c in context.Partiyas
                                         .Include(x => x.Product)
                                         where t.Partiya.Id == c.Id
                                         select c).FirstOrDefault();
                                y.CountProduct -= t.CountProduct;
                                context.Kassas.Remove(t);
                            }

                            this.Close();

                            context.SaveChanges();

                        }
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error21");
            }
        }

        private void txtNaqt_KeyUp(object sender, KeyEventArgs e)
        {
            if (checkQarz.IsChecked == true)
            {
                decimal All = Convert.ToDecimal(labSUMM.Text);
                decimal naqt;
                decimal plastik;
                if (txtPlastik.Text == "")
                {
                    txtPlastik.Text = "0";
                    plastik = 0;
                }
                else
                {
                    plastik = Convert.ToDecimal(txtPlastik.Text);
                }
                if (txtNaqt.Text == "")
                {
                    txtNaqt.Text = "0";
                    naqt = 0;
                }
                else
                {
                    naqt = Convert.ToDecimal(txtNaqt.Text);
                }
                txtQarz.Text = (Convert.ToInt32((All - plastik - naqt))).ToString();
            }
            else
            {
                decimal All = Convert.ToDecimal(labSUMM.Text);
                decimal naqt;
                if (txtNaqt.Text == "")
                {
                    txtNaqt.Text = "0";
                    naqt = 0;
                }
                else
                {
                    naqt = Convert.ToDecimal(txtNaqt.Text);
                }
                txtPlastik.Text = (Convert.ToInt32((All - naqt))).ToString();
            }
        }

        private void txtPlastik_KeyUp(object sender, KeyEventArgs e)
        {
            if (checkQarz.IsChecked == true)
            {
                decimal All = Convert.ToDecimal(labSUMM.Text);
                decimal naqt;
                decimal plastik;
                if (txtPlastik.Text == "")
                {
                    txtPlastik.Text = "0";
                    plastik = 0;
                }
                else
                {
                    plastik = Convert.ToDecimal(txtPlastik.Text);
                }
                if (txtNaqt.Text == "")
                {
                    txtNaqt.Text = "0";
                    naqt = 0;
                }
                else
                {
                    naqt = Convert.ToDecimal(txtNaqt.Text);
                }
                txtQarz.Text = (Convert.ToInt32((All - plastik - naqt))).ToString();
            }
            else
            {
                decimal All = Convert.ToDecimal(labSUMM.Text);
                decimal plastik;
                if (txtPlastik.Text == "")
                {
                    txtPlastik.Text = "0";
                    plastik = 0;
                }
                else
                {
                    plastik = Convert.ToDecimal(txtPlastik.Text);
                }
                txtNaqt.Text = (Convert.ToInt32((All - plastik))).ToString();
            }
        }

        private void txtQarz_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtQarz.Text == "")
            {
                txtQarz.Text = "0";
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void DG_AllDebtor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DG_AllDebtor.SelectedCells.Count > 0 && DG_AllDebtor.SelectedCells[0].IsValid)
                {
                    DebtInfo pDebtInfo = new DebtInfo();
                    pDebtInfo = DG_AllDebtor.SelectedValue as DebtInfo;
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        DebtInfo v = (from a in context.DebtInfos
                                      where a.FIO == pDebtInfo.FIO
                                      select a).FirstOrDefault();

                        if (v != null)
                        {
                            txtFIO_Qarzdor.Text = v.FIO;
                        }
                        else
                        {
                            MessageBox.Show("Ошибка!!!");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error22");
            }

        }

        void Distribution()
        {
            if (checkQarz.IsChecked == false)
            {
                txtQarz.Text = 0.ToString();
                decimal All = Convert.ToDecimal(labSUMM.Text);
                decimal naqt;
                decimal plastik;
                if (txtPlastik.Text == "")
                {
                    plastik = 0;
                }
                else
                {
                    plastik = Convert.ToDecimal(txtPlastik.Text);
                }
                if (txtNaqt.Text == "")
                {
                    naqt = 0;
                }
                else
                {
                    naqt = Convert.ToDecimal(txtNaqt.Text);
                }
                txtNaqt.Text = (Convert.ToInt32((All - plastik))).ToString();
            }
            else
            {
                decimal All = Convert.ToDecimal(labSUMM.Text);
                decimal naqt;
                decimal plastik;
                if (txtPlastik.Text == "")
                {
                    plastik = 0;
                }
                else
                {
                    plastik = Convert.ToDecimal(txtPlastik.Text);
                }
                if (txtNaqt.Text == "")
                {
                    naqt = 0;
                }
                else
                {
                    naqt = Convert.ToDecimal(txtNaqt.Text);
                }
                txtQarz.Text = (Convert.ToInt32((All - plastik - naqt))).ToString();
            }
        }


        private void txtNaqt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtPlastik_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtQarz_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void checkQarz_Click(object sender, RoutedEventArgs e)
        {
            if (checkQarz.IsChecked == true)
            {
                txtQarz.IsEnabled = true;
                QarzGruh.IsEnabled = true;
            }
            else
            {
                txtQarz.IsEnabled = false;
                QarzGruh.IsEnabled = false;
            }
            Distribution();
        }

    }
}
