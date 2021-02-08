using Microsoft.EntityFrameworkCore;
using Server.Data;
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
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for ChangeAddProductWindow.xaml
    /// </summary>
    public partial class ChangeAddProductWindow : Window
    {
        private int _id;
        private int tType;
        private int tMassa;
        private int tProvider;

        public ChangeAddProductWindow(int id)
        {
            InitializeComponent();
            _id = id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var t = (from a in context.Providers
                             select a).ToList();
                    foreach (Provider p in t)
                    {
                        combProvider.Items.Add(p.ProviderName);
                    }

                    var v = (from a in context.Partiyas
                             .Include(x => x.Product)
                             .Include(x => x.Provider)
                             .Include(x => x.Product.Types)
                             .Include(x => x.Product.Massa)
                             where a.Id == _id
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        txtShtrix.Text = v.Product.Shtrix;
                        txtNameOfProduct.Text = v.Product.NameOfProduct;
                        txtbazaviyPrice.Text = ((int)v.BazaPrice).ToString();
                        txtSotuvPrice.Text = ((int)v.SalePrice).ToString();
                        txtSoni.Text = v.CountProduct2.ToString();
                        date1.SelectedDate = v.TodayDate;
                        combTypeMassa.Items.Add(v.Product.Types.TypeName);
                        combTypeOf.Items.Add(v.Product.Massa.Name);
                        combProvider.SelectedItem = v.Provider.ProviderName;
                    }
                    combTypeMassa.SelectedIndex = 0;
                    combTypeOf.SelectedIndex = 0;
                    date1.SelectedDate = DateTime.Today;

                    txtSoni.IsEnabled = false;
                    txtShtrix.IsEnabled = false;
                    txtNameOfProduct.IsEnabled = false;
                    combTypeMassa.IsEnabled = false;
                    combTypeOf.IsEnabled = false;

                    txtbazaviyPrice.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Partiyas
                             .Include(x => x.Product)
                             .Include(x => x.Provider)
                             .Include(x => x.Product.Types)
                             .Include(x => x.Product.Massa)
                             where a.Id == _id
                             select a).FirstOrDefault();

                    v.BazaPrice = Convert.ToDecimal(txtbazaviyPrice.Text);
                    v.SalePrice = Convert.ToDecimal(txtSotuvPrice.Text);
                    v.TodayDate = (DateTime)date1.SelectedDate;
                    v.ProviderId = tProvider;

                    context.SaveChanges();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Partiyas
                             .Include(x => x.Product)
                             .Include(x => x.Provider)
                             where a.Id == _id
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        context.Partiyas.Remove(v);
                        context.SaveChanges();
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }

        }

        private void btnSrokUtgan_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SrokUtganTovarWindow window = new SrokUtganTovarWindow(_id);
            window.ShowDialog();
            this.Close();
        }

        private void txtSoni_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtbazaviyPrice.Text == "")
            {
                txtbazaviyPrice.Text = "0";
            }
        }

        private void txtSoni_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void combProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                MessageBox.Show("Error");
            }

        }

        private void txtSotuvPrice_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtbazaviyPrice.Text == "")
            {
                txtbazaviyPrice.Text = "0";
            }
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

        private void txtbazaviyPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtFoizga_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtSotuvPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void combTypeMassa_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                MessageBox.Show("Error");
            }

        }

        private void combTypeOf_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                MessageBox.Show("Error");
            }

        }
    }
}
