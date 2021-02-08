using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Server
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void txtOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Admins
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        if (v.Passw == txtOldPassw.Password && txtNew1Passw.Password == txtNew2Passw.Password)
                        {
                            v.Passw = txtNew1Passw.Password;
                            context.SaveChanges();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Пароль неправильно");
                        }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtOldPassw.Focus();
        }
    }
}
