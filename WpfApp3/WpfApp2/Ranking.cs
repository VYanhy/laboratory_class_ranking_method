using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    public class Ranking
    {
        public static ProductCollection products = new ProductCollection();

        public static int m = products.Count;
        public int n;

        public static String[] columnHeaders;
        public static String[] rowHeaders;
        public int[,] matrix;
        public int[,] matrix_ranking;
        public int[,] MatrixRanking
        {
            get
            {
                if (matrix_ranking == null)
                {
                    matrix_ranking = new int[m, n];

                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            matrix_ranking[matrix[i, j] - 1, j] = i + 1;
                        }
                    }

                }
                return matrix_ranking;
            }
        }

        public static string[,] matrix_view;

        public Dictionary<int, ChoiceRow[]> expertsChoiceRows;
        public List<string> filePaths;

        public void GenRatesMatrix()
        {
            matrix = new int[m, n];
            matrix_ranking = new int[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = expertsChoiceRows[j][i].Rate;
                    matrix_ranking[matrix[i, j] - 1, j] = i + 1;
                }
            }
        }

        public void GenMatrixView()
        {
            matrix_view = new string[m + 1, n + 2];

            matrix_view[0, 0] = "№";
            matrix_view[0, 1] = "Назва";

            for (int j = 0; j < n; j++)
            {
                matrix_view[0, j + 2] = (j + 1).ToString();
            }

            for (int i = 0; i < m; i++)
            {
                matrix_view[i + 1, 0] = products[i].ProductId.ToString();
                matrix_view[i + 1, 1] = products[i].ProductName;
            }

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix_view[i + 1, j + 2] = matrix[i, j].ToString();
                }
            }
        }

        public void ReadFilePath()
        {
            expertsChoiceRows = new Dictionary<int, ChoiceRow[]>(n);

            for (int i = 0; i < n; i++)
            {
                expertsChoiceRows.Add(i, ReadExpertRanks(filePaths.ElementAt(i)));
            }
        }

        public bool WriteRanksMatrixToFile(string file)
        {
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

            sheet.Cells[CellsHelper.CellIndexToName(0, 0)].PutValue("№");
            sheet.Cells[CellsHelper.CellIndexToName(0, 1)].PutValue("Назва");

            for (int j = 0; j < n; j++)
            {
                sheet.Cells[CellsHelper.CellIndexToName(0, j + 2)].PutValue("Експерт " + (j + 1).ToString());
            }

            ProductCollection products = new ProductCollection();
            for (int i = 0; i < m; i++)
            {
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 0)].PutValue(products.ElementAt(i).ProductId);
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 1)].PutValue(products.ElementAt(i).ProductName);

                for (int j = 0; j < n; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(i + 1, j + 2)].PutValue(matrix[i, j].ToString());
                }
            }

            wb.Save(file, SaveFormat.Xlsx);

            return true;
            //MessageBox.Show("Файл " + file + " був створений");
        }

        public bool WriteRankingsMatrixToFile(string file)
        {
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

            sheet.Cells[CellsHelper.CellIndexToName(0, 0)].PutValue("№");
            for (int j = 0; j < n; j++)
            {
                sheet.Cells[CellsHelper.CellIndexToName(0, j + 1)].PutValue("Експерт " + (j + 1).ToString());
            }

            for (int i = 0; i < m; i++)
            {
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 0)].PutValue((i + 1).ToString());

                for (int j = 0; j < n; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(i + 1, j + 1)].PutValue(MatrixRanking[i, j].ToString());
                }
            }

            int row = m + 2;

            sheet.Cells[CellsHelper.CellIndexToName(row, 0)].PutValue("№");
            sheet.Cells[CellsHelper.CellIndexToName(row, 1)].PutValue("Назва");

            ProductCollection products = new ProductCollection();
            for (int i = 0; i < m; i++, row++)
            {
                sheet.Cells[CellsHelper.CellIndexToName(row, 0)].PutValue(products.ElementAt(i).ProductId);
                sheet.Cells[CellsHelper.CellIndexToName(row, 1)].PutValue(products.ElementAt(i).ProductName);
            }

            wb.Save(file, SaveFormat.Xlsx);
            return true;
            //MessageBox.Show("Файл " + file + " був створений");
        }

        public void ReadRanksMatrixFromFile(string file)
        {
            LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);
            Workbook wb = new Workbook(file, loadOptions);
            Worksheet sheet = wb.Worksheets[0];

            matrix = new int[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Convert.ToInt32(sheet.Cells[CellsHelper.CellIndexToName(i + 1, j + 2)].Value);
                }
            }
        }

        public void ReadRankingMatrixFromFile(string file)
        {
            LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);
            Workbook wb = new Workbook(file, loadOptions);
            Worksheet sheet = wb.Worksheets[0];

            matrix = new int[m, n];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix_ranking[i, j] = Convert.ToInt32(sheet.Cells[CellsHelper.CellIndexToName(i + 1, j + 2)].Value);
                }
            }
        }

        public static ChoiceRow[] ReadExpertRanks(string file)
        {
            LoadOptions loadOptions = new LoadOptions(LoadFormat.Xlsx);
            Workbook wb = new Workbook(file, loadOptions);
            Worksheet sheet = wb.Worksheets[0];

            ChoiceRow[] choiceRows = new ChoiceRow[m];

            for (int i = 0; i < m; i++)
            {
                choiceRows[i] = new ChoiceRow(
                    products.ElementAt(i),
                    Convert.ToInt32(sheet.Cells[CellsHelper.CellIndexToName(i + 1, 2)].Value)
                    );
            }

            return choiceRows;
        }

        public static void WriteExpertRanks(string file, ChoiceRow[] choiceRows)
        {
            Workbook wb = new Workbook();
            Worksheet sheet = wb.Worksheets[0];

            sheet.Cells[CellsHelper.CellIndexToName(0, 0)].PutValue("ID");
            sheet.Cells[CellsHelper.CellIndexToName(0, 1)].PutValue("Name");
            sheet.Cells[CellsHelper.CellIndexToName(0, 2)].PutValue("Rate");

            for (int i = 0; i < m; i++)
            {
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 0)].PutValue(choiceRows[i].Product.ProductId);
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 1)].PutValue(choiceRows[i].Product.ProductName);
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 2)].PutValue(choiceRows[i].Rate);
            }
            wb.Save(file, SaveFormat.Xlsx);

            MessageBox.Show("Файл " + file + " був створений");
        }
    }
}
