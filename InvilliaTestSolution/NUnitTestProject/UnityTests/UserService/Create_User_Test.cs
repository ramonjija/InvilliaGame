using Domain.Interfaces;
using Domain.Model.Entity;
using Domain.Service;
using Moq;
using NUnit.Framework;
using Security;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.UserServiceTest
{
    [TestFixture]
    public class Create_User_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IUserService userService;
        private string userName = "User Test";
        private string password = "1234";
        private UserType userType = new UserType();


        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.UserTypes.GetById(It.IsAny<int>())).ReturnsAsync(userType);
            unityOfWorkMock.Setup(c => c.Users.GetUserByName(It.IsAny<string>())).ReturnsAsync(() => null);
            unityOfWorkMock.Setup(c => c.Users.Create(It.IsAny<User>())).ReturnsAsync(new User(userName, userType, password));

            userService = new UserService(unityOfWorkMock.Object, new PasswordService());
        }

        [Test]
        public async Task Should_Create_User()
        {
            //Arrange
            //Act
            var serviceResult = await userService.CreateUser(userName, password, 0);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(userName, serviceResult.Result.UserName);
            Assert.AreEqual(password, serviceResult.Result.PasswordHash);
            Assert.AreEqual(userType, serviceResult.Result.UserType);

        }

        [Test]
        public async Task Should_Create_Friend()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.UserTypes.GetById(It.IsAny<int>())).ReturnsAsync(new UserType(2));

            //Act
            var serviceResult = await userService.CreateUser(userName, password, 0);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(userName, serviceResult.Result.UserName);
            Assert.AreEqual(password, serviceResult.Result.PasswordHash);
            Assert.AreEqual(userType, serviceResult.Result.UserType);

        }

        [Test]
        public async Task Should_Validate_Existing_User()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.GetUserByName(It.IsAny<string>())).ReturnsAsync(new User(userName, userType, password));

            //Act
            var serviceResult = await userService.CreateUser(userName, password, 0);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), $"There's already a user with this name. 'Name: {userName}'");

        }

        [Test]
        public async Task Should_Validate_Unexisting_UserType()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.UserTypes.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await userService.CreateUser(userName, password, 0);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), $"User Type Not Found. 'UserTypeId: {userType.TypeId}'");

        }
    }
}