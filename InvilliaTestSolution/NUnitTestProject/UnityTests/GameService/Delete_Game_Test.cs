using Domain.Interfaces;
using Domain.Model.Entity;
using Domain.Service;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.GameServiceTest
{
    [TestFixture]
    public class Delete_Game_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IGameService gameService;
        private int gameId = 1;
        private string gameName = "Game Test";


        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.Games.Delete(It.IsAny<int>())).ReturnsAsync(new Game(gameName));

            gameService = new GameService(unityOfWorkMock.Object);
        }

        [Test]
        public async Task Should_Delete_Game()
        {
            //Arrange
            //Act
            var serviceResult = await gameService.DeleteGame(gameId);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(gameName, serviceResult.Result.GameName);

        }

        [Test]
        public async Task Should_Validate_Unexisting_Game()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.Delete(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await gameService.DeleteGame(gameId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Game Not Found");
        }
    }
}