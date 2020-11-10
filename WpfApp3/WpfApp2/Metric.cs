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

        Dictionary<int, double> competence = new Dictionary<int, double>();


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

                for (; j < AllDistances.ElementAt(0).distance.Count/* Ranking.m*/; j++)
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

        public void SaveCompromisesToWorkbookWithSumsByExpert(string workbook_path)
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

                for (; j < ranking.n; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(filling, j + 1)].PutValue(CompromiseDistances.ElementAt(i).distance_sum.ElementAt(j));
                }

                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 1)].PutValue(CompromiseDistances.ElementAt(i).sum);
                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 2)].PutValue(CompromiseDistances.ElementAt(i).max);
            }

            workbook.Save(workbook_path, SaveFormat.Xlsx);
            MessageBox.Show("Файл " + workbook_path + " був створений");
        }


        protected abstract void ReadMatrixForCompromises(Worksheet sheetMatrix);

        public void ReadCompromisesFromWorkbook(string workbook_path)
        {
            Workbook workbook = new Workbook(workbook_path);
            Worksheet sheetMatrix = workbook.Worksheets[0];
            Worksheet sheetWithCompromises = workbook.Worksheets[2];

            ReadMatrixForCompromises(sheetMatrix);

            //int colcount = sheetWithCompromises.Cells.Columns.Count;
            int rowcount = sheetWithCompromises.Cells.Rows.Count;

            for (int i = 0, j = 0; i < rowcount; i++, j = 0)
            {
                List<int> temp_distances = new List<int>();
                List<int> temp_distance_sum = new List<int>();

                for (; j < Ranking.m; j++)
                {
                    temp_distances.Add(
                        Convert.ToInt32(
                        sheetWithCompromises.Cells[CellsHelper.CellIndexToName(i, j)].Value));
                }

                for (; j < ranking.n; j++)
                {
                    temp_distance_sum.Add(
                        Convert.ToInt32(
                        sheetWithCompromises.Cells[CellsHelper.CellIndexToName(i, j + 1)].Value));
                }

                compromise_distances.Add(new CompromiseRow(temp_distances, temp_distance_sum));
            }
        }

        Random rand = new Random();
        public void ExpertCompetence()
        {
            int compromise_num = rand.Next(0, CompromiseDistances.Count);
            CompromiseRow compromise = CompromiseDistances.ElementAt(compromise_num);
            double denominator = compromise.max;

            for (int i = 0; i < compromise.distance_sum.Count; i++)
            {
                competence.Add(i, Math.Round(
                    ((denominator - compromise.distance_sum.ElementAt(i)) / denominator), 2);
            }

            if (competence.Values.Sum() != 0)
            {
                double min = competence.Values.Min();
                List<int> min_index_list = (from x in competence where x.Value == min select x.Key).ToList();
                
                double sum_oth = (from x in competence where x.Value != min select x.Value).ToList().Sum();

                min_index_list.ForEach(i =>
                {
                    competence[i] = (1 - sum_oth) / min_index_list.Count;
                });
            }

        }




    }
}
