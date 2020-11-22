using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class ProductCollection : ObservableCollection<Product>
    {
        public ProductCollection()
        {
            Add(new Product() { ProductId = 1, ProductName = "Adobe Illustrator" });
            Add(new Product() { ProductId = 2, ProductName = "Adobe InDesign" });
            Add(new Product() { ProductId = 3, ProductName = "Adobe XD" });
            Add(new Product() { ProductId = 4, ProductName = "Balsamiq Mockups" });
            Add(new Product() { ProductId = 5, ProductName = "Cleanmock" });
            
            Add(new Product() { ProductId = 6, ProductName = "Figma" });
            Add(new Product() { ProductId = 7, ProductName = "OmniGraffle" });
            Add(new Product() { ProductId = 8, ProductName = "Sketch" });
            /*
            Add(new Product() { ProductId = 9, ProductName = "Smartmockups" });
            Add(new Product() { ProductId = 10, ProductName = "UXPin" });
            */
        }
    }
}
