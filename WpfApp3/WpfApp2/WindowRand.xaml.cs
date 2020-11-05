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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for WindowRand.xaml
    /// </summary>
    public partial class WindowRand : Window
    {
        Ranking ranking = new Ranking();
        Random rand = new Random();

        public WindowRand()
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
                ranking.WriteRatesMatrixToFile($"{output_file_path.Text}.xlsx");
            }
        }

        private void Steps()
        {
            ranking.n = Convert.ToInt32(expert_number.Text);

            Appraise();
            ranking.GenRatesMatrix();
        }
        private void Appraise()
        {
            ChoiceRow[] marks_by_expert;
            ranking.expertsChoiceRows = new Dictionary<int, ChoiceRow[]>(ranking.n);

            for (int j = 0; j < ranking.n; j++)
            {
                marks_by_expert = new ChoiceRow[Ranking.m];

                for (int i = 0; i < Ranking.m; i++)
                {
                    marks_by_expert[i] = new ChoiceRow(Ranking.products.ElementAt(i), rand.Next(101));
                    //Console.WriteLine("In for id:" + marks_by_expert[i].Product.ProductId + ", name:" + marks_by_expert[i].Product.ProductName + ", rate: " + marks_by_expert[i].Rate);
                }

                var list_marks_sort = marks_by_expert.ToList();
                list_marks_sort.Sort((pair1, pair2) => pair1.Rate.CompareTo(pair2.Rate));
                list_marks_sort.Reverse();

                int k = 1;
                foreach (ChoiceRow c in list_marks_sort)
                {
                    c.Rate = k;
                    k++;
                    //Console.WriteLine("In foreach id:" + c.Product.ProductId + ", name:" + c.Product.ProductName + ", rate: " + c.Rate);
                }

                list_marks_sort.Sort((pair1, pair2) => pair1.Product.ProductId.CompareTo(pair2.Product.ProductId));
                list_marks_sort.Reverse();

                ranking.expertsChoiceRows.Add(j, list_marks_sort.ToArray());
            }
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
