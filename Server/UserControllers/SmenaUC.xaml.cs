using ClosedXML.Excel;
using Microsoft.Win32;
using Server.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for SmenaUC.xaml
    /// </summary>
    public partial class SmenaUC : UserControl
    {
        public SmenaUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            date1.SelectedDate = DateTime.Today;
            date2.SelectedDate = DateTime.Today;
            try
            {
                using(ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Workers
                             select a).ToList();
                    foreach (var a in v)
                    {
                        combTypeDebt.Items.Add(a.FIO);
                    }
                }
                combTypeDebt.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error19");
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var w = (from a in context.Workers
                             where a.FIO == combTypeDebt.SelectedItem
                             select a).FirstOrDefault();
                    var v = (from a in context.Smenas
                             where date1.SelectedDate <= a.Started && a.Finished <= second && a.WorkerId == w.Id
                             select a).ToList();
                    DG.ItemsSource = v;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error20");
            }

        }
    }
}
