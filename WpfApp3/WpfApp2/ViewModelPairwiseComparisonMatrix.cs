using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class ViewModelPairwiseComparisonMatrix
    {
        private String[] _columnHeaders;
        public String[] ColumnHeaders
        {
            get { return _columnHeaders; }
            set { _columnHeaders = value; }
        }

        private String[] _rowHeaders;
        public String[] RowHeaders
        {
            get { return _rowHeaders; }
            set { _rowHeaders = value; }
        }

        private int[,] _data2D;
        public int[,] Data2D
        {
            get { return _data2D; }
            set { _data2D = value; }
        }

        public ViewModelPairwiseComparisonMatrix()
        {
            ColumnHeaders = ComparisonMatrix.columnHeaders;
            RowHeaders = ComparisonMatrix.rowHeaders;
            Data2D = ComparisonMatrix.matrix;
        }
    }
}
