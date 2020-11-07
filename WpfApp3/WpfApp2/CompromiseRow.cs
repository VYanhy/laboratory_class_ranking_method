using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class CompromiseRow
    {
        public List<int> distance;
        public List<int> distance_sum;
        public int sum;
        public int max;

        public CompromiseRow()
        {

        }

        public CompromiseRow(List<int> distance, List<int> distance_sums)
        {
            this.distance = distance;
            this.distance_sum = distance_sums;

            sum = distance_sums.Sum();
            max = distance_sums.Max();
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
