using Xunit;

namespace LinqStock
{
    public class StockFacts
    {
        [Fact]

        public void Contains_Add_Methods()
        {
            var stock = new Stock();
            var productType = new List<Product>()
            {
                new Product("bicicleta", 50),
                new Product("papusa", 10),
                new Product("minge", 15),
                new Product("masina", 2)
            };

            stock.Add(productType[0]);
            stock.Add(productType[1]);
            stock.Add(productType[2]);

            Assert.True(stock.Contains(productType[0]));
            Assert.False(stock.Contains(productType[3]));
        }

        [Fact]
        public void CurrentQantityOfProduct_Method()
        {
            var stock = new Stock();
            var productType = new List<Product>()
            {
                new Product("bicicleta", 50),
                new Product("papusa", 10),
                new Product("minge", 15),
            };

            stock.Add(productType[0]);
            stock.Add(productType[1]);
            stock.Add(productType[2]);

            Assert.Equal(50, stock.CurrentQantityOfProduct(productType[0]));
            Assert.Equal(15, stock.CurrentQantityOfProduct(productType[2]));
        }
    }
}
