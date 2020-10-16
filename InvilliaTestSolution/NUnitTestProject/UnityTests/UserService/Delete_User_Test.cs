using Domain.Interfaces;
using Domain.Model.Entity;
using Domain.Service;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.UserServiceTest
{
    [TestFixture]
    public class Delete_User_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IUserService userService;
        private int userId = 1;
        private string userName = "User Test";
        private string password = "1234";
        private UserType userType = new UserType();


        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.Users.Delete(It.IsAny<int>())).ReturnsAsync(new User(userName, userType, password));

            userService = new UserService(unityOfWorkMock.Object, new PasswordService());
        }

        [Test]
        public async Task Should_Delete_User()
        {
            //Arrange
            //Act
            var serviceResult = await userService.DeleteUser(userId);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(userName, serviceResult.Result.UserName);
            Assert.AreEqual(password, serviceResult.Result.PasswordHash);
            Assert.AreEqual(userType, serviceResult.Result.UserType);

        }

        [Test]
        public async Task Should_Valide_Unexisting_User_Delete_()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.Delete(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await userService.DeleteUser(userId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "User Not Found");
        }
    }
}