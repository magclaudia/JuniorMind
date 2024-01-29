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
            string message = "";
            var productType = new List<Product>()
            {
                new Product("bicicleta", 50),
                new Product("papusa", 10),
                new Product("minge", 15),
            };

            stock.Add(productType[0]);
            stock.Add(productType[1]);
            stock.Add(productType[2]);
            bool receiveNotification = false;
            Action<Product, int> action = (product, quantity) =>
            {
                receiveNotification = true;
                message = $"Quantity of product: '{product.ProductName}' is under 10 pieces, it remaind {quantity} products of this type.";
            };

            stock.CallRegistration(action);
            stock.Sell(productType[1], 1);
            Assert.True(receiveNotification);
            Assert.Equal("Quantity of product: 'papusa' is under 10 pieces, it remaind 9 products of this type.", message);
        }

        [Fact]
        public void Check_UpdateQuantityAfterSellMethod_Under_5Products()
        {
            var stock = new Stock();
            string message = "";
            var productType = new List<Product>()
            {
                new Product("bicicleta", 50),
                new Product("papusa", 10),
                new Product("minge", 15),
            };

            stock.Add(productType[0]);
            stock.Add(productType[1]);
            stock.Add(productType[2]);
            bool receiveNotification = false;
            Action<Product, int> action = (product, quantity) =>
            {
                receiveNotification = true;
                message = $"Quantity of product: '{product.ProductName}' is under 5 pieces, it remaind {quantity} products of this type.";
            };

            stock.CallRegistration(action);
            stock.Sell(productType[0], 46);
            Assert.True(receiveNotification);
            Assert.Equal("Quantity of product: 'bicicleta' is under 5 pieces, it remaind 4 products of this type.", message);
        }

        [Fact]
        public void Check_UpdateQuantityAfterSellMethod_Under_2Products()
        {
            var stock = new Stock();
            string message = string.Empty;
            var productType = new List<Product>()
            {
                new Product("bicicleta", 50),
                new Product("papusa", 10),
                new Product("minge", 15),
            };

            stock.Add(productType[0]);
            stock.Add(productType[1]);
            stock.Add(productType[2]);
            bool receiveNotification = false;
            Action<Product, int> action = (product, quantity) =>
            {
                receiveNotification = true;
                message = $"Quantity of product: '{product.ProductName}' is under 2 pieces, it remaind {quantity} products of this type.";
            };

            stock.CallRegistration(action);
            stock.Sell(productType[2], 14);
            Assert.True(receiveNotification);
            Assert.Equal("Quantity of product: 'minge' is under 2 pieces, it remaind 1 products of this type.", message);
        }

        [Fact]
        public void Products_Sell_But_Dont_Cross_The_Threshold()
        {
            var stock = new Stock();
            string message = string.Empty;
            var productType = new List<Product>()
            {
                new Product("bicicleta", 50),
                new Product("papusa", 10),
                new Product("minge", 15),
            };

            stock.Add(productType[0]);
            stock.Add(productType[1]);
            stock.Add(productType[2]);
            bool receiveNotification = false;
            Action<Product, int> action = (product, quantity) =>
            {
                receiveNotification = true;
                message = $"Quantity of product: '{product.ProductName}' is under 2 pieces, it remaind {quantity} products of this type.";
            };

            stock.CallRegistration(action);
            stock.Sell(productType[2], 5);
            Assert.False(receiveNotification);
            Assert.Equal("", message);
        }
    }
}
