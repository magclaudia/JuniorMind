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
        public static IEnumerable<Product> AtLeastOneFeature(List<Product> productsList, List<Feature> featuresList)
        {
            return productsList.FindAll(product => product.Features.Any(productFeature => featuresList
                                .Any(feature => feature.Id == productFeature.Id)));   
        }

        public static IEnumerable<Product> AllFeatures(List<Product> productsList, List<Feature> featuresList)
        {
            return productsList.FindAll(product => featuresList.All(feature => product.Features.Contains(feature)));
        }

        public static IEnumerable<Product> NotEvenOneFeature(List<Product> productsList, List<Feature> featuresList)
        {
            return productsList.FindAll(product => featuresList.All(feature => !product.Features.Contains(feature)));
        }
    }
}
