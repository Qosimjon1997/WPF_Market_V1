using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for WorkerUC.xaml
    /// </summary>
    public partial class WorkerUC : UserControl
    {
        public WorkerUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Workers
                         select a).ToList();
                DG_Worker.ItemsSource = v;
            }
        }

        private void DG_Worker_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DG_Worker.SelectedIndex >= 0)
            {
                Worker t = DG_Worker.SelectedItem as Worker;
                if (t.Id > 0)
                {
                    WorkerChangeStatusWindow tc = new WorkerChangeStatusWindow(t.Id);
                    tc.ShowDialog();
                    Refresh();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtFIO.Text != "" && txtLogin.Text != "" && txtPassw.Password != "")
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Workers
                             where a.Login == txtLogin.Text && a.FIO == txtFIO.Text
                             select a).FirstOrDefault();
                    if (v == null)
                    {
                        Worker w = new Worker()
                        {
                            FIO = txtFIO.Text,
                            Login = txtLogin.Text,
                            Passw = txtPassw.Password,
                            Active = Convert.ToBoolean(checkActive.IsChecked)
                        };
                        context.Workers.Add(w);
                        context.SaveChanges();
                        txtFIO.Text = txtLogin.Text = txtPassw.Password = "";
                        Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Эта логин уже существует");
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
