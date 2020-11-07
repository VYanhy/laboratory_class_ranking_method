using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp2
{
    abstract class Metric
    {
        public Ranking ranking;
        protected Dictionary<int, DistanceRow> distances = new Dictionary<int, DistanceRow>();
        protected List<CompromiseRow> all_distances = new List<CompromiseRow>();
        protected List<CompromiseRow> compromise_distances = new List<CompromiseRow>();
        protected int min;
        protected int max;

        public Dictionary<int, DistanceRow> Distances
        {
            get
            {
                if (distances.Count == 0)
                {
                    Distance();
                }

                return distances;
            }
        }

        public List<CompromiseRow> AllDistances
        {
            get
            {
                if (all_distances.Count == 0)
                {
                    MinMax();
                }

                return all_distances;
            }
        }

        public List<CompromiseRow> CompromiseDistances
        {
            get
            {
                if (compromise_distances.Count == 0)
                {
                    MinMax();
                }

                return compromise_distances;
            }
        }

        public Metric()
        {

        }

        public Metric(Ranking ranking)
        {
            this.ranking = ranking;
        }

        protected abstract void Distance();

        public abstract void WriteInitialDataToWorkbook(string workbook_path);

        protected abstract List<CompromiseRow> CalculateAllDistances();

        protected void FindCompromiseRows()
        {
            if (Permutation.permutations == null)
            {
                Permutation.CalculatePermutations(Ranking.m);
            }

            CalculateAllDistances();
        }

        public List<CompromiseRow> MinMax()
        {
            FindCompromiseRows();

            min = all_distances.Min(x => x.sum);
            foreach (CompromiseRow c in all_distances)
            {
                if (c.sum == min)
                {
                    compromise_distances.Add(c);
                }
            }
            max = compromise_distances.Max(x => x.max);

            return compromise_distances;
        }

        public void SaveDistancesToWorkbook(string workbook_path)
        {
            WriteInitialDataToWorkbook(workbook_path);
            Workbook workbook = new Workbook(workbook_path);
            Worksheet sheet1 = workbook.Worksheets[workbook.Worksheets.Add()];

            Worksheet sheet = sheet1;
            for (int i = 0, j = 0; i < Distances.Count; i++, j = 0)
            {
                sheet.Cells[CellsHelper.CellIndexToName(i, 0)].PutValue(Distances.ElementAt(i).Key);

                for (; j < ranking.n; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(i, j + 1)].PutValue(Distances.ElementAt(i).Value.distance.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(i, j + 1)].PutValue(Distances.ElementAt(i).Value.sum);
            }

            workbook.Save(workbook_path, SaveFormat.Xlsx);
            MessageBox.Show("Файл " + workbook_path + " був створений");
        }

        public void SaveAllDistancesToWorkbook(string workbook_path)
        {
            WriteInitialDataToWorkbook(workbook_path);
            Workbook workbook = new Workbook(workbook_path);
            Worksheet sheet1 = workbook.Worksheets[workbook.Worksheets.Add()];

            Worksheet sheet = sheet1;
            for (int i = 0, j = 0, filling = 0; i < AllDistances.Count; i++, j = 0, filling++)
            {
                if (filling == 1048575)
                {
                    filling = 0;
                    var worksheet_num = workbook.Worksheets.Add();
                    sheet = workbook.Worksheets[worksheet_num];
                }

                for (; j < Ranking.m; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(filling, j)].PutValue(AllDistances.ElementAt(i).distance.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 1)].PutValue(AllDistances.ElementAt(i).sum);
                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 2)].PutValue(AllDistances.ElementAt(i).max);
            }

            workbook.Save(workbook_path, SaveFormat.Xlsx);
            MessageBox.Show("Файл " + workbook_path + " був створений");
        }

        public void SaveCompromisesToWorkbook(string workbook_path)
        {
            WriteInitialDataToWorkbook(workbook_path);
            Workbook workbook = new Workbook(workbook_path);
            Worksheet sheet1 = workbook.Worksheets[workbook.Worksheets.Add()];

            Worksheet sheet = sheet1;
            for (int i = 0, j = 0, filling = 0; i < CompromiseDistances.Count; i++, j = 0, filling++)
            {
                if (filling == 1048575)
                {
                    filling = 0;
                    var worksheet_num = workbook.Worksheets.Add();
                    sheet = workbook.Worksheets[worksheet_num];
                }

                for (; j < Ranking.m; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(filling, j)].PutValue(CompromiseDistances.ElementAt(i).distance.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 1)].PutValue(CompromiseDistances.ElementAt(i).sum);
                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 2)].PutValue(CompromiseDistances.ElementAt(i).max);
            }

            workbook.Save(workbook_path, SaveFormat.Xlsx);
            MessageBox.Show("Файл " + workbook_path + " був створений");
        }

    }
}
