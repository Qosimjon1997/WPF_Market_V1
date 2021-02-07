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
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for TypeChangeWindow.xaml
    /// </summary>
    public partial class TypeChangeWindow : Window
    {
        private int _id;
        public TypeChangeWindow(int Id)
        {
            InitializeComponent();
            _id = Id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Types
                         where a.Id == _id
                         select a).FirstOrDefault();
                if (v != null)
                {
                    txtSearch.Text = v.TypeName;
                }
            }
            txtSearch.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                this.Close();
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var v = (from a in context.Types
                         where a.Id == _id
                         select a).FirstOrDefault();
                v.TypeName = txtSearch.Text;
                context.SaveChanges();
                this.Close();
            }

        }
    }
}
