using Kassa1.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for DebtWindow.xaml
    /// </summary>
    public partial class DebtWindow : Window
    {
        private int _id;

        public DebtWindow(int id)
        {
            InitializeComponent();
            _id = id;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            if(e.Key == Key.Enter)
            {
                downEnter();
            }
        }
        void downEnter()
        {
            try
            {
                if ((Convert.ToDecimal(txtPlastik.Text) + Convert.ToDecimal(txtNaxt.Text)) <= Math.Abs(Convert.ToDecimal(labSumm.Text)))
                {
                    using (ApplicationDBContext context = new ApplicationDBContext())
                    {
                        var v = (from a in context.Debts
                                 .Include(x => x.DebtInfo)
                                 where a.DebtInfoId == _id
                                 select a).FirstOrDefault();

                        Income i = new Income()
                        {
                            SaleProductPrice = 0,
                            CashIncome = Convert.ToDecimal(txtNaxt.Text),
                            DebtIncome = Convert.ToDecimal(txtNaxt.Text) + Convert.ToDecimal(txtPlastik.Text),
                            DebtOut = 0,
                            PlasticIncome = Convert.ToDecimal(txtPlastik.Text),
                            DateTimeNow = DateTime.Now,
                            WorkerId = Properties.Settings.Default.WorkerId,
                            Vozvrat = 0
                        };
                        context.Incomes.Add(i);
                        Debt d = new Debt()
                        {
                            Price = Convert.ToDecimal(txtNaxt.Text) + Convert.ToDecimal(txtPlastik.Text),
                            timeNow = DateTime.Now,
                            DebtInfoId = _id
                        };

                        context.Debts.Add(d);
                        context.SaveChanges();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка с сумма!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error12");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {

                    var v = (from a in context.Debts
                             .Include(x => x.DebtInfo)
                             where a.DebtInfoId == _id
                             select a).ToList();

                    labSumm.Text = v.Sum(x => x.Price).ToString();
                    txtNaxt.Text = (Convert.ToInt32(Math.Abs(Convert.ToDecimal(labSumm.Text)))).ToString();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error11");
            }
        }

        private void txtNaxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));

        }

        private void txtNaxt_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtNaxt.Text == "")
            {
                txtNaxt.Text = "0";
            }
        }

        private void txtPlastik_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtPlastik.Text == "")
            {
                txtPlastik.Text = "0";
            }
        }

        private void txtPlastik_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            downEnter();
        }


    }
}
