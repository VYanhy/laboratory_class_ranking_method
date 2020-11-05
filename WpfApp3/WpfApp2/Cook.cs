using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class Cook
    {
        Ranking ranking;
        Dictionary<int, CookDistanceRow> distances = new Dictionary<int, CookDistanceRow>();
        List<CalculationCompromiseRow> all_distances = new List<CalculationCompromiseRow>();
        List<CalculationCompromiseRow> compromise_distances = new List<CalculationCompromiseRow>();
        public int min;
        public int max;

        Workbook workbook;
        string workbook_path = "";

        public string WorkbookPath
        {
            get
            {
                if (workbook_path == "")
                {
                    FillWorkbook();
                }
                return workbook_path;
            }
        }

        public Cook()
        {

        }

        public Cook(Ranking ranking)
        {
            this.ranking = ranking;
        }

        public void CooksDistance()
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
//                        Console.WriteLine(ranking.matrix[i, k] + "-" + ranking.matrix[j, k]);
                    }

                    distances.Add(key, new CookDistanceRow(temp));
                }
            }
        }

        public List<CalculationCompromiseRow> MinMax()
        {
            FindCompromiseRankings();

            min = all_distances.Min(x => x.sum);
            foreach (CalculationCompromiseRow c in all_distances)
            {
                if (c.sum == min)
                {
                    compromise_distances.Add(c);
                }
            }
            max = compromise_distances.Max(x => x.max);

            return compromise_distances;
        }

        private void FindCompromiseRankings()
        {
            if (Permutation.permutations == null)
            {
                Permutation.CalculatePermutations(Ranking.m);
            }

            CalculateAllDistances();
        }

        private List<CalculationCompromiseRow> CalculateAllDistances()
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

                all_distances.Add(new CalculationCompromiseRow(p, distance_sum));
            }

            return all_distances;
        }

        private void FillWorkbook()
        {
            int num_worksheet = 0;
            workbook = new Workbook();
            Worksheet sheet1 = workbook.Worksheets[num_worksheet];
            Worksheet sheet = sheet1;

            for (int i = 0, j = 0, filling = 0; i < all_distances.Count; i++, j = 0, filling++)
            {
                if (filling == 1048575)
                {
                    filling = 0;
                    num_worksheet++;
                    sheet = workbook.Worksheets[num_worksheet];
                }

                for (; j < Ranking.m; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(i, j)].PutValue(all_distances.ElementAt(i).distance.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(i, j + 1)].PutValue(all_distances.ElementAt(i).sum);
                sheet.Cells[CellsHelper.CellIndexToName(i, j + 2)].PutValue(all_distances.ElementAt(i).max);
            }

            workbook_path = "cook.xlsx";
            workbook.Save(workbook_path, SaveFormat.Xlsx);
        }


        class CookDistanceRow
        {
            public List<int> distance;
            public int sum;

            public CookDistanceRow()
            {

            }

            public CookDistanceRow(List<int> distance)
            {
                this.distance = distance;
                sum = distance.Sum();
            }
        }
    }

}

