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
                    }

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

                for (int i = 0, sum = 0; i < Ranking.m; i++, sum = 0)
                {
                    for (int j = 0; j < ranking.n; j++)
                    {
                        sum += Math.Abs(ranking.matrix[i, j] - p.ElementAt(i));
                    }

                    distance_sum.Add(sum);
                }

                all_distances.Add(new CompromiseRow(p, distance_sum));
            }

            return all_distances;
        }

    }
}

