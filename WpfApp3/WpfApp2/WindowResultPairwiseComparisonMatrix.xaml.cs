using Aspose.Cells;
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
using MessageBox = System.Windows.MessageBox;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для WindowResultPairwiseComparisonMatrix.xaml
    /// </summary>
    public partial class WindowResultPairwiseComparisonMatrix : Window
    {
        ComparisonMatrix comparisonMatrix = new ComparisonMatrix();
        public static int n;
        public static string message_ranking;
        ChoiceRow[] choiceRows;

        public WindowResultPairwiseComparisonMatrix()
        {
            InitializeComponent();
        }

        private void filePathWrite_GotFocus(object sender, RoutedEventArgs e)
        {
            hintPath.Visibility = Visibility.Hidden;
        }

        private void filePathWrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (filePathWrite.Text.Length == 0)
            {
                hintPath.Visibility = Visibility.Visible;
            }
        }

        private void filePathWrite_LostFocus(object sender, RoutedEventArgs e)
        {
            if (filePathWrite.Text.Length == 0)
            {
                hintPath.Visibility = Visibility.Visible;
            }
        }

        private void fileNameWrite_GotFocus(object sender, RoutedEventArgs e)
        {
            hintName.Visibility = Visibility.Hidden;
        }

        private void fileNameWrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fileNameWrite.Text.Length == 0)
                hintName.Visibility = Visibility.Visible;
        }

        private void fileNameWrite_LostFocus(object sender, RoutedEventArgs e)
        {
            if (fileNameWrite.Text.Length == 0)
                hintName.Visibility = Visibility.Visible;
        }

        public WindowResultPairwiseComparisonMatrix(ChoiceRow[] choiceRows)
        {
            ComparisonMatrix.choiceRows = choiceRows;
            ComparisonMatrix.n = choiceRows.Length;
            ComparisonMatrix.CalculateMatrix();

            ComparisonMatrix.columnHeaders = new String[n];
            ComparisonMatrix.rowHeaders = new String[n];

            for (int i = 1; i <= n; i++)
            {
                ComparisonMatrix.columnHeaders.Append(i.ToString());
                ComparisonMatrix.rowHeaders.Append(i.ToString());
            }

            ComparisonMatrix.GenMessageRanking();

            InitializeComponent();
            textbox.Text = message_ranking;
        }

        

        private void Button_Click_WriteMatrixToFile(object sender, RoutedEventArgs e)
        {
            if (fileNameWrite.Text != "")
            {
                ComparisonMatrix.WriteMatrixToFile($"{filePathWrite.Text}\\{fileNameWrite.Text}.xlsx");
            }
        }

        private void filePathWrite_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    filePathWrite.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
