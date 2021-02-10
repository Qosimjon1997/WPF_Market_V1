using Kassa1.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.Enter)
            {
                pressEnter();
            }
        }

        private void pressEnter()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Workers
                             where a.Id == Properties.Settings.Default.WorkerId
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
                MessageBox.Show("Error10");
            }
        }
        private void txtOK_Click(object sender, RoutedEventArgs e)
        {
            pressEnter();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtOldPassw.Focus();
        }
    }
}
