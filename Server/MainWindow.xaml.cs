﻿using Server.UserControllers;
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
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductUC obj = new ProductUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnTypeProduct_Click(object sender, RoutedEventArgs e)
        {
            TypeUC obj = new TypeUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnTypeMass_Click(object sender, RoutedEventArgs e)
        {
            MassUC obj = new MassUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnProvider_Click(object sender, RoutedEventArgs e)
        {
            ProviderUC obj = new ProviderUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnDebtor_Click(object sender, RoutedEventArgs e)
        {
            DebtHistoryUC obj = new DebtHistoryUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnIncome_Click(object sender, RoutedEventArgs e)
        {
            IncomeStatic obj = new IncomeStatic();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }


        private void btnStatistikaProduct_Click(object sender, RoutedEventArgs e)
        {
            StatistikaProductUC obj = new StatistikaProductUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow window = new ChangePasswordWindow();
            window.ShowDialog();
        }

        private void btnChangePartiya_Click(object sender, RoutedEventArgs e)
        {
            PartiyaInsideUC obj = new PartiyaInsideUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnVazvrat_Click(object sender, RoutedEventArgs e)
        {
            VazvratUC obj = new VazvratUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnSrokUtgan_Click(object sender, RoutedEventArgs e)
        {
            SrokUtganUC obj = new SrokUtganUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            About window = new About();
            window.ShowDialog();
        }

        private void btnSmena_Click(object sender, RoutedEventArgs e)
        {
            SmenaUC obj = new SmenaUC();
            gridForUC.Children.Clear();
            gridForUC.Children.Add(obj);
        }
    }
}
