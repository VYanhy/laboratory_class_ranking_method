using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    class Hamming : Metric
    {
        public Hamming() : base()
        {

        }

        public Hamming(Ranking ranking) : base(ranking)
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
                        int val;

                        if (ranking.MatrixRanking[i, k] < ranking.MatrixRanking[j, k])
                            val = 1;
                        else
                            val = -1;

                        temp.Add(val);
                    }

                    distances.Add(key, new DistanceRow(temp));
                }
            }
        }

        public override void SaveInitialDataToWorkbook(string workbook_path)
        {
            ranking.WriteRankingsMatrixToFile(workbook_path);
        }

        protected override List<CompromiseRow> CalculateAllDistances()
        {
            //Distance();

            foreach (List<int> p in Permutation.permutations)
            {
                List<int> temp = GetRankingVector(p);
                List<int> distance_sum = new List<int>();

                for (int i = 0, sum = 0; i < ranking.n; i++, sum = 0)
                {
                    for (int j = 0; j < Distances.Count; j++)
                    {
                        sum += Math.Abs(Distances.ElementAt(j).Value.distance.ElementAt(i) - temp.ElementAt(j));
                    }

                    distance_sum.Add(sum);
                }

                all_distances.Add(new CompromiseRow(temp, distance_sum));
            }

            return all_distances;
        }

        private List<int> GetRankingVector(List<int> permutation)
        {
            List<int> temp = new List<int>();

            for (int i = 0; i < Ranking.m; i++)
            {
                for (int j = i + 1; j < Ranking.m; j++)
                {
                    //int key = Convert.ToInt32((i + 1).ToString() + (j + 1).ToString());
                    int val;

                    if (permutation.ElementAt(i) < permutation.ElementAt(j))
                        val = 1;
                    else
                        val = -1;

                    temp.Add(val);
                }
            }

            return temp;
        }

        protected override void ReadInitialMatrix(string workbook_path)
        {
            ranking.ReadRankingMatrixFromFile(workbook_path);
        }

        protected override void ReadCompromisesFromWorksheet(Worksheet worksheet)
        {
            int rowcount = worksheet.Cells.Rows.Count;
            int m = Ranking.m;
            int colcount = (m * m - m) / 2;

            for (int i = 0, j = 0; i < rowcount; i++, j = 0)
            {
                List<int> temp_distances = new List<int>();
                List<int> temp_distance_sum = new List<int>();


                for (; j < colcount; j++)
                {
                    temp_distances.Add(
                        Convert.ToInt32(
                        worksheet.Cells[CellsHelper.CellIndexToName(i, j)].Value));
                }

                for (; j < ranking.n; j++)
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
