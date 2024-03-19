using LinqStock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqProduct 
{
    public class ProductFeatures
    {
        public static IEnumerable<Product> AtLeastOneFeature(Product[] productsList, Feature[] featuresList)
        {
            return productsList.Where(product => product.Features.Intersect(featuresList).Any());
        }

        public static IEnumerable<Product> AllFeatures(Product[] productsList, Feature[] featuresList)
        {
            return productsList.Where(product => product.Features.Intersect(featuresList).Count() == featuresList.Length);
        }

        public static IEnumerable<Product> NotEvenOneFeature(Product[] productsList, Feature[] featuresList)
        {
            return productsList.Where(product => product.Features.Intersect(featuresList).Count() == 0);
        }

        public static IEnumerable<ProductWithQuantity> AllProductsAppearsOnlyOnceAndGenerateTotalIfDuplicates(ProductWithQuantity[] firstList, ProductWithQuantity[] secondList)
        {
            return firstList.Concat(secondList).GroupBy(key => key.Name)
                .Select(group => 
                {
                    var totalQuantity = group.Sum(product => product.Quantity);
                    return new ProductWithQuantity { Name = group.Key, Quantity = totalQuantity };
                });
        }
    }
}
