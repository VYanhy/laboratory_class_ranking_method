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
        #region fields

        public Ranking ranking;
        protected Dictionary<int, DistanceRow> distances = new Dictionary<int, DistanceRow>();
        protected List<CompromiseRow> all_distances = new List<CompromiseRow>();
        protected List<CompromiseRow> compromise_distances = new List<CompromiseRow>();
        protected int min;
        protected int max;

        Dictionary<int, double> competence = new Dictionary<int, double>();
        Random rand = new Random();
        int compromise_num = 0;

        #endregion

        #region properties

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

        public Dictionary<int, double> Competence
        {
            get
            {
                if (competence.Count == 0)
                {
                    ExpertCompetence();
                }

                return competence;
            }
        }

        #endregion

        #region constructors
        public Metric()
        {

        }

        public Metric(Ranking ranking)
        {
            this.ranking = ranking;
        }

        #endregion

        #region saving_to_workbook_methods

        public abstract void SaveInitialDataToWorkbook(string workbook_path);

        public void SaveDistancesToWorkbook(string workbook_path)
        {
            SaveInitialDataToWorkbook(workbook_path);
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
            SaveInitialDataToWorkbook(workbook_path);
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
            SaveInitialDataToWorkbook(workbook_path);
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

                for (; j < CompromiseDistances.ElementAt(i).distance.Count; j++)
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
            SaveInitialDataToWorkbook(workbook_path);
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

                for (; j < CompromiseDistances.ElementAt(i).distance.Count; j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(filling, j)].PutValue(CompromiseDistances.ElementAt(i).distance.ElementAt(j));
                }

                for (int k = 0; k < ranking.n; k++, j++)
                {
                    sheet.Cells[CellsHelper.CellIndexToName(filling, j + 1)].PutValue(CompromiseDistances.ElementAt(i).distance_sum.ElementAt(k));
                }

                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 2)].PutValue(CompromiseDistances.ElementAt(i).sum);
                sheet.Cells[CellsHelper.CellIndexToName(filling, j + 3)].PutValue(CompromiseDistances.ElementAt(i).max);
            }

            workbook.Save(workbook_path, SaveFormat.Xlsx);
            MessageBox.Show("Файл " + workbook_path + " був створений");
        }
        
        public void SaveCompetenceToWorkbook(string workbook_path)
        {
            SaveCompromisesToWorkbookWithSumsByExpert(workbook_path);
            Workbook workbook = new Workbook(workbook_path);
            Worksheet sheet = workbook.Worksheets[workbook.Worksheets.Add()];

            sheet.Cells[CellsHelper.CellIndexToName(0, 0)].PutValue("Для компромісного ранжування № " + (compromise_num + 1));

            for (int i = 0; i < Competence.Count; i++)
            {
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 0)].PutValue("Експерт " + (Competence.ElementAt(i).Key + 1));
                sheet.Cells[CellsHelper.CellIndexToName(i + 1, 1)].PutValue(Competence.ElementAt(i).Value);
            }

            workbook.Save(workbook_path, SaveFormat.Xlsx);
            MessageBox.Show("У файл " + workbook_path + " була додана інформація про компетентності експертів");
        }

        #endregion

        #region read_data

        protected abstract void ReadInitialMatrix(string workbook_path);

        protected abstract void ReadCompromisesFromWorksheet(Worksheet worksheet);

        public void ReadInitialMatrixAndCompromisesFromWorkbook(string workbook_path)
        {
            Workbook workbook = new Workbook(workbook_path);

            ReadInitialMatrix(workbook_path);
            ReadCompromisesFromWorksheet(workbook.Worksheets[2]);
        }

        #endregion

        protected abstract void Distance();

        protected abstract List<CompromiseRow> CalculateAllDistances();

        protected void FindCompromiseRows()
        {
            if (Permutation.permutations == null)
            {
                Permutation.CalculatePermutations(Ranking.m);
            }

            CalculateAllDistances();
        }

        private static int determineLCD(int a, int b)
        {
            int num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (int i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num1 * num2;
        }


        public List<CompromiseRow> MinMax()
        {
            FindCompromiseRows();
            List<CompromiseRow> temp = new List<CompromiseRow>();

            min = all_distances.Min(x => x.sum);
            foreach (CompromiseRow c in all_distances)
            {
                if (c.sum == min)
                {
                    temp.Add(c);
                }
            }
            max = temp.Max(x => x.max);

            foreach (CompromiseRow c in temp)
            {
                if (c.max == max)
                {
                    compromise_distances.Add(c);
                }
            }

            return compromise_distances;
        }

        public Dictionary<int, double> ExpertCompetence()
        {
            competence = new Dictionary<int, double>();
            compromise_num = rand.Next(0, CompromiseDistances.Count);
            CompromiseRow compromise = CompromiseDistances.ElementAt(compromise_num);

            List<double> temp = new List<double>();

            double denominator = 1;
            compromise.distance_sum.ForEach(i =>
            {
                int val = i;
                if (i != 0)
                {
                    val = i;
                }
                else
                {
                    val = 1;
                }
                denominator = determineLCD((int)denominator, val);
            });

            temp = new List<double>();
            compromise.distance_sum.ForEach(i =>
            {
                if (i != 0)
                {
                    temp.Add((int)denominator / (int)i);
                }
                else
                {
                    temp.Add((int)denominator);
                }
            });
            double numerator = temp.Sum();


            for (int i = 0; i < compromise.distance_sum.Count; i++)
            {
                if (compromise.distance_sum.ElementAt(i) != 0)
                {
                    competence.Add(i, Math.Round(
                    ((1 / (double)compromise.distance_sum.ElementAt(i)) / (numerator / denominator)), 2));
                }
                else
                {
                    competence.Add(i, Math.Round(
                    (denominator / numerator), 2));
                }
            }

            if (competence.Values.Sum() != 0)
            {
                double min = competence.Values.Min();
                List<int> min_index_list = (from x in competence where x.Value == min select x.Key).ToList();

                double sum_oth = (from x in competence where x.Value != min select x.Value).ToList().Sum();

                min_index_list.ForEach(i =>
                {
                    competence[i] = ((1 - sum_oth) / min_index_list.Count);
                });

            }

            return competence;
        }

    }
}
