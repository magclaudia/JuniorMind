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
            Assert.Equal("Not enough quantity of this product: 'bicicleta' in stock. After sale quantity of this product will be: '-1'.", exception2.Message);
        }

        [Fact]
        public void Check_UpdateQuantityAfterSellMethod_Under_10Products()
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

            stock.Sell(productType[1], 1);

            Action<Product, int> callback = stock.UpdateQuantityAfBterSell;
            Assert.NotNull(callback);
        }

        [Fact]
        public void Check_UpdateQuantityAfterSellMethod_Under_5Products()
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

            stock.Sell(productType[0], 46);

            Action<Product, int> callback = stock.UpdateQuantityAfBterSell;
            Assert.NotNull(callback);
        }

        [Fact]
        public void Check_UpdateQuantityAfterSellMethod_Under_2Products()
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

            stock.Sell(productType[2], 14);

            Action<Product, int> callback = stock.UpdateQuantityAfBterSell;
            Assert.NotNull(callback);
        }
    }
}
