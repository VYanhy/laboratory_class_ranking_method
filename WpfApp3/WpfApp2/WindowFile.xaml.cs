using System;
using System.Collections.Generic;
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
using Aspose.Cells;
using Microsoft.Win32;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for WindowFile.xaml
    /// </summary>
    public partial class WindowFile : Window
    {
        Ranking ranking = new Ranking();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        int openFileDialogCount = 0;

        public WindowFile()
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Steps();
            OpenResult();
        }

        private void button_write_Click(object sender, RoutedEventArgs e)
        {
            if (output_file_path.Text.Length > 0)
            {
                Steps();
                ranking.WriteRanksMatrixToFile($"{output_file_path.Text}.xlsx");
            }
        }

        private void Steps()
        {
            GetFilePath();
            ranking.ReadFilePath();
            ranking.GenRatesMatrix();
        }

        private void GetFilePath()
        {
            try
            {
                ranking.n = Convert.ToInt32(expert_number.Text);
                ranking.filePaths = new List<string>(ranking.n);

                string temp = input_file_path.Text;
                for (int i = 0; i < ranking.n; i++)
                {
                    int pos = temp.IndexOf(';');

                    if (pos == -1)
                    {
                        ranking.filePaths.Add(temp);
                    }
                    else
                    {
                        ranking.filePaths.Add(temp.Substring(0, pos));
                        temp = temp.Remove(0, pos + 1);

                        if (temp.Length > 0)
                        {
                            while (temp.ElementAt(0) == ' ')
                                temp.Remove(0);
                        }


                    }
                    Console.WriteLine(ranking.filePaths[i]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenResult()
        {
            WindowResult windowResult = new WindowResult(ranking);
            windowResult.Show();
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
