using Microsoft.EntityFrameworkCore;
using Server.Data;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for DeleteSold.xaml
    /// </summary>
    public partial class DeleteSold : Window
    {
        private int _id;

        public DeleteSold(int Id)
        {
            InitializeComponent();
            _id = Id;
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    Sold v = (from a in context.Solds
                              where a.Id == _id
                              select a).FirstOrDefault();

                    if (v != null)
                    {
                        Partiya p = (from a in context.Partiyas
                                     .Include(x => x.Product)
                                     .Include(x => x.Product.Massa)
                                     .Include(x => x.Product.Types)
                                     where v.PartiyaId == a.Id
                                     select a).FirstOrDefault();
                        p.CountProduct += v.CountProduct;

                        Vazvrat t = new Vazvrat()
                        {
                            Tovar = p.Product.NameOfProduct,
                            Shtrix = p.Product.Shtrix,
                            MassaName = p.Product.Massa.Name,
                            TypeName = p.Product.Types.TypeName,
                            CountProduct = v.CountProduct
                        };
                        context.Vazvrats.Add(t);
                        context.Solds.Remove(v);
                        context.SaveChanges();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Этот товар не найден в базе");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }

        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
