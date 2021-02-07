using Server.UserControllers;
using System.Windows;

namespace Server
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

        private void btnWorker_Click(object sender, RoutedEventArgs e)
        {
            WorkerUC obj = new WorkerUC();
            gridForUC.Children.Add(obj);
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductUC obj = new ProductUC();
            gridForUC.Children.Add(obj);
        }

        private void btnTypeProduct_Click(object sender, RoutedEventArgs e)
        {
            TypeUC obj = new TypeUC();
            gridForUC.Children.Add(obj);
        }

        private void btnTypeMass_Click(object sender, RoutedEventArgs e)
        {
            MassUC obj = new MassUC();
            gridForUC.Children.Add(obj);
        }

        private void btnProvider_Click(object sender, RoutedEventArgs e)
        {
            ProviderUC obj = new ProviderUC();
            gridForUC.Children.Add(obj);
        }

        private void btnDebtor_Click(object sender, RoutedEventArgs e)
        {
            DebtHistoryUC obj = new DebtHistoryUC();
            gridForUC.Children.Add(obj);
        }

        private void btnIncome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDiscount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnStatistikaProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChangePartiya_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnVazvrat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSrokUtgan_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
