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
using System.IO;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for WindowResult.xaml
    /// </summary>
    public partial class WindowResult : Window
    {
        Ranking ranking = new Ranking();

        public WindowResult()
        {
            InitializeComponent();
        }

        public WindowResult(Ranking ranking)
        {
            this.ranking = ranking;
            Ranking.columnHeaders = new String[ranking.n];
            Ranking.rowHeaders = new String[Ranking.m];

            for (int i = 1; i <= Ranking.m; i++)
            {
                Ranking.rowHeaders.Append(i.ToString());
            }
            Ranking.columnHeaders.Append("№");
            Ranking.columnHeaders.Append("Назва");
            for (int i = 1; i <= ranking.n; i++)
            {
                Ranking.columnHeaders.Append(i.ToString());
            }

            this.ranking.GenMatrixView();

            InitializeComponent();

            Cook cook = new Cook(ranking);
            cook.MinMax();

            var app = new Microsoft.Office.Interop.Excel.Application();
            var workbook = app.Workbooks.Open(Directory.GetCurrentDirectory() + cook.WorkbookPath);




            /*
            Hamming hamming = new Hamming();
            hamming.HammingDistance(Ranking.m, ranking.n, ranking.matrix_ranking);
            hamming.FindCompromiseRanking();
            */
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

        private void button_write_Click(object sender, RoutedEventArgs e)
        {
            if (output_file_path.Text.Length > 0)
            {
                ranking.WriteRatesMatrixToFile($"{output_file_path.Text}.xlsx");
            }
        }
    }
}
