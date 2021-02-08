using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for MassUC.xaml
    /// </summary>
    public partial class MassUC : UserControl
    {
        public MassUC()
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
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Massas
                             select a).ToList();
                    DG_MassaType.ItemsSource = v;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text != "")
            {
                try
                {
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        var v = (from a in context.Massas
                                 where a.Name == txtSearch.Text
                                 select a).FirstOrDefault();
                        if (v == null)
                        {
                            Massa massa = new Massa()
                            {
                                Name = txtSearch.Text
                            };
                            context.Massas.Add(massa);
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
                catch (Exception ex)
                {
                    MessageBox.Show("Error");
                }
            }
        }

        private void DG_MassaType_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DG_MassaType.SelectedIndex >= 0)
            {
                Massa m = DG_MassaType.SelectedItem as Massa;
                if (m.Id > 0)
                {
                    MassaChangeWindow mc = new MassaChangeWindow(m.Id);
                    mc.ShowDialog();
                    Refresh();
                }
            }
        }
    }
}
