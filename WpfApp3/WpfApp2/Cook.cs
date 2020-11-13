using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    class Cook : Metric
    {
        public Cook() : base()
        {

        }

        public Cook(Ranking ranking) : base(ranking)
        {

        }

        protected override void Distance()
        {
            for (int i = 0; i < Ranking.m; i++)
            {
                for (int j = i + 1; j < Ranking.m; j++)
                {
                    int key = Convert.ToInt32((i + 1).ToString() + (j + 1).ToString());
                    List<int> temp = new List<int>();

                    for (int k = 0; k < ranking.n; k++)
                    {
                        temp.Add(Math.Abs(ranking.matrix[i, k] - ranking.matrix[j, k]));
                        //Console.WriteLine("Distance i = " + i + ", j = " + j + ", k = " + k + ", ranking.matrix[i, k] = " + ranking.matrix[i, k] + ", ranking.matrix[j, k] = " + ranking.matrix[j, k] + ", x-y = " + (ranking.matrix[i, k] - ranking.matrix[j, k]));
                    }
                    //Console.WriteLine();

                    distances.Add(key, new DistanceRow(temp));
                }
            }
        }

        public override void WriteInitialDataToWorkbook(string workbook_path)
        {
            ranking.WriteRanksMatrixToFile(workbook_path);
        }

        protected override List<CompromiseRow> CalculateAllDistances()
        {
            foreach (List<int> p in Permutation.permutations)
            {
                List<int> distance_sum = new List<int>();

                for (int j = 0, sum = 0; j < ranking.n; j++, sum = 0)
                {
                    for (int i = 0; i < Ranking.m; i++)
                    {
                        sum += Math.Abs(ranking.matrix[i, j] - p.ElementAt(i));
                        //Console.WriteLine("AllDistances i = " + i + ", j = " + j + ", ranking.matrix[i, j] = " + ranking.matrix[i, j] + ", p.ElementAt(i) = " + p.ElementAt(i) + ", sum = " + sum);
                    }
                    distance_sum.Add(sum);
                    //Console.WriteLine();
                }

                all_distances.Add(new CompromiseRow(p, distance_sum));
            }

            return all_distances;
        }

        protected override void ReadInitialMatrix(string workbook_path)
        {
            ranking.ReadRanksMatrixFromFile(workbook_path);
        }

        protected override void ReadCompromisesFromWorksheet(Worksheet worksheet)
        {
            int rowcount = worksheet.Cells.Rows.Count;

            for (int i = 0, j = 0; i < rowcount; i++, j = 0)
            {
                List<int> temp_distances = new List<int>();
                List<int> temp_distance_sum = new List<int>();


                for (; j < Ranking.m; j++)
                {
                    temp_distances.Add(
                        Convert.ToInt32(
                        worksheet.Cells[CellsHelper.CellIndexToName(i, j)].Value));
                }

                for (int k = 0; k < ranking.n; k++, j++)
                {
                    temp_distance_sum.Add(
                        Convert.ToInt32(
                        worksheet.Cells[CellsHelper.CellIndexToName(i, j + 1)].Value));
                }

                compromise_distances.Add(new CompromiseRow(temp_distances, temp_distance_sum));
            }
        }

    }
}

