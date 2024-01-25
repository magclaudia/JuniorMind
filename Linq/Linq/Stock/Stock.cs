using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqStock
{
    public class Stock
    {
        private readonly List<Product> productList;
        internal Action<Product, int> callBack;

        public Stock() 
        {
            productList = new List<Product>();
        }

        public void Add(Product product)
        {
            if (productList.Contains(product))
            {
                throw new ArgumentException($"This product: '{product.ProductName}' exist in stock.");
            }

            productList.Add(product);
        }

        public bool ContainsProduct(Product product)
        {
            return productList.Any(item => item.ProductName == product.ProductName);
        }

        public void Sell(Product product, int quantityToSell)
        {
            if (!ContainsProduct(product))
            {
                throw new ArgumentException($"This product: '{product.ProductName}' don`t exist in stock.");
            }

            if ((product.Quantity - quantityToSell) < 0)
            {
                throw new ArgumentException($"Not enough quantity of this product: '{product.ProductName}' in stock. " +
                    $"After sale quantity of this product will be: '{product.Quantity - quantityToSell}'.");
            }

            productList[FindIndexOfProduct(product.ProductName)].Quantity = productList[FindIndexOfProduct(product.ProductName)].Quantity - quantityToSell;
            callBack = UpdateQuantityAfBterSell;
            callBack(product, product.Quantity);
        }

        public void UpdateQuantityAfBterSell(Product product, int quantity)
        {
            string message;
            if (product.Quantity < 10 && product.Quantity > 5)
            {
                message = $"Quantity of product: '{product.ProductName}' is under 10 pieces, it remaind {product.Quantity} products of this type.";
            }

            if (product.Quantity < 5 && product.Quantity > 2)
            {
                message = $"Quantity of product: '{product.ProductName}' is under 5 pieces, it remaind {product.Quantity} products of this type.";
            }

            if (product.Quantity < 2)
            {
                message = $"Quantity of product: '{product.ProductName}' is under 2 pieces, it remaind {product.Quantity} products of this type.";
            }
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
