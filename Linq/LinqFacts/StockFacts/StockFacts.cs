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

            Assert.True(stock.ContainsProduct(productType[0]));
            Assert.False(stock.ContainsProduct(productType[3]));
            var exception = Assert.Throws<ArgumentException>(() => stock.Add(productType[0]));
            Assert.Equal("This product: 'bicicleta' exist in stock.", exception.Message);
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

        [Fact]
        public void Sell_Method()
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
            stock.Sell(productType[0], 30);

            Assert.Equal(20, productType[0].Quantity);
            var exception1 = Assert.Throws<ArgumentException>(() => stock.Sell(new Product("bila", 10), 5));
            Assert.Equal("This product: 'bila' don`t exist in stock.", exception1.Message);

            var exception2 = Assert.Throws<ArgumentException>(() => stock.Sell(productType[0], 21));
            Assert.Equal($"Not enough quantity of this product: 'bicicleta' in stock. After sale quantity of this product will be: '-1'.", exception2.Message);
        }
    }
}
