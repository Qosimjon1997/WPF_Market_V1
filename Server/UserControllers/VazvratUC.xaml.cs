using Microsoft.EntityFrameworkCore;
using Server.Data;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Server.UserControllers
{
    /// <summary>
    /// Interaction logic for VazvratUC.xaml
    /// </summary>
    public partial class VazvratUC : UserControl
    {
        public VazvratUC()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Workers
                             select a).ToList();
                    combWorker.Items.Add("Все");
                    combWorker.SelectedIndex = 0;
                    foreach (Worker w in v)
                    {
                        combWorker.Items.Add(w.FIO);
                    }
                }
                date1.SelectedDate = DateTime.Today;
                date2.SelectedDate = DateTime.Today;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error27");
            }
        }

        void RefreshDG()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    if (combWorker.SelectedItem != null && date1.SelectedDate.ToString() != "" && date2.SelectedDate.ToString() != "")
                    {
                        DateTime second = new DateTime(Convert.ToInt32(date2.SelectedDate.Value.ToString("yyyy")), Convert.ToInt32(date2.SelectedDate.Value.ToString("MM")), Convert.ToInt32(date2.SelectedDate.Value.ToString("dd")), 23, 59, 59);
                        if (combWorker.SelectedItem.ToString() == "Все")
                        {
                            var v = (from a in context.Solds
                                     .Include(x => x.Worker)
                                     where a.dateTimeNow >= date1.SelectedDate && a.dateTimeNow <= second
                                     select a).ToList();
                            DG_Sold.ItemsSource = v;

                        }
                        else
                        {
                            var v = (from a in context.Solds
                                     .Include(x => x.Worker)
                                     where a.dateTimeNow >= date1.SelectedDate && a.dateTimeNow <= date2.SelectedDate && a.Worker.FIO == combWorker.SelectedItem.ToString()
                                     select a).ToList();
                            DG_Sold.ItemsSource = v;

                        }

                    }
                    else
                    {
                        MessageBox.Show("Ошибка");

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error28");
            }
        }


        private void DG_Sold_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DG_Sold.SelectedCells.Count > 0 && DG_Sold.SelectedCells[0].IsValid)
                {
                    Sold t = DG_Sold.SelectedItem as Sold;
                    DeleteSold window = new DeleteSold(t.Id);
                    window.ShowDialog();
                    RefreshDG();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error29");
            }

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            RefreshDG();
        }
    }
}
