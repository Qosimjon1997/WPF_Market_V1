using Kassa1.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kassa1
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

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            pressEnter();
        }

        private void pressEnter()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Workers
                             where a.Login == combLogin.SelectedItem.ToString()
                             select a).FirstOrDefault();
                    if (v != null && v.Login == combLogin.SelectedItem.ToString() && v.Passw == txtPassw.Password && v.Active == true)
                    {
                        Properties.Settings.Default.WorkerId = v.Id;
                        Properties.Settings.Default.WorkerEntered = true;
                        Properties.Settings.Default.WorkerFIO = v.FIO;
                        Properties.Settings.Default.Save();
                        this.Hide();
                        MainWindow window = new MainWindow();
                        window.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        MessageBox.Show("Error Worker!");
                    }

                    txtPassw.Password = "";
                    RefreshComb();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error2");
            }
        }
        private void btnMenuSerringsServer_Click(object sender, RoutedEventArgs e)
        {
            ServerSettingsWindow window = new ServerSettingsWindow();
            window.ShowDialog();
            RefreshComb();
            labServerName.Text = Properties.Settings.Default.ServerName;
        }

        void RefreshComb()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    combLogin.Items.Clear();
                    var b = (from a in context.Workers
                             where a.Active == true
                             select a).ToList();
                    foreach (Worker w in b)
                    {
                        combLogin.Items.Add(w.Login);
                    }
                    combLogin.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error3");
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshComb();
            labServerName.Text = Properties.Settings.Default.ServerName;
            txtPassw.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                pressEnter();

            }
        }
    }
}
