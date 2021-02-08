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
    /// Interaction logic for DiscountWindow.xaml
    /// </summary>
    public partial class DiscountWindow : Window
    {
        public DiscountWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Discounts
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        myUpDownControl.Value = v.Precent;
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

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (myUpDownControl.Value >= myUpDownControl.Minimum && myUpDownControl.Value <= myUpDownControl.Maximum)
            {
                try
                {
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        var v = (from a in context.Discounts
                                 select a).FirstOrDefault();
                        if (v != null)
                        {
                            v.Precent = Convert.ToInt32(myUpDownControl.Value);
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
        }
    }
}
