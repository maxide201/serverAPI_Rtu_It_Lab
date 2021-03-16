using Xunit;
using Moq;
using ShopApp.Database;
using ShopApp.Controllers;
using ShopApp.Models;

namespace ShopApp.Tests
{
    public class PurchasesControllerTest
    {
        [Fact]
        public void MakePurchaseReturnsCode409()
        {
            CheckDTO returnedCheck = null;
            var request = new PurchaseRequest();
            var dbMock = new Mock<IPurchaseRepository>();
            dbMock.Setup(f => f.isUserAndShopExists(request)).Returns(true);
            dbMock.Setup(f => f.MakePurchase(request)).Returns(returnedCheck);

            var controllerMock = new Mock<PurchasesController>(dbMock.Object);
            controllerMock.Setup(f => f.isPurchaseRequestValid(request)).Returns(true);
            PurchasesController controller = controllerMock.Object;

            var result = controller.MakePurchase(request);

            Assert.Equal(409, result.StatusCode);
        }

        [Fact]
        public void GetChecksReturns403()
        {
            var request = new ShopAdminRequest();
            var dbMock = new Mock<IPurchaseRepository>();
            dbMock.Setup(f => f.isShopPasswordRight(request)).Returns(false);

            var controllerMock = new Mock<PurchasesController>(dbMock.Object);
            controllerMock.Setup(f => f.isShopRequestValid(request)).Returns(true);
            PurchasesController controller = controllerMock.Object;

            var result = controller.GetChecks(request);

            Assert.Equal(403, result.StatusCode);
        }

        [Fact]
        public void isShopRequestValidReturnsTrue()
        {
            var dbMock = new Mock<IPurchaseRepository>();
            var controller = new PurchasesController(dbMock.Object);
            var request = new ShopAdminRequest()
            {
                Password = "password",
                ShopId = 3
            };

            var result = controller.isShopRequestValid(request);

            Assert.True(result);
        }

        [Fact]
        public void isPurchaseRequestValidReturnsTrue()
        {
            var dbMock = new Mock<IPurchaseRepository>();
            var controller = new PurchasesController(dbMock.Object);
            var request = new PurchaseRequest()
            {
                UserId = 1,
                ShopId = 2,
                PaymentMethod = "card",
                Products = new System.Collections.Generic.List<ProductDTO>()
                {
                    new ProductDTO()
                }
            };

            var result = controller.isPurchaseRequestValid(request);

            Assert.True(result);
        }
    }
}
