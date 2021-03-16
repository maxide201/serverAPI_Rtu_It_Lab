using Moq;
using UserApp.Controllers;
using UserApp.Database;
using UserApp.Models;
using Xunit;

namespace UserApp.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void isUserValidReturnTrue()
        {
            var mock = new Mock<IUserRepository>();
            UsersController controller = new UsersController(mock.Object);
            User user = new User() { Name = "maxim", Password = "secret" };

            bool result = controller.isUserValid(user);

            Assert.True(result);
        }

        [Fact]
        public void isUserValidReturnFalse()
        {
            var mock = new Mock<IUserRepository>();
            UsersController controller = new UsersController(mock.Object);
            User user = new User();

            bool result = controller.isUserValid(user);

            Assert.False(result);
        }

        [Fact]
        public void getPurchasesReturnCode403()
        {
            var mock = new Mock<IUserRepository>();
            mock.Setup(f => f.isUserPasswordRight(new User())).Returns(false);
            UsersController controller = new UsersController(mock.Object);
            User user = new User() { Id = 1, Name = "a", Password = "a" };

            var result = controller.GetPurchases(user);
            var expected = controller.StatusCode(403);

            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void postUserReturnCode400()
        {
            var mock = new Mock<IUserRepository>();
            UsersController controller = new UsersController(mock.Object);
            User user = new User();

            var result = controller.PostUser(user);
            var expected = controller.StatusCode(400);

            Assert.Equal(expected.StatusCode, result.StatusCode);
        }

        [Fact]
        public void deleteUserReturnCode400()
        {
            var mock = new Mock<IUserRepository>();
            UsersController controller = new UsersController(mock.Object);
            User user = new User();

            var result = controller.DeleteUser(user);
            var expected = controller.StatusCode(400);

            Assert.Equal(expected.StatusCode, result.StatusCode);
        }
    }
}
