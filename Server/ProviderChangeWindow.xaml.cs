using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Server
{
    /// <summary>
    /// Interaction logic for ProviderChangeWindow.xaml
    /// </summary>
    public partial class ProviderChangeWindow : Window
    {
        private int _id;

        public ProviderChangeWindow(int Id)
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
                    var v = (from a in context.Providers
                             where a.Id == _id
                             select a).FirstOrDefault();
                    v.ProviderName = txtSearch.Text;
                    context.SaveChanges();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error47");
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
                    var v = (from a in context.Providers
                             where a.Id == _id
                             select a).FirstOrDefault();
                    if (v != null)
                    {
                        txtSearch.Text = v.ProviderName;
                    }
                    txtSearch.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error48");
            }
        }
    }
}
