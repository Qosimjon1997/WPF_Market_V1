using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for WorkerDebtWindow.xaml
    /// </summary>
    public partial class WorkerDebtWindow : Window
    {
        public WorkerDebtWindow()
        {
            InitializeComponent();
        }

        private void Refresh()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var v = (from a in context.Debts
                             .Include(x => x.DebtInfo)
                             where a.Price != 0
                             select new InfoDebt()
                             {
                                 Id = a.Id,
                                 FIO = a.DebtInfo.FIO,
                                 Address = a.DebtInfo.Address,
                                 Phone = a.DebtInfo.Phone,
                                 dateTimeFrom = a.dateTimeFrom,
                                 dateTimeUntil = a.dateTimeUntil,
                                 Price = a.Price
                             }).ToList();
                    DG_Debt.ItemsSource = v;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void DG_Debt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DG_Debt.SelectedCells.Count > 0 && DG_Debt.SelectedCells[0].IsValid)
                {
                    InfoDebt t = DG_Debt.SelectedItem as InfoDebt;
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        Debt v = (from a in context.Debts
                                     .Include(p => p.DebtInfo)
                                  where a.Id == t.Id
                                  select a).FirstOrDefault();

                        if (v != null)
                        {
                            DebtWindow window = new DebtWindow(v.Id);
                            window.ShowDialog();
                            Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Этот должник не найден в базе");
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Error23");
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
