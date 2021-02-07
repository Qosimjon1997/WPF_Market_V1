using Server.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for ProviderUC.xaml
    /// </summary>
    public partial class ProviderUC : UserControl
    {
        public ProviderUC()
        {
            InitializeComponent();
        }
        private void Refresh()
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Providers
                         select a).ToList();
                DG_Provider.ItemsSource = v;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text != "")
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Providers
                             where a.ProviderName == txtSearch.Text
                             select a).FirstOrDefault();
                    if (v == null)
                    {
                        Provider t = new Provider()
                        {
                            ProviderName = txtSearch.Text
                        };
                        context.Providers.Add(t);
                        context.SaveChanges();
                        txtSearch.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("У нас ест такое поставщик");
                        txtSearch.Text = "";
                    }
                    Refresh();
                }
            }
        }

        private void DG_Provider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DG_Provider.SelectedIndex >= 0)
            {
                Provider t = DG_Provider.SelectedItem as Provider;
                if (t.Id > 0)
                {
                    ProviderChangeWindow tc = new ProviderChangeWindow(t.Id);
                    tc.ShowDialog();
                    Refresh();
                }
            }
        }
    }
}
