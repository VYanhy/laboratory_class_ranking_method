using Microsoft.Win32;
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
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для WindowFileFullMatrix.xaml
    /// </summary>
    public partial class WindowFileFullMatrix : Window
    {
        Ranking ranking = new Ranking();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        int openFileDialogCount = 0;

        public WindowFileFullMatrix()
        {
            InitializeComponent();
        }

        private void input_file_path_GotFocus(object sender, RoutedEventArgs e)
        {
            hintPath.Visibility = Visibility.Hidden;

        }

        private void input_file_path_LostFocus(object sender, RoutedEventArgs e)
        {
            if (input_file_path.Text.Length == 0)
            {
                hintPath.Visibility = Visibility.Visible;
            }
        }

        private void input_file_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (input_file_path.Text.Length == 0)
            {
                hintPath.Visibility = Visibility.Visible;
            }
        }

        private void input_file_path_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            openFileDialog.Multiselect = true;
            //openFileDialog.Filter = "Excel files (*.xlsx)";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    input_file_path.Text += filename + ";";
                }
                openFileDialogCount += openFileDialog.FileNames.Length;
                expert_number.Text = openFileDialogCount.ToString();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Read();

            WindowResult windowResult = new WindowResult(ranking);
            windowResult.Show();
        }

        private void Read()
        {
            ranking.n = Convert.ToInt32(expert_number.Text);
            ranking.ReadRatesMatrixFromFile(input_file_path.Text);
        }

        private void button_coef_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
