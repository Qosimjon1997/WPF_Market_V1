using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for KassaWindow.xaml
    /// </summary>
    public partial class KassaWindow : Window
    {
        private int _id;

        public KassaWindow(int id)
        {
            InitializeComponent();
            _id = id;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Kassas
                             where a.Id == _id
                             where a.WorkerID == Properties.Settings.Default.WorkerId
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        context.Kassas.Remove(v);
                        context.SaveChanges();
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error13");
            }
        }

        void btnEnterClick()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Kassas
                             where a.Id == _id
                             where a.WorkerID == Properties.Settings.Default.WorkerId
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        if (myUpDown.Value > 0)
                        {
                            v.CountProduct = Convert.ToDecimal(myUpDown.Value);
                            v.AllPrice = Convert.ToDecimal(labSUMMA.Text);
                        }
                        else
                        {
                            context.Kassas.Remove(v);
                        }
                        context.SaveChanges();
                        this.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error14");
            }

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            btnEnterClick();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Kassas
                             .Include(x => x.Partiya.Product)
                             .Include(x => x.Partiya.Product.Types)
                             .Include(x => x.Partiya.Product.Massa)
                             where a.Id == _id
                             where a.WorkerID == Properties.Settings.Default.WorkerId
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        labTovarNomi.Text = v.Partiya.Product.NameOfProduct;
                        labTovarMiqdorTuri.Text = v.Partiya.Product.Massa.Name;
                        labTovarTuri.Text = v.Partiya.Product.Types.TypeName;
                        labTovarNarci.Text = v.Partiya.SalePrice.ToString();
                        myUpDown.Value = Convert.ToDecimal(v.CountProduct);
                        AllSumma();
                    }
                    var b = (from a in context.Partiyas
                             where a.Id == v.Partiya.Id
                             select a).FirstOrDefault();
                    myUpDown.Maximum = Convert.ToDecimal(b.CountProduct);
                    labMaxKol.Text = (b.CountProduct).ToString();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error15");
            }
        }

        void AllSumma()
        {
            decimal pricaOne = Convert.ToDecimal(labTovarNarci.Text);
            decimal countAll = Convert.ToDecimal(myUpDown.Value);
            labSUMMA.Text = Convert.ToString(pricaOne * countAll);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if(e.Key == Key.Enter)
            {
                btnEnterClick();
            }
        }

        private void myUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            AllSumma();
        }
    }
}
