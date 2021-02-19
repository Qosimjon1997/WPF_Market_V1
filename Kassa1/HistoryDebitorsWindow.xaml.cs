using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for HistoryDebitorsWindow.xaml
    /// </summary>
    public partial class HistoryDebitorsWindow : Window
    {
        private int _id;

        public HistoryDebitorsWindow(int Id)
        {
            InitializeComponent();
            _id = Id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        void Refresh()
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var c = (from a in context.Debts
                             .Include(x => x.DebtInfo)
                             where a.DebtInfoId == _id
                             select a).FirstOrDefault();
                    if (c != null)
                    {
                        labName.Text = c.DebtInfo.FIO;
                    }
                    else
                    {
                        MessageBox.Show("Этот должник не найден в базе");
                    }

                    var v = (from a in context.Debts
                             .Include(x => x.DebtInfo)
                             where a.DebtInfoId == _id
                             select a).ToList();
                    DG.ItemsSource = v;

                    labSumma.Text = v.Sum(x => x.Price).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error25");
            }
        }

        private void btnVazvrat_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(Math.Abs(Convert.ToDecimal(labSumma.Text))) != 0)
            {
                DebtWindow window = new DebtWindow(_id);
                window.ShowDialog();
                Refresh();
            }
            //MessageBox.Show(Convert.ToInt32(labSumma.Text).ToString());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
