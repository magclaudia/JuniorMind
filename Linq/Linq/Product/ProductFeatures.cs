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
            return productsList.Where(product => product.Features.Any(productFeature => featuresList
                               .Contains(productFeature)));
        }

        public static IEnumerable<Product> AllFeatures(Product[] productsList, Feature[] featuresList)
        {
            return productsList.Where(product => featuresList.All(feature => product.Features.Contains(feature)));
        }

        public static IEnumerable<Product> NotEvenOneFeature(Product[] productsList, Feature[] featuresList)
        {
            return productsList.Where(product => featuresList.All(feature => !product.Features.Contains(feature)));
        }
    }
}
