using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    public class ComparisonMatrix
    {
        public static ProductCollection products = new ProductCollection();
        public static int m = products.Count;
        public static int n;

        public static String[] columnHeaders;
        public static String[] rowHeaders;
        public static int[,] matrix;

        public static string message_ranking;
        public static ChoiceRow[] choiceRows;


        public static void CalculateMatrix()
        {
            try
            {
                matrix = new int[n, n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == j)
                            matrix[i, j] = 0;
                        else if (choiceRows[i].Rate < choiceRows[j].Rate)
                            matrix[i, j] = 1;
                        else if (choiceRows[i].Rate > choiceRows[j].Rate)
                            matrix[i, j] = -1;

                        //Console.Write(matrix[i, j] + " ");
                    }
                    //Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public static void GenMessageRanking()
        {
            message_ranking = String.Format(
                "a{0}∈A, i∈I=1,...,n, n = {1}\ne{2}: ", "i".ToLower(), n, "1".ToLower()
                );

            var choiceRowsListSort = choiceRows.ToList();
            choiceRowsListSort.Sort((pair1, pair2) => pair1.Rate.CompareTo(pair2.Rate));

            for (int i = 0; i < n; i++)
            {
                if (i != 0)
                {
                    message_ranking += ">";
                }
                message_ranking += "a" + choiceRowsListSort[i].Product.ProductId.ToString();
            }
        }

        public static void WriteMatrixToFile(string file)
        {
            try
            {
                Workbook wb = new Workbook();
                Worksheet sheet = wb.Worksheets[0];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        sheet.Cells[CellsHelper.CellIndexToName(i, j)].PutValue(matrix[i, j]);
                    }
                }
                wb.Save(file, SaveFormat.Xlsx);
                MessageBox.Show(file + " був створений");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
