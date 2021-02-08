using ClosedXML.Excel;
using Microsoft.Win32;
using Server.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                MessageBox.Show("Error");
            }
        }

    }
}
