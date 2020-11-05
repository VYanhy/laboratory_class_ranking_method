using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public Product() { }
        public Product(int id, string name)
        {
            ProductId = id;
            ProductName = name;
        }

        public override string ToString()
        {
            return ProductId + ". " + ProductName;
        }
    }
}
