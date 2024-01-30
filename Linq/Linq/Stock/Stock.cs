using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqStock
{
    public class Stock
    {
        private readonly List<Product> productList;
        private int[] alertThresholds = { 2, 5, 10 };
        internal Action<Product, int> callBack;

        public Stock()
        {
            productList = new List<Product>();
        }

        public void CallRegistration(Action<Product, int> action)
        {
            callBack = action;
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

            int initialQuantity = product.Quantity;
            productList[FindIndexOfProduct(product.ProductName)].Quantity = productList[FindIndexOfProduct(product.ProductName)].Quantity - quantityToSell;

            if (CheckIfNumberOfProductsIsUnderAlertThresholds(product, initialQuantity))
            {
                CallBackNotification(product);
            }
        }

        public bool CheckIfNumberOfProductsIsUnderAlertThresholds(Product product, int initialQuantity)
        {
            bool checkTreshole = false;
            for (int i = alertThresholds.Length - 1; i >= 0; i--)
            {
                if (alertThresholds[i] <= initialQuantity && product.Quantity < alertThresholds[i])
                {
                    checkTreshole = alertThresholds.Any(threshold => threshold > product.Quantity);
                }

            }

            return checkTreshole;
        }

        public void CallBackNotification(Product product)
        {
            callBack(product, product.Quantity);
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
