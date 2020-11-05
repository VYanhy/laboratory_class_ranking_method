using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class Cook
    {
        public Ranking ranking;
        Dictionary<int, List<int>> distance;
        Dictionary<int, int> distance_sum;
        public int min;

        public void CooksDistance(int obj_num, long expert_num, int[,] matrix)
        {
            expert_num = 5;

            distance = new Dictionary<int, List<int>>();
            distance_sum = new Dictionary<int, int>();

            for (int i = 0; i < obj_num; i++)
            {
                for (int j = i + 1; j < obj_num; j++)
                {
                    int key = Convert.ToInt32((i + 1).ToString() + (j + 1).ToString());
                    List<int> temp = new List<int>();

                    for (int k = 0; k < expert_num; k++)
                    {
                        temp.Add(Math.Abs(matrix[i, k] - matrix[j, k]));
//                        Console.WriteLine(matrix[i, k] + "-" + matrix[j, k]);
                    }

                    distance.Add(key, temp);
                    distance_sum.Add(key, temp.Sum());
                }
            }
        }

        public Dictionary<int, List<int>> MinMax()
        {
            min = distance_sum.Values.Min();

            Dictionary<int, List<int>> num_list = new Dictionary<int, List<int>>();

            foreach (KeyValuePair<int, int> i in distance_sum)
            {
                if (i.Value == min)
                {
                    num_list.Add(i.Key, distance[i.Key]);
                }
            }

            return num_list;
        }

        public Dictionary<int, List<int>> FindCompromiseRanking()
        {
            int obj_num = Ranking.m;

            if (Permutation.matrix_permutation == null)
            {
                Permutation.CalculatePermutationMatrix(5/*obj_num*/);
            }

            CooksDistance(obj_num, Permutation.matrix_permutation.Length, Permutation.matrix_permutation);
            
            return MinMax();
        }

        public void WriteToConsole()
        {
            foreach (KeyValuePair<int, List<int>> d in distance)
            {
                Console.Write(d.Key + " ");

                foreach (int i in d.Value)
                {
                    Console.Write(i + " ");
                }

                Console.Write(distance_sum[d.Key]);

                Console.WriteLine();
            }
        }


    }

}

