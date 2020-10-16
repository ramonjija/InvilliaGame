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
    public class Update_User_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IUserService userService;
        private int userId = 0;
        private string userName = "User Test";
        private string updatedUserName = "Updated Test";
        private string password = "1234";
        private UserType userType = new UserType();


        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.UserTypes.GetById(It.IsAny<int>())).ReturnsAsync(userType);
            unityOfWorkMock.Setup(c => c.Users.GetById(It.IsAny<int>())).ReturnsAsync(new User(userName, userType, password));
            unityOfWorkMock.Setup(c => c.Users.Update(It.IsAny<User>())).Returns(new User(updatedUserName, userType, password));

            userService = new UserService(unityOfWorkMock.Object, new PasswordService());
        }

        [Test]
        public async Task Should_Update_User()
        {
            //Arrange
            //Act
            var serviceResult = await userService.UpdateUser(userId, userName, password, userType.TypeId);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreNotEqual(userName, serviceResult.Result.UserName);
            Assert.AreEqual(updatedUserName, serviceResult.Result.UserName);
            Assert.AreEqual(password, serviceResult.Result.PasswordHash);
            Assert.AreEqual(userType, serviceResult.Result.UserType);

        }

        [Test]
        public async Task Should_Validate_Unexisting_User()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await userService.UpdateUser(userId, userName, password, userType.TypeId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), $"User Not Found '{userName}'");
        }

        [Test]
        public async Task Should_Validate_Unexisting_UserType()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.UserTypes.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await userService.UpdateUser(userId, userName, password, userType.TypeId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), $"User Type Not Found. 'UserTypeId: {userType.TypeId}'");

        }
    }
}