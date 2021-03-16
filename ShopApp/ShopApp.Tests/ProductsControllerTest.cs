using Xunit;
using Moq;
using ShopApp.Database;
using ShopApp.Controllers;
using ShopApp.Models;

namespace ShopApp.Tests
{

    public class ProductsControllerTest
    {
        [Fact]
        public void isRequestValidRetunsTrue()
        {
            var mock = new Mock<IProductRepository>();
            ProductsController controller = new ProductsController(mock.Object);
            Product product = new Product()
            {
                Id = 1,
                ShopId = 1,
                Category = "Sport",
                Cost = 3.2,
                Count = 3,
                Name = "aaaa"
            };
            ShopAdminProductRequest request = new ShopAdminProductRequest();
            request.Password = "password";
            request.Product = product;

            bool result = controller.isRequestValid(request);

            Assert.True(result);
        }

        [Fact]
        public void postPutDeleteProductRetunsCode403()
        {
            var request = new ShopAdminProductRequest();
            var dbMock = new Mock<IProductRepository>();
            dbMock.Setup(f => f.isPasswordRight(request)).Returns(false);

            var controllerMock = new Mock<ProductsController>(dbMock.Object);
            controllerMock.Setup(f => f.isRequestValid(request, false)).Returns(true);
            controllerMock.Setup(f => f.isRequestValid(request, true)).Returns(true);
            ProductsController controller = controllerMock.Object;

            var resultPost = controller.PostProduct(request);
            var resultPut = controller.UpdateProduct(request);
            var resultDelete = controller.DeleteProduct(request);

            Assert.Equal(403, resultPost.StatusCode);
            Assert.Equal(403, resultPut.StatusCode);
            Assert.Equal(403, resultDelete.StatusCode);
        }


        [Fact]
        public void putProductRetunsCode404()
        {
            var request = new ShopAdminProductRequest() { Product = new Product() { Id = 1 } };
            var dbMock = new Mock<IProductRepository>();
            dbMock.Setup(f => f.isPasswordRight(request)).Returns(true);
            dbMock.Setup(f => f.Get(0)).Returns(new Product());

            var controllerMock = new Mock<ProductsController>(dbMock.Object);
            controllerMock.Setup(f => f.isRequestValid(request, false)).Returns(true);
            ProductsController controller = controllerMock.Object;

            var result = controller.UpdateProduct(request);

            Assert.Equal(404, result.StatusCode);
        }

    }
}
