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
    /// Interaction logic for SrokUtganTovarWindow.xaml
    /// </summary>
    public partial class SrokUtganTovarWindow : Window
    {
        private int _id;
        public SrokUtganTovarWindow(int Id)
        {
            InitializeComponent();
            _id = Id;
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
                    if (v.CountProduct >= Convert.ToDecimal(txtSoni.Text))
                    {
                        InvalidGoods q = new InvalidGoods()
                        {
                            Shtrix = v.Product.Shtrix,
                            NameOfProduct = v.Product.NameOfProduct,
                            MassaName = v.Product.Massa.Name,
                            TypeName = v.Product.Types.TypeName,
                            BuyPrice = v.BazaPrice,
                            CountProduct = Convert.ToDecimal(txtSoni.Text),
                            Sana = DateTime.Now
                        };
                        v.CountProduct -= Convert.ToDecimal(txtSoni.Text);
                        context.InvalidGoods.Add(q);
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

        private void txtSoni_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
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
                    if (v != null)
                    {
                        labTovarNomi.Text = v.Product.NameOfProduct;
                        labTovarQoldiq.Text = v.CountProduct.ToString();
                    }
                    txtSoni.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }

        }

    }
}
