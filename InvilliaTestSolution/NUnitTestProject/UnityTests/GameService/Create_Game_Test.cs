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
    public class Create_Game_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IGameService gameService;
        private string gameName = "Game Test";


        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.Games.GetGameByName(It.IsAny<string>())).ReturnsAsync(() => null);
            unityOfWorkMock.Setup(c => c.Games.Create(It.IsAny<Game>())).ReturnsAsync(new Game(gameName));

            gameService = new GameService(unityOfWorkMock.Object);
        }

        [Test]
        public async Task Should_Create_Game()
        {
            //Arrange
            //Act
            var serviceResult = await gameService.CreateGame(gameName);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(gameName, serviceResult.Result.GameName);

        }

        [Test]
        public async Task Should_Validate_Existing_Game()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByName(It.IsAny<string>())).ReturnsAsync(new Game(gameName));

            //Act
            var serviceResult = await gameService.CreateGame(gameName);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), $"There's already a Game with this name. 'Name: {gameName}'");
        }
    }
}