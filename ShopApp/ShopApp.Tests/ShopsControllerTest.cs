using Xunit;
using Moq;
using ShopApp.Database;
using ShopApp.Controllers;
using ShopApp.Models;
using Microsoft.Extensions.Configuration;

namespace ShopApp.Tests
{
    public class ShopsControllerTest
    {
        [Fact]
        public void PostDeleteShopReturns403()
        {
            var dbMock = new Mock<IShopRepository>();
            var dbConf = new Mock<IConfiguration>();
            var controllerMock = new Mock<ShopsController>(dbMock.Object, dbConf.Object);
            controllerMock.Setup(f => f.isSuperAdmin(" ")).Returns(false);
            ShopsController controller = controllerMock.Object;

            var resultPost = controller.PostShop(new SuperAdminRequest() { RootPassword = " "});
            var resultDelete = controller.DeleteShop(new SuperAdminRequest() { RootPassword = " "});

            Assert.Equal(403, resultPost.StatusCode);
            Assert.Equal(403, resultDelete.StatusCode);
        }

        [Fact]
        public void DeleteShopReturns404()
        {
            Shop returnedShop = null;
            var dbMock = new Mock<IShopRepository>();
            var dbConf = new Mock<IConfiguration>();
            dbMock.Setup(f => f.Get(1)).Returns(returnedShop);
            var controllerMock = new Mock<ShopsController>(dbMock.Object, dbConf.Object);
            controllerMock.Setup(f => f.isSuperAdmin(" ")).Returns(true);
            controllerMock.Setup(f => f.isShopValid(new Shop())).Returns(true);
            ShopsController controller = controllerMock.Object;

            var result = controller.DeleteShop(new SuperAdminRequest() { RootPassword = " ", Shop = new Shop() {Id = 1 } });

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void isShopValidReturnsTrue()
        {
            var dbMock = new Mock<IShopRepository>();
            var dbConf = new Mock<IConfiguration>();
            var controller = new ShopsController(dbMock.Object, dbConf.Object);
            var shop = new Shop()
            {
                Id = 1,
                Address = "a",
                Name = "a",
                PhoneNumber = "a",
                Password = "a"
            };

            var result = controller.isShopValid(shop);

            Assert.True(result);
        }
    }
}
