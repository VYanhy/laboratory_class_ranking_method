using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for WindowManual.xaml
    /// </summary>
    public partial class WindowManual : Window
    {
        Ranking ranking = new Ranking();
        static List<string> filePaths = new List<string>();
        static int n;

        public WindowManual()
        {
            InitializeComponent();
        }

        private void output_file_path_GotFocus(object sender, RoutedEventArgs e)
        {
            hintOptionally.Visibility = Visibility.Hidden;
        }

        private void output_file_path_LostFocus(object sender, RoutedEventArgs e)
        {
            if (output_file_path.Text.Length == 0)
            {
                hintOptionally.Visibility = Visibility.Visible;
            }
        }

        private void output_file_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (output_file_path.Text.Length == 0)
            {
                hintOptionally.Visibility = Visibility.Visible;
            }
        }

        private void button_set_Click(object sender, RoutedEventArgs e)
        {
            n = Convert.ToInt32(expert_number.Text);
            ranking.n = n;
            filePaths = new List<string>(ranking.n);

            for (int i = 0; i < n; i++) 
            {
                OpenWindowExpert(i);
            }

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            FirstSteps();
            OpenResult();
        }

        private void button_write_Click(object sender, RoutedEventArgs e)
        {
            if (output_file_path.Text.Length > 0)
            {
                FirstSteps();
                ranking.WriteRatesMatrixToFile($"{output_file_path.Text}.xlsx");
            }
        }

        private void FirstSteps()
        {
            ranking.filePaths = filePaths;

            foreach (string f in filePaths)
            {
                Console.WriteLine("filePath" + f);
            }
            foreach (string f in ranking.filePaths)
            {
                Console.WriteLine("ranking.filePath" + f);
            }

            ranking.ReadFilePath();
            ranking.GenRatesMatrix();
        }

        private static void AddFilePath(string fileName)
        {
            Console.WriteLine("!!!" + fileName);
            filePaths.Add(fileName);
        }

        private void OpenWindowExpert(int i)
        {
            WindowManualExpert window = new WindowManualExpert(i);
            window.Notify += AddFilePath;
            window.ShowDialog();
        }

        private void OpenResult()
        {
            WindowResult windowResult = new WindowResult(ranking);
            windowResult.Show();
        }

        private void output_file_path_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    output_file_path.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
