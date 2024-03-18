using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqProduct
{
    public struct ProductWithQuantity
    {
        public string Name;
        public int Quantity;

        public ProductWithQuantity(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
