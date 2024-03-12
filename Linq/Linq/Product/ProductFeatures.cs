using System;
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
            var list = new List<Product>();
            foreach (Product product in productsList) 
            {
                foreach (Feature feature in featuresList)
                {
                    if (product.Features.Any(element => element.Id == feature.Id))
                    {
                        list.Add(product);
                    }
                }
            }

            return list;
        }

        public static IEnumerable<Product> AllFeatures(List<Product> productsList, List<Feature> featuresList)
        {
            var list = new List<Product>();
            foreach (Product product in productsList)
            {
                    if (featuresList.All(elements => product.Features.Contains(elements)))
                    {
                        list.Add(product);
                    }
            }

            return list;
        }
    }
}
