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
    public class Update_Game_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IGameService gameService;
        private int gameId = 1;
        private string gameName = "Game Test";
        private string updatedGameName = "Updated Test";



        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.Games.GetById(It.IsAny<int>())).ReturnsAsync(new Game(gameName));
            unityOfWorkMock.Setup(c => c.Games.GetGameByName(It.IsAny<string>())).ReturnsAsync(() => null);
            unityOfWorkMock.Setup(c => c.Games.Update(It.IsAny<Game>())).Returns(new Game(updatedGameName));

            gameService = new GameService(unityOfWorkMock.Object);

        }

        [Test]
        public async Task Should_Update_Game()
        {
            //Arrange
            //Act
            var serviceResult = await gameService.UpdateGame(gameId, gameName);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreNotEqual(gameName, serviceResult.Result.GameName);
            Assert.AreEqual(updatedGameName, serviceResult.Result.GameName);
        }

        [Test]
        public async Task Should_Validate_Unexisting_Game()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetById(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await gameService.UpdateGame(gameId, gameName);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), $"Game Not Found '{gameName}'");
        }
    }
}