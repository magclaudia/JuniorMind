using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LinqProduct
{
    public class ProductListFacts
    {
        [Fact]
        public void AllProductsAppearsOnlyOnce()
        {
            var minge = new ProductWithQuantity { Name = "minge", Quantity = 30 };
            var bile = new ProductWithQuantity { Name = "bile", Quantity = 10 };
            var biciclete = new ProductWithQuantity { Name = "biciclete", Quantity = 18 };
            var papusa = new ProductWithQuantity { Name = "papusa", Quantity = 2 };
            var bile1 = new ProductWithQuantity { Name = "bile", Quantity = 5 };
            var biciclete1 = new ProductWithQuantity { Name = "biciclete", Quantity = 7 };
            var palarie = new ProductWithQuantity { Name = "palarie", Quantity = 34 };
            var fluture = new ProductWithQuantity { Name = "fluture", Quantity = 12 };
            var minge1 = new ProductWithQuantity { Name = "minge", Quantity = 5 };
            var palarie1 = new ProductWithQuantity { Name = "palarie", Quantity = 12 };

            // papusa,  fluture
            var productList1 = new ProductWithQuantity[] { minge, bile, biciclete, papusa, palarie };
            var productList2 = new ProductWithQuantity[] { bile1, biciclete1, fluture, minge1, palarie1 };

            //minge,  bile,  biciclete,  palarie 

            var result = ProductFeatures.AllProductsAppearsOnlyOnceAndGenerateTotalIfDuplicates(productList1, productList2);
            var expected = new ProductWithQuantity[]
            {
                new ProductWithQuantity { Name = "minge", Quantity = 35 },
                new ProductWithQuantity { Name = "bile", Quantity = 15 },
                new ProductWithQuantity { Name = "biciclete", Quantity = 25 },
                new ProductWithQuantity { Name = "papusa", Quantity = 2 },
                new ProductWithQuantity { Name = "palarie", Quantity = 46 },
                new ProductWithQuantity { Name = "fluture", Quantity = 12 },
            };

            Assert.Equal(expected, result);
        }
    }
}
