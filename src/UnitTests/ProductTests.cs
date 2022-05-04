using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ProductTests : BaseTest
    {
        [TestMethod]
        public void GetProductByIdTest()
        {
            var product = ProductService.GetProduct(1);
            Assert.IsNotNull(product);
            Assert.AreEqual(1, product.Id);
        }

        [TestMethod]
        public void GetProductByNameTest()
        {
            var product = ProductService.GetProduct("coke");
            Assert.IsNotNull(product);
            Assert.AreEqual("coke", product.Name);
        }

        [TestMethod]
        public void GetProductsTest()
        {
            var products = ProductService.GetProducts();
            Assert.IsNotNull(products);
            Assert.IsTrue(products.Count > 0);
        }

        [TestMethod]
        public void ShipProductTest()
        {
            var product = ProductService.GetProduct(1);
            Assert.IsNotNull(product);
            Assert.AreEqual(1, product.Id);

            var shipped = ProductService.ShipProduct(product);
            Assert.AreEqual(true, shipped);
        }

        [TestMethod]
        public void ShipProduct_OutOfStock_Test()
        {
            var product = ProductService.GetProduct(1);
            Assert.IsNotNull(product);
            Assert.AreEqual(1, product.Id);

            var availableQuantity = product.AvailableQuantity;
            bool shipped;

            // Ship entire available quantity
            for (int i = 0; i < availableQuantity; i++)
            {
                shipped = ProductService.ShipProduct(product);
                Assert.AreEqual(true, shipped);
            }

            // One more shipping attempt
            shipped = ProductService.ShipProduct(product);
            Assert.AreEqual(false, shipped);

        }
    }
}