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

                        if (ranking.matrix[i, k] < ranking.matrix[j, k])
                            val = 1;
                        else
                            val = -1;

                        temp.Add(val);
                    }

                    distances.Add(key, new DistanceRow(temp));
                }
            }
        }

        public override void WriteInitialDataToWorkbook(string workbook_path)
        {
            ranking.WriteRankingsMatrixToFile(workbook_path);
        }

        protected override List<CompromiseRow> CalculateAllDistances()
        {
            Distance();

            foreach (List<int> p in Permutation.permutations)
            {
                List<int> temp = GetRankingVector(p);
                List<int> distance_sum = new List<int>();

                for (int i = 0, sum = 0; i < ranking.n; i++, sum = 0)
                {
                    for (int j = 0; j < distances.Count; j++)
                    {
                        sum += Math.Abs(distances.ElementAt(j).Value.distance.ElementAt(i) - temp.ElementAt(j));
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

    }
}
