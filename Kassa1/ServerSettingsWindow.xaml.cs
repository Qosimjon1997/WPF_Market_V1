﻿using System;
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

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for ServerSettingsWindow.xaml
    /// </summary>
    public partial class ServerSettingsWindow : Window
    {
        public ServerSettingsWindow()
        {
            InitializeComponent();
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

        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}