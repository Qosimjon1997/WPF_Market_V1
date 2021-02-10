using Microsoft.EntityFrameworkCore;
using Server.Data;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Server
{
    /// <summary>
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : Window
    {
        private string _shtrix;
        private bool _bor;
        private int tMassa;
        private int tType;
        private int tProvider;
        private int _id;

        public AddProductWindow(string shtrix, bool bor)
        {
            InitializeComponent();
            _shtrix = shtrix;
            _bor = bor;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text = _shtrix;
            txtShtrix.IsEnabled = false;
            txtFoizga.IsEnabled = false;
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {

                    if (_bor == true)
                    {
                        txtbazaviyPrice.Focus();
                        var v = (from a in context.Products
                                 .Include(x => x.Massa)
                                 .Include(y => y.Types)
                                 where a.Shtrix == _shtrix
                                 select a).FirstOrDefault();

                        txtNameOfProduct.Text = v.NameOfProduct;
                        _id = v.Id;
                        combTypeMassa.Items.Add(v.Massa.Name.ToString());
                        combTypeOf.Items.Add(v.Types.TypeName.ToString());
                        txtNameOfProduct.IsEnabled = combTypeMassa.IsEnabled = combTypeOf.IsEnabled = false;
                        combTypeMassa.SelectedIndex = 0;
                        combTypeOf.SelectedIndex = 0;
                        combTypeMassa.IsEnabled = false;
                        combTypeOf.IsEnabled = false;
                    }
                    else
                    {
                        txtNameOfProduct.Focus();
                        var m = (from a in context.Massas
                                 select a).ToList();
                        foreach (Massa m1 in m)
                        {
                            combTypeMassa.Items.Add(m1.Name);
                        }

                        var t = (from a in context.Types
                                 select a).ToList();
                        foreach (Types t1 in t)
                        {
                            combTypeOf.Items.Add(t1.TypeName);
                        }
                    }
                    date1.SelectedDate = DateTime.Today;
                    var y = (from a in context.Providers
                             select a).ToList();
                    foreach (Provider p in y)
                    {
                        combProvider.Items.Add(p.ProviderName);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error32");
            }

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    if (_bor == true)
                    {
                        Partiya partiya = new Partiya()
                        {
                            BazaPrice = Convert.ToDecimal(txtbazaviyPrice.Text),
                            SalePrice = Convert.ToDecimal(txtSotuvPrice.Text),
                            CountProduct = Convert.ToDecimal(txtSoni.Text),
                            CountProduct2 = Convert.ToDecimal(txtSoni.Text),
                            TodayDate = (DateTime)date1.SelectedDate,
                            ProductId = _id,
                            ProviderId = tProvider,
                        };
                        context.Partiyas.Add(partiya);
                        context.SaveChanges();
                    }
                    else
                    {
                        Product product = new Product()
                        {
                            Shtrix = txtShtrix.Text,
                            NameOfProduct = txtNameOfProduct.Text,
                            MassaId = tMassa,
                            TypesId = tType,
                        };
                        context.Products.Add(product);
                        context.SaveChanges();

                        Partiya partiya = new Partiya()
                        {
                            BazaPrice = Convert.ToDecimal(txtbazaviyPrice.Text),
                            SalePrice = Convert.ToDecimal(txtSotuvPrice.Text),
                            CountProduct = Convert.ToDecimal(txtSoni.Text),
                            CountProduct2 = Convert.ToDecimal(txtSoni.Text),
                            TodayDate = (DateTime)date1.SelectedDate,
                            Product = product,
                            ProviderId = tProvider
                        };
                        context.Partiyas.Add(partiya);
                        context.SaveChanges();
                    }
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error33");
            }
        }

        private void combTypeMassa_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Massas
                             where a.Name == combTypeMassa.SelectedItem.ToString()
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        tMassa = v.Id;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error34");
            }
        }

        private void combTypeOf_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Types
                             where a.TypeName == combTypeOf.SelectedItem.ToString()
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        tType = v.Id;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error35");
            }
        }

        private void combProvider_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Providers
                             where a.ProviderName == combProvider.SelectedItem.ToString()
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        tProvider = v.Id;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error36");
            }
        }

        private void txtSotuvPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtSoni_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtbazaviyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtbazaviyPrice_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtbazaviyPrice.Text == "")
            {
                txtbazaviyPrice.Text = "0";
            }
            if (checkFoiz.IsChecked == true)
            {
                int x = (Convert.ToInt32(txtbazaviyPrice.Text) + (Convert.ToInt32(txtbazaviyPrice.Text) * Convert.ToInt32(txtFoizga.Text) / 100));
                x = (int)x / 100;
                x *= 100;
                txtSotuvPrice.Text = x.ToString();
            }
        }

        private void txtFoizga_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtFoizga_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtFoizga.Text == "")
            {
                txtFoizga.Text = "0";
            }
            int x = (Convert.ToInt32(txtbazaviyPrice.Text) + (Convert.ToInt32(txtbazaviyPrice.Text) * Convert.ToInt32(txtFoizga.Text) / 100));
            x = (int)x / 100;
            x *= 100;
            txtSotuvPrice.Text = x.ToString();
        }

        private void checkFoiz_Click(object sender, RoutedEventArgs e)
        {
            if (checkFoiz.IsChecked == true)
            {
                txtFoizga.IsEnabled = true;
                string a = txtbazaviyPrice.Text;
                string b = txtFoizga.Text;
                if (a == "")
                {
                    a = "0";
                }
                if (b == "")
                {
                    b = "0";
                }
                txtSotuvPrice.Text = (Convert.ToInt32(a) + (Convert.ToInt32(a) * Convert.ToInt32(b) / 100)).ToString();
                txtSotuvPrice.IsEnabled = false;
            }
            else
            {
                txtFoizga.Text = "0";
                txtSotuvPrice.Text = "0";
                txtSotuvPrice.IsEnabled = true;
                txtFoizga.IsEnabled = false;
            }
        }

        private void txtSotuvPrice_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtSotuvPrice.Text == "")
            {
                txtSotuvPrice.Text = "0";
            }
        }

        private void txtSoni_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtSoni.Text == "")
            {
                txtSoni.Text = "0";
            }
        }
    }
}
