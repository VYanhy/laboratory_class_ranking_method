using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public static class Permutation
    {
        public static int[,] matrix_permutation;

        public static void CalculatePermutationMatrix(int obj_num)
        {
            int[] obj = new int[obj_num];
            for (int i = 0; i < obj_num; i++)
            {
                obj[i] = i + 1;
            }

            long n = Fact(obj_num);
            matrix_permutation = new int[obj_num, n];

            List<int[]> temp = new List<int[]>();
            foreach (var permutation in obj.GetPermutations())
            {
                temp.Add(permutation.ToArray<int>());
            }
            int[][] matrix_for_convert = temp.ToArray();


            for (int i = 0; i < obj_num; i++)
            {
                for (long j = 0; j < n; j++)
                {
                    matrix_permutation[i, j] = matrix_for_convert[j][i];
                    Console.Write(matrix_permutation[i, j] + " ");
                }
                Console.WriteLine();
            }
        }


        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();

            var factorials = Enumerable.Range(0, array.Length + 1)
                .Select(Factorial)
                .ToArray();

            for (var i = 0L; i < factorials[array.Length]; i++)
            {
                var sequence = GenerateSequence(i, array.Length - 1, factorials);

                yield return GeneratePermutation(array, sequence);
            }
        }

        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var clone = (T[])array.Clone();

            for (int i = 0; i < clone.Length - 1; i++)
            {
                Swap(ref clone[i], ref clone[i + sequence[i]]);
            }

            return clone;
        }

        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var sequence = new int[size];

            for (var j = 0; j < sequence.Length; j++)
            {
                var facto = factorials[sequence.Length - j];

                sequence[j] = (int)(number / facto);
                number = (int)(number % facto);
            }

            return sequence;
        }

        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        private static long Factorial(int n)
        {
            long result = n;

            for (int i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }

        public static long Fact(int n)
        {
            long result = n;

            for (int i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }

    }
}
