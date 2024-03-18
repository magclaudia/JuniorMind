using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqProduct
{
    public struct ProductList
    {
        public string Name;
        public int Quantity;

        public ProductList(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
