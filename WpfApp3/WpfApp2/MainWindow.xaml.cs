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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_rand_Click(object sender, RoutedEventArgs e)
        {
            WindowRand windowRand = new WindowRand();
            windowRand.Show();
        }

        private void button_file_Click(object sender, RoutedEventArgs e)
        {
            WindowFile windowFile = new WindowFile();
            windowFile.Show();
        }

        private void button_manual_Click(object sender, RoutedEventArgs e)
        {
            WindowManual windowManual = new WindowManual();
            windowManual.Show();
        }

    }
}
