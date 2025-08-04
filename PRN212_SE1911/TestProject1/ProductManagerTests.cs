using CRUDProductApp;
using Xunit;

namespace CRUDProductApp.Tests
{
    public class ProductManagerTests
    {
        [Fact]
        public void AddProduct_Should_AddSuccessfully()
        {
            var name = "test_product_add";
            var price = 123.45m;

            TestDataHelper.RemoveTestProduct(name);
            ProductManager.AddProduct(name, price);

            var products = ProductManager.GetAllProducts();
            Assert.Contains(products, p => p.Name == name && p.Price == price);

            TestDataHelper.RemoveTestProduct(name);
        }

        [Fact]
        public void UpdateProduct_Should_UpdateSuccessfully()
        {
            var oldName = "test_product_old";
            var newName = "test_product_new";
            var newPrice = 200.00m;

            TestDataHelper.RemoveTestProduct(oldName);
            TestDataHelper.RemoveTestProduct(newName);
            TestDataHelper.AddTestProduct(oldName, 100.00m);

            ProductManager.UpdateProduct(oldName, newName, newPrice);

            var products = ProductManager.GetAllProducts();
            Assert.Contains(products, p => p.Name == newName && p.Price == newPrice);

            TestDataHelper.RemoveTestProduct(newName);
        }

        [Fact]
        public void DeleteProduct_Should_RemoveSuccessfully()
        {
            var name = "test_product_delete";
            TestDataHelper.AddTestProduct(name, 99.99m);

            ProductManager.DeleteProduct(name);
            var products = ProductManager.GetAllProducts();

            Assert.DoesNotContain(products, p => p.Name == name);
        }
    }
}
