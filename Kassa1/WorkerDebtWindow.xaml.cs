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
                             group a by new
                             {
                                 a.DebtInfoId,
                                 a.DebtInfo.FIO,
                                 a.DebtInfo.Address,
                                 a.DebtInfo.Phone,
                             } into gcs
                             select new InfoDebt()
                             {
                                 Id = gcs.Key.DebtInfoId,
                                 FIO = gcs.Key.FIO,
                                 Address = gcs.Key.Address,
                                 Phone = gcs.Key.Phone,
                                 Price = gcs.Sum(x => x.Price)
                             }).ToList();
                    DG_Debt.ItemsSource = v;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error24");
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
                            HistoryDebitorsWindow window = new HistoryDebitorsWindow(v.DebtInfoId);
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
