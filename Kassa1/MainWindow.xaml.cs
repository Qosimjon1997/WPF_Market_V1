using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            // Some Code
            //if((txtShtrix.Text).Length != 0)
            //{
            //    string A = txtShtrix.Text;
            //    A = 
            //}
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
                         orderby a.TodayDate
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
                else
                {
                    txtShtrix.Text = "";
                    MessageBox.Show("Этот товар не найден в базе");
                }
            }
        }

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
            // Some Code
            txtShtrix.Focus();
        }

        private void btnDeleteKassa_Click(object sender, RoutedEventArgs e)
        {
            // Some Code
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
            txtShtrix.Focus();
        }

        private void btnProductNoBar_Click(object sender, RoutedEventArgs e)
        {
            ProductOfNoBarWindow window = new ProductOfNoBarWindow();
            window.ShowDialog();
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                txtShtrix.Text = "";
                txtShtrix.Focus();
            }
            if(e.Key == Key.Enter)
            {
                pressEnter();
            }
        }

        private void txtShtrix_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            using(ApplicationDBContext context = new ApplicationDBContext())
            {
                Smena smena = new Smena()
                {
                    Started = Convert.ToDateTime(Properties.Settings.Default.StartSmena),
                    Finished = DateTime.Now,
                    WorkerId = Properties.Settings.Default.WorkerId
                };
                context.Smenas.Add(smena);
                context.SaveChanges();
            }
        }
    }
}
