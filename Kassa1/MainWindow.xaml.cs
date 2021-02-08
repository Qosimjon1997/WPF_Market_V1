using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtShtrix.Focus();
            RefreshKassa();
            labSmenaStart.Text = DateTime.Now.ToString();
            txtEmployee.Text = Properties.Settings.Default.WorkerFIO;
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "1";
            txtShtrix.Focus();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "2";
            txtShtrix.Focus();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "3";
            txtShtrix.Focus();
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "4";
            txtShtrix.Focus();
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "5";
            txtShtrix.Focus();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "6";
            txtShtrix.Focus();
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "7";
            txtShtrix.Focus();
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "8";
            txtShtrix.Focus();
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "9";
            txtShtrix.Focus();
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "0";
            txtShtrix.Focus();
        }

        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text = "";
            txtShtrix.Focus();
        }

        private void btnEnterNumber_Click(object sender, RoutedEventArgs e)
        {
            // Some Code
            pressEnter();
            txtShtrix.Focus();
        }

        void pressEnter()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Partiyas
                                .Include(p => p.Product)
                             where a.Product.Shtrix == txtShtrix.Text
                             orderby a.TodayDate
                             select a).ToList();
                    var p = (from a in context.Partiyas
                                .Include(p => p.Product)
                             where a.Product.Shtrix == txtShtrix.Text
                             select a).Count();

                    if (p != 0)
                    {
                        foreach (var q in v)
                        {
                            var t1 = (from a in context.Kassas
                                      where a.Partiya.Id == q.Id
                                      where a.WorkerID == Properties.Settings.Default.WorkerId
                                      select a).FirstOrDefault();

                            if (t1 != null)
                            {
                                if (q.CountProduct > t1.CountProduct)
                                {
                                    var y = (from a in context.Kassas
                                             where a.Partiya.Id == q.Id
                                             where a.WorkerID == Properties.Settings.Default.WorkerId
                                             select a).FirstOrDefault();
                                    y.CountProduct += 1;
                                    y.AllPrice = q.SalePrice * y.CountProduct;
                                    context.SaveChanges();
                                    break;
                                }
                            }
                            else
                            {
                                Kassa k = new Kassa()
                                {
                                    CountProduct = 1,
                                    AllPrice = q.SalePrice,
                                    Partiya = q,
                                    WorkerID = Properties.Settings.Default.WorkerId
                                };
                                context.Kassas.Add(k);
                                context.SaveChanges();
                                break;
                            }
                        }

                        txtShtrix.Text = "";
                        RefreshKassa();
                    }
                    else if (p==0)
                    {
                        txtShtrix.Text = "";
                        //MessageBox.Show("Этот товар не найден в базе");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        //Tayyor
        private void RefreshKassa()
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Kassas
                         .Include(p => p.Partiya)
                         where a.WorkerID == Properties.Settings.Default.WorkerId
                         select new InfoKassa()
                         {
                             Id = a.Id,
                             Shtrix = a.Partiya.Product.Shtrix,
                             Name = a.Partiya.Product.NameOfProduct,
                             TypeName = a.Partiya.Product.Types.TypeName,
                             MassaName = a.Partiya.Product.Massa.Name,
                             Price = a.Partiya.SalePrice,
                             CountProduct = a.CountProduct,
                             AllPrice = a.AllPrice
                         }).ToList();
                decimal SUMMA = 0;
                foreach (var k in v)
                {
                    SUMMA += k.AllPrice;
                }
                
                labSumma.Text = SUMMA.ToString();
                DG_Kassa.ItemsSource = v;
            }
        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            pressSold();
        }

        private void btnDeleteKassa_Click(object sender, RoutedEventArgs e)
        {
            // Some Code
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Kassas
                             where a.WorkerID == Properties.Settings.Default.WorkerId
                             select a).ToList();
                    foreach (Kassa k in v)
                    {
                        context.Kassas.Remove(k);
                    }
                    context.SaveChanges();
                    RefreshKassa();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
            txtShtrix.Focus();
        }

        private void btnProductNoBar_Click(object sender, RoutedEventArgs e)
        {
            ProductOfNoBarWindow window = new ProductOfNoBarWindow();
            window.ShowDialog();
            RefreshKassa();
            txtShtrix.Focus();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword window = new ChangePassword();
            window.ShowDialog();
            txtShtrix.Focus();
        }

        private void btnDebtors_Click(object sender, RoutedEventArgs e)
        {
            WorkerDebtWindow window = new WorkerDebtWindow();
            window.ShowDialog();
            txtShtrix.Focus();
        }

        private void txtShtrix_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
            txtShtrix.Focus();
        }

        private void pressSold()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    int v = (from a in context.Kassas
                             where a.WorkerID == Properties.Settings.Default.WorkerId
                             select a).Count();
                    if (v > 0)
                    {
                        SellingWindow window = new SellingWindow(Convert.ToDecimal(labSumma.Text));
                        window.ShowDialog();
                        RefreshKassa();

                        int v2 = (from a in context.Kassas
                                  where a.WorkerID == Properties.Settings.Default.WorkerId
                                  select a).Count();
                        if (v2 == 0)
                        {
                            labSumma.Text = "0";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
            txtShtrix.Focus();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                txtShtrix.Text = "";
                txtShtrix.Focus();
            }
            else if(e.Key == Key.Enter)
            {
                pressEnter();
            }
            else if (e.Key == Key.F5)
            {
                pressSold();
            }
        }

        private void txtShtrix_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                pressEnter();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    Smena smena = new Smena()
                    {
                        Started = Convert.ToDateTime(labSmenaStart.Text),
                        Finished = DateTime.Now,
                        WorkerId = Properties.Settings.Default.WorkerId
                    };
                    context.Smenas.Add(smena);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void DG_Kassa_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DG_Kassa.SelectedCells.Count > 0 && DG_Kassa.SelectedCells[0].IsValid)
                {
                    InfoKassa t = DG_Kassa.SelectedItem as InfoKassa;
                    KassaWindow window = new KassaWindow(t.Id);
                    window.ShowDialog();
                    RefreshKassa();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

    }
}
