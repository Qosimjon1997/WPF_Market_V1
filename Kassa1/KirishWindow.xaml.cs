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

namespace Kassa1
{
    /// <summary>
    /// Interaction logic for KirishWindow.xaml
    /// </summary>
    public partial class KirishWindow : Window
    {
        public KirishWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMenuSerringsServer_Click(object sender, RoutedEventArgs e)
        {
            ServerSettingsWindow window = new ServerSettingsWindow();
            window.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
