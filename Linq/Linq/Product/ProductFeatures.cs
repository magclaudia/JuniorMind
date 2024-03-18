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
            var checkContainDouplicates = firstList.Where(first => secondList.Any(second => first.Name == second.Name))
                .Select(first =>
                {
                    var totalQuantity = first.Quantity + secondList.First(second => second.Name == first.Name).Quantity;
                    return new ProductWithQuantity { Name = first.Name, Quantity = totalQuantity };
                });

            var returnSeparateElements = firstList.Union(secondList).GroupBy(x => x.Name).Where(x => x.Count() == 1).Select(x => x.FirstOrDefault());

            return checkContainDouplicates.Concat(returnSeparateElements);
        }
    }
}
