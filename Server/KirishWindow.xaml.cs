using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Server
{
    /// <summary>
    /// Interaction logic for KirishWindow.xaml
    /// </summary>
    public partial class KirishWindow : Window
    {
        public KirishWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            labServerName.Text = Properties.Settings.Default.ServerName;
            txtLogin.Focus();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Admins
                             select a).FirstOrDefault();
                    if (v.Login == txtLogin.Text && v.Passw == txtPassw.Password)
                    {
                        this.Hide();
                        MainWindow window = new MainWindow();
                        window.ShowDialog();
                        this.Show();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:2 - Ошибка подключения к серверу!.");
            }
            
        }

        private void btnMenuSerringsServer_Click(object sender, RoutedEventArgs e)
        {
            ServerSettingsWindow window = new ServerSettingsWindow();
            window.ShowDialog();
            labServerName.Text = Properties.Settings.Default.ServerName;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                try
                {
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        var v = (from a in context.Admins
                                 select a).FirstOrDefault();
                        if (v.Login == txtLogin.Text && v.Passw == txtPassw.Password)
                        {
                            this.Hide();
                            MainWindow window = new MainWindow();
                            window.ShowDialog();
                            this.Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error:3 - Ошибка подключения к серверу!.");
                }
            }
        }
    }
}
