using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class ChoiceRow
    {
        public Product Product { get; set; }
        public int Rate { get; set; }

        public ChoiceRow() { }

        public ChoiceRow(Product product, int rate)
        {
            Product = product;
            Rate = rate;
        }

    }
}
