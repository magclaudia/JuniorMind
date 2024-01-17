using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqStock
{
    public class Stock
    {
        private readonly List<Product> productList;
        public Stock() 
        {
            productList = new List<Product>();
        }

        public void Add(Product product)
        {
            if (productList.Contains(product))
            {
                throw new ArgumentException("This product exist in stock.");
            }

            productList.Add(product);
        }

        public bool Contains(Product product)
        {
            return productList.Any(item => item.ProductName == product.ProductName);
        }

        public int CurrentQantityOfProduct(Product product)
        {
            return productList[FindIndexOfProduct(product.ProductName)].Quantity;
        }

        private int FindIndexOfProduct(string name)
        {
            for (int i = 0; i < productList.Count; i++)
            {
                if (productList[i].ProductName == name)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
