using Kassa1.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for ServerSettingsWindow.xaml
    /// </summary>
    public partial class ServerSettingsWindow : Window
    {
        public ServerSettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtServerName.Focus();
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ServerName = txtServerName.Text;
            Properties.Settings.Default.UserId = txtUserName.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.DbName = txtDB.Text;
            Properties.Settings.Default.Save();

            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Admins
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        MessageBox.Show("Успешно подключился к серверу!");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:1 - Ошибка подключения к серверу");
            }
        }
    }
}
