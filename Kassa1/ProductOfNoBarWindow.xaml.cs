using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for ProductOfNoBarWindow.xaml
    /// </summary>
    public partial class ProductOfNoBarWindow : Window
    {
        public ProductOfNoBarWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Partiyas
                             .Include(p => p.Product)
                             .Include(x => x.Product.Types)
                             where a.CountProduct > 0
                             select new InfoPartiya()
                             {
                                 Id = a.Id,
                                 Shtrix = a.Product.Shtrix,
                                 Name = a.Product.NameOfProduct,
                                 TypeName = a.Product.Types.TypeName,
                                 MassaName = a.Product.Massa.Name,
                                 Price = a.SalePrice,
                             }).ToList();
                    v.OrderBy(x => x.Name);
                    DG_AllProduct.ItemsSource = v;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error16");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void btnSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Partiyas
                             .Include(p => p.Product)
                             where a.Product.NameOfProduct.Contains(btnSearch.Text) || a.Product.Shtrix.Contains(btnSearch.Text)
                             orderby a.TodayDate
                             select new InfoPartiya()
                             {
                                 Id = a.Id,
                                 Shtrix = a.Product.Shtrix,
                                 Name = a.Product.NameOfProduct,
                                 TypeName = a.Product.Types.TypeName,
                                 MassaName = a.Product.Massa.Name,
                                 Price = a.SalePrice,
                             }).ToList();
                    DG_AllProduct.ItemsSource = v;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error17");
            }
        }

        private void DG_AllProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DG_AllProduct.SelectedCells.Count > 0 && DG_AllProduct.SelectedCells[0].IsValid)
                {
                    InfoPartiya t = DG_AllProduct.SelectedItem as InfoPartiya;
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        Partiya v = (from a in context.Partiyas
                                     .Include(p => p.Product)
                                     where a.Id == t.Id
                                     orderby a.TodayDate
                                     select a).FirstOrDefault();

                        if (v != null)
                        {
                            var t1 = (from a in context.Kassas
                                      where a.Partiya.Id == v.Id
                                      where a.WorkerID == Properties.Settings.Default.WorkerId
                                      select a).FirstOrDefault();

                            if (t1 != null)
                            {
                                if (v.CountProduct > t1.CountProduct)
                                {
                                    var y = (from a in context.Kassas
                                             where a.Partiya.Id == v.Id
                                             where a.WorkerID == Properties.Settings.Default.WorkerId
                                             select a).FirstOrDefault();
                                    y.CountProduct += 1;
                                    y.AllPrice = v.SalePrice * y.CountProduct;
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                Kassa k = new Kassa()
                                {
                                    CountProduct = 1,
                                    AllPrice = v.SalePrice,
                                    Partiya = v,
                                    WorkerID = Properties.Settings.Default.WorkerId
                                };
                                context.Kassas.Add(k);
                                context.SaveChanges();

                            }
                            MessageBox.Show("Товар был успешно добавлен");
                        }
                        else
                        {
                            MessageBox.Show("Этот товар не найден в базе");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error18");
            }
            btnSearch.Text = "";
            btnSearch.Focus();
        }
    }
}
