using Server.Data;
using System;
using System.Linq;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Interaction logic for MassaChangeWindow.xaml
    /// </summary>
    public partial class MassaChangeWindow : Window
    {
        private int _id;

        public MassaChangeWindow(int Id)
        {
            InitializeComponent();
            _id = Id;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Massas
                             where a.Id == _id
                             select a).FirstOrDefault();
                    v.Name = txtSearch.Text;
                    context.SaveChanges();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Massas
                             where a.Id == _id
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        txtSearch.Text = v.Name;
                    }
                    txtSearch.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }
        }
    }
}
