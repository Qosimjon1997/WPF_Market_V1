using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtShtrix.Focus();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "1";
            txtShtrix.Focus();
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "2";
            txtShtrix.Focus();
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "3";
            txtShtrix.Focus();
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "4";
            txtShtrix.Focus();
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "5";
            txtShtrix.Focus();
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "6";
            txtShtrix.Focus();
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "7";
            txtShtrix.Focus();
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "8";
            txtShtrix.Focus();
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "9";
            txtShtrix.Focus();
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            txtShtrix.Text += "0";
            txtShtrix.Focus();
        }

        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            // Some Code
            //if((txtShtrix.Text).Length != 0)
            //{
            //    string A = txtShtrix.Text;
            //    A = 
            //}
            txtShtrix.Focus();
        }

        private void btnEnterNumber_Click(object sender, RoutedEventArgs e)
        {
            // Some Code
            pressEnter();
            txtShtrix.Focus();
        }

        void pressEnter()
        {

        }

        private void btnSale_Click(object sender, RoutedEventArgs e)
        {
            // Some Code
            txtShtrix.Focus();
        }

        private void btnDeleteKassa_Click(object sender, RoutedEventArgs e)
        {
            // Some Code
            txtShtrix.Focus();
        }

        private void btnProductNoBar_Click(object sender, RoutedEventArgs e)
        {
            ProductOfNoBarWindow window = new ProductOfNoBarWindow();
            window.ShowDialog();
            txtShtrix.Focus();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword window = new ChangePassword();
            window.ShowDialog();
            txtShtrix.Focus();
        }

        private void btnDebtors_Click(object sender, RoutedEventArgs e)
        {
            WorkerDebtWindow window = new WorkerDebtWindow();
            window.ShowDialog();
            txtShtrix.Focus();
        }

        private void txtShtrix_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
            txtShtrix.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                txtShtrix.Text = "";
                txtShtrix.Focus();
            }
            if(e.Key == Key.Enter)
            {
                pressEnter();
            }
        }
    }
}
