using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class DistanceRow
    {
        public List<int> distance;
        public int sum;

        public DistanceRow()
        {

        }

        public DistanceRow(List<int> distance)
        {
            this.distance = distance;
            sum = distance.Sum();
        }
    }
}
