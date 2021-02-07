using Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for TypeUC.xaml
    /// </summary>
    public partial class TypeUC : UserControl
    {
        public TypeUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
            txtSearch.Focus();
        }

        private void Refresh()
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Types
                         select a).ToList();
                DG_Type.ItemsSource = v;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text != "")
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Types
                             where a.TypeName == txtSearch.Text
                             select a).FirstOrDefault();
                    if (v == null)
                    {
                        Types t = new Types()
                        {
                            TypeName = txtSearch.Text
                        };
                        context.Types.Add(t);
                        context.SaveChanges();
                        txtSearch.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("У нас ест такое тип");
                        txtSearch.Text = "";
                    }
                    Refresh();
                }
            }

        }

        private void DG_Type_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DG_Type.SelectedIndex >= 0)
            {
                Types t = DG_Type.SelectedItem as Types;
                if (t.Id > 0)
                {
                    TypeChangeWindow tc = new TypeChangeWindow(t.Id);
                    tc.ShowDialog();
                    Refresh();
                }
            }

        }
    }
}
