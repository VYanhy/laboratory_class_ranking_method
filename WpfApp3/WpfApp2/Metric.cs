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
        protected Ranking ranking;
        protected Dictionary<int, DistanceRow> distances = new Dictionary<int, DistanceRow>();
        protected List<CompromiseRow> all_distances = new List<CompromiseRow>();
        protected List<CompromiseRow> compromise_distances = new List<CompromiseRow>();
        protected int min;
        protected int max;

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
            if (distances.Count == 0)
            {
                Distance();
            }

            WriteInitialDataToWorkbook(workbook_path);
            Workbook workbook = new Workbook(workbook_path);
            Worksheet sheet1 = workbook.Worksheets[workbook.Worksheets.Add()];

            Worksheet sheet = sheet1;
            for (int i = 0, j = 0; i < distances.Count; i++, j = 0)
            {
                sheet.Cells[CellsHelper.CellIndexToName(i, 0)].PutValue(distances.ElementAt(i).Key);

                for (; j < ranking.n; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(i, j + 1)].PutValue(distances.ElementAt(i).Value.distance.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(i, j + 1)].PutValue(distances.ElementAt(i).Value.sum);
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
            for (int i = 0, j = 0, filling = 0; i < all_distances.Count; i++, j = 0, filling++)
            {
                if (filling == 1048575)
                {
                    filling = 0;
                    var worksheet_num = workbook.Worksheets.Add();
                    sheet = workbook.Worksheets[worksheet_num];
                }

                for (; j < Ranking.m; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(filling, j)].PutValue(all_distances.ElementAt(i).distance.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 1)].PutValue(all_distances.ElementAt(i).sum);
                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 2)].PutValue(all_distances.ElementAt(i).max);
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
            for (int i = 0, j = 0, filling = 0; i < compromise_distances.Count; i++, j = 0, filling++)
            {
                if (filling == 1048575)
                {
                    filling = 0;
                    var worksheet_num = workbook.Worksheets.Add();
                    sheet = workbook.Worksheets[worksheet_num];
                }

                for (; j < Ranking.m; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(filling, j)].PutValue(compromise_distances.ElementAt(i).distance.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 1)].PutValue(compromise_distances.ElementAt(i).sum);
                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 2)].PutValue(compromise_distances.ElementAt(i).max);
            }

            workbook.Save(workbook_path, SaveFormat.Xlsx);
            MessageBox.Show("Файл " + workbook_path + " був створений");
        }

    }
}
