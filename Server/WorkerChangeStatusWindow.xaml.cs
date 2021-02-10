using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Server
{
    /// <summary>
    /// Interaction logic for WorkerChangeStatusWindow.xaml
    /// </summary>
    public partial class WorkerChangeStatusWindow : Window
    {
        private int _id;

        public WorkerChangeStatusWindow(int Id)
        {
            InitializeComponent();
            _id = Id;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtFIO.Text != "" && txtLogin.Text != "" && txtPassw.Password != "")
            {
                try
                {
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        var v = (from a in context.Workers
                                 where a.Id == _id
                                 select a).FirstOrDefault();
                        if (v != null)
                        {
                            v.FIO = txtFIO.Text;
                            v.Login = txtLogin.Text;
                            v.Passw = txtPassw.Password;
                            v.Active = Convert.ToBoolean(checkActive.IsChecked);

                            context.SaveChanges();
                            this.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error54");
                }
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
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Workers
                             where a.Id == _id
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        txtFIO.Text = v.FIO;
                        txtLogin.Text = v.Login;
                        txtPassw.Password = v.Passw;
                        checkActive.IsChecked = v.Active;
                    }
                    txtFIO.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error55");
            }
        }
    }
}
