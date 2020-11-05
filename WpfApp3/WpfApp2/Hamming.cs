using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class Hamming
    {
        /*
        public Ranking ranking;
        Dictionary<int, List<int>> distance;
        Dictionary<int, int> distance_sum;
        public int min;


        public void HammingDistance(int obj_num, long expert_num, int[,] matrix)
        {
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
                        int val;

                        if (matrix[i, k] < matrix[j, k])
                            val = 1;
                        else
                            val = -1;

                        temp.Add(val);
                        //                        Console.WriteLine(matrix[i, k] + "?" + matrix[j, k] + " " + val);
                    }

                    distance.Add(key, temp);
                    distance_sum.Add(key, temp.Sum());
                }
            }

            //WriteToConsole();
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

        public void FindCompromiseRanking()
        {
            int obj_num = Ranking.m;

            if (Permutation.permutations == null)
            {
                Permutation.CalculatePermutations(obj_num);
            }

            HammingDistance(obj_num, Permutation.permutations.Length, Permutation.permutations);
            Console.WriteLine(MinMax());
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
        */

    }
}
