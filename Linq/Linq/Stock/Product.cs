using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqStock
{
    public class Product
    {
        public Product(string name, int quantity)
        {
            this.ProductName = name;
            this.Quantity = quantity;
        }

        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
