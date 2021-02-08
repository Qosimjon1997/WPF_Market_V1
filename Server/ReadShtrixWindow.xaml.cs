using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Server
{
    /// <summary>
    /// Interaction logic for ReadShtrixWindow.xaml
    /// </summary>
    public partial class ReadShtrixWindow : Window
    {
        public ReadShtrixWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtShtrix.Focus();
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
            myPressFunc();
        }

        private void myPressFunc()
        {
            if (txtShtrix.Text != "")
            {
                try
                {
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        var v = (from a in context.Products
                                 where a.Shtrix == txtShtrix.Text
                                 select a).FirstOrDefault();
                        if (v == null)
                        {
                            AddProductWindow window = new AddProductWindow(txtShtrix.Text, false);
                            window.ShowDialog();
                        }
                        else
                        {
                            AddProductWindow window = new AddProductWindow(txtShtrix.Text, true);
                            window.ShowDialog();
                        }
                        txtShtrix.Text = "";
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error");
                }
            }
        }
        private void txtShtrix_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                myPressFunc();
            }
        }
    }
}
