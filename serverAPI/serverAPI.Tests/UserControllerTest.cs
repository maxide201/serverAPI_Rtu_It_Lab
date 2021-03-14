using Microsoft.AspNetCore.Mvc;
using serverAPI.Controllers;
using serverAPI.Database;
using Xunit;
using Moq;
using serverAPI.Models;
using System.Collections.Generic;

namespace serverAPI.Tests
{

    public class UserControllerTest
    {
        [Fact]
        public void PutUserReturnsNotFound()
        {
            //Arrange
            User user = new User();

            var dbMock = new Mock<IUserRepository>();// заглушка бд
            dbMock.Setup(f => f.Get(0)).Returns(user);// Get не находит пользователя

            var controllerMock = new Mock<UsersController>(dbMock.Object); // заглушка котроллера
            controllerMock.Setup(f => f.isUserInvalid(user, true)).Returns(false); // считаем, что входные данные корректны
            UsersController controller = controllerMock.Object;

            //Act
            var result = controller.PutUser(user);

            //Assert
            Assert.Equal(result.ToString(), new JsonResult(controller.NotFound()).ToString());
        }

        [Fact]
        public void GetUserReturnsNotFound()
        {
            //Arrange
            uint id = 404;
            User user = null;
            var mock = new Mock<IUserRepository>();
            mock.Setup(f => f.Get(id)).Returns(user);// метод Get возвращает null
            UsersController controller = new UsersController(mock.Object);

            //Act
            var result = controller.GetUser(id);

            //Assert
            Assert.Equal(result.ToString(), new JsonResult(controller.NotFound()).ToString());
        }

        [Fact]
        public void DeleteUserReturnsNotFound()
        {
            //Arrange
            uint id = 404;
            User user = null;
            var mock = new Mock<IUserRepository>();
            mock.Setup(f => f.Get(id)).Returns(user);// метод Get возвращает null
            UsersController controller = new UsersController(mock.Object);

            //Act
            var result = controller.DeleteUser(id);

            //Assert
            Assert.Equal(result.ToString(), new JsonResult(controller.NotFound()).ToString());
        }

        [Fact]
        public void isUserInvalidReturnsFalseWhenUserIsNull()
        {
            //Arrange
            User user = null;
            var mock = new Mock<IUserRepository>();
            UsersController controller = new UsersController(mock.Object);

            //Act
            var result = controller.isUserInvalid(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void isUserInvalidReturnsFalseWhenPurchasesIsNull()
        {
            //Arrange
            User user = new User() { Id = 1 };
            var mock = new Mock<IUserRepository>();
            UsersController controller = new UsersController(mock.Object);

            //Act
            var result = controller.isUserInvalid(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void isUserInvalidReturnsFalseWhenPurchaseIncorrect()
        {
            //Arrange
            var purchases = new List<Purchase>();
            var purchase = new Purchase()
            {
                Id = 1,
                UserId = 1,
                Name = null, // недопустимое значение
                PurchaseDate = new System.DateTime(12, 12, 12),
                Cost = 100
            };
            purchases.Add(purchase);
            User user = new User()
            {
                Id = 1,
                Purchases = purchases
            };
            var mock = new Mock<IUserRepository>();
            UsersController controller = new UsersController(mock.Object);

            //Act
            var result = controller.isUserInvalid(user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void isUserInvalidReturnsFalse()
        {
            //Arrange
            var purchases = new List<Purchase>();
            var purchase = new Purchase()
            {
                Id = 1,
                UserId = 1,
                Name = "apple",
                PurchaseDate = new System.DateTime(12, 12, 12),
                Cost = 100
            };
            purchases.Add(purchase);
            User user = new User()
            {
                Id = 1,
                Purchases = purchases
            };
            var mock = new Mock<IUserRepository>();

            UsersController controller = new UsersController(mock.Object);

            //Act
            var result = controller.isUserInvalid(user);

            //Assert
            Assert.False(result);
        }
    }
}
