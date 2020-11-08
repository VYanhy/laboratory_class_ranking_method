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
        Cook cookMetric;
        Hamming hammingMetric;

        Cook CookMetric
        {
            get
            {
                if (cookMetric == null)
                {
                    cookMetric = new Cook(ranking);
                }

                return cookMetric;
            }
            set { cookMetric = value; }
        }

        Hamming HammingMetric
        {
            get
            {
                if (hammingMetric == null)
                {
                    hammingMetric = new Hamming(ranking);
                }

                return hammingMetric;
            }
            set { hammingMetric = value; }
        }

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
                    output_file_path.Text = fbd.SelectedPath + "\\";
                }
            }
        }

        private void button_write_Click(object sender, RoutedEventArgs e)
        {
            if (output_file_path.Text.Length > 0)
            {
                if (ranking.WriteRanksMatrixToFile($"{output_file_path.Text}.xlsx"))
                {
                    System.Windows.MessageBox.Show("Файл " + $"{output_file_path.Text}.xlsx" + " був створений");
                }
            }
        }

        private void action_file_path_GotFocus(object sender, RoutedEventArgs e)
        {
            hintOptionallyForActionPath.Visibility = Visibility.Hidden;
        }

        private void action_file_path_LostFocus(object sender, RoutedEventArgs e)
        {
            if (action_file_path.Text.Length == 0)
            {
                hintOptionallyForActionPath.Visibility = Visibility.Visible;
            }
        }

        private void action_file_path_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (action_file_path.Text.Length == 0)
            {
                hintOptionallyForActionPath.Visibility = Visibility.Visible;
            }
        }

        private void action_file_path_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    action_file_path.Text = fbd.SelectedPath + "\\";
                }
            }
        }

        private void button_write_action_Click(object sender, RoutedEventArgs e)
        {
            string file_path = $"{action_file_path.Text}.xlsx";

            if ((bool)cook_distances.IsChecked)
            {
                CookMetric.SaveDistancesToWorkbook(file_path);
            }
            else
            if ((bool)cook_all.IsChecked)
            {
                CookMetric.SaveAllDistancesToWorkbook(file_path);
            }
            else
            if ((bool)cook_compromise_min_max.IsChecked)
            {
                CookMetric.SaveCompromisesToWorkbook(file_path);
            }
            else
            if ((bool)hamming_distances.IsChecked)
            {
                HammingMetric.SaveDistancesToWorkbook(file_path);
            }
            else
            if ((bool)hamming_all.IsChecked)
            {
                HammingMetric.SaveAllDistancesToWorkbook(file_path);
            }
            else
            if ((bool)hamming_compromise_min_max.IsChecked)
            {
                HammingMetric.SaveCompromisesToWorkbook(file_path);
            }
        }

    }
}
