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
            var minge = new ProductList { Name = "minge", Quantity = 30 };
            var bile = new ProductList { Name = "bile", Quantity = 10 };
            var biciclete = new ProductList { Name = "biciclete", Quantity = 18 };
            var papusa = new ProductList { Name = "papusa", Quantity = 2 };
            var bile1 = new ProductList { Name = "bile", Quantity = 5 };
            var biciclete1 = new ProductList { Name = "biciclete", Quantity = 7 };
            var palarie = new ProductList { Name = "palarie", Quantity = 34 };
            var fluture = new ProductList { Name = "fluture", Quantity = 12 };
            var minge1 = new ProductList { Name = "minge", Quantity = 5 };
            var palarie1 = new ProductList { Name = "palarie", Quantity = 12 };

            // papusa,  fluture
            var productList1 = new ProductList[] { minge, bile, biciclete, papusa, palarie };
            var productList2 = new ProductList[] { bile1, biciclete1, fluture, minge1, palarie1 };

            //minge,  bile,  biciclete,  palarie 

            var result = ProductFeatures.AllProductsAppearsOnlyOnceAndGenerateTotalIfDuplicates(productList1, productList2);
            var expected = new ProductList[]
            {
                new ProductList { Name = "minge", Quantity = 35 },
                new ProductList { Name = "bile", Quantity = 15 },
                new ProductList { Name = "biciclete", Quantity = 25 },
                new ProductList { Name = "palarie", Quantity = 46 },
                new ProductList { Name = "papusa", Quantity = 2 },
                new ProductList { Name = "fluture", Quantity = 12 },
            };

            Assert.Equal(expected, result);
        }

    }
}
