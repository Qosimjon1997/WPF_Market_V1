using Microsoft.EntityFrameworkCore;
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}
