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
    public class Get_User_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;

        private IUserService userService;
        private int userId = 1;
        private string userName = "User Test";
        private string password = "1234";
        private string hashPassword = new PasswordService().HashPassword("1234");

        private UserType userType = new UserType();


        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.UserTypes.GetById(It.IsAny<int>())).ReturnsAsync(userType);
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<int>())).ReturnsAsync(new User(userName,userType, hashPassword));
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new User(userName, userType, hashPassword));
            unityOfWorkMock.Setup(c => c.Users.GetUsersWithType()).ReturnsAsync(new List<User>() { new User(userName, userType, hashPassword) });
            unityOfWorkMock.Setup(c => c.Users.Get()).ReturnsAsync(new List<User>() { new User(userName, userType, hashPassword) });
            userService = new UserService(unityOfWorkMock.Object, new PasswordService());
        }

        [Test]
        public async Task Should_Get_User_by_Id()
        {
            //Arrange
            //Act
            var serviceResult = await userService.GetUser(userId);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(userName, serviceResult.Result.UserName);
            Assert.IsTrue(new PasswordService().IsPasswordValid(password, serviceResult.Result.PasswordHash));
            Assert.AreEqual(userType, serviceResult.Result.UserType);

        }

        [Test]
        public async Task Should_Get_User_by_Name_Password()
        {
            //Arrange
            //Act
            var serviceResult = await userService.GetUser(userName, password);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(userName, serviceResult.Result.UserName);
            Assert.IsTrue(new PasswordService().IsPasswordValid(password, serviceResult.Result.PasswordHash));
            Assert.AreEqual(userType, serviceResult.Result.UserType);

        }

        [Test]
        public async Task Should_Get_All_Users()
        {
            //Arrange
            var expectedUser = new User(userName, userType, hashPassword);

            //Act
            var serviceResult = await userService.GetUser();
            var user = serviceResult.Result.FirstOrDefault();

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            user.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task Should_Validate_Unexisting_User()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await userService.GetUser(userId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "User Not Found");

        }

        [Test]
        public async Task Should_Validate_Unexisting_User_By_Name_Password()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await userService.GetUser(userName, password);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "User Not Found");
        }

        [Test]
        public async Task Should_Validate_User_With_Incorrect_Password()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new User(userName, userType, hashPassword));

            //Act
            var serviceResult = await userService.GetUser(userName, hashPassword);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Incorrect Password");
        }
    }
}