using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for SrokUtganUC.xaml
    /// </summary>
    public partial class SrokUtganUC : UserControl
    {
        public SrokUtganUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            date1.SelectedDate = DateTime.Today;
            date2.SelectedDate = DateTime.Today;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.InvalidGoods
                             where date1.SelectedDate <= a.Sana && a.Sana <= second
                             select a).ToList();

                    var SUMMA = (from a in context.InvalidGoods
                                 where date1.SelectedDate <= a.Sana && a.Sana <= second
                                 select a.BuyPrice).Sum();
                    labSumma.Text = SUMMA.ToString();

                    DG.ItemsSource = v;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error22");
            }
        }

    }
}
