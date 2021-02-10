using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
            try
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
                    txtSearch.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error52");
            }
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Error53");
            }

        }
    }
}
