using Domain.Interfaces;
using Domain.Model.Entity;
using Domain.Service;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.GameServiceTest
{
    [TestFixture]
    public class Get_Game_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IGameService gameService;
        private int gameId = 1;
        private string gameName = "Game Test";


        [SetUp]
        public void Setup()
        {
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.IsAny<int>())).ReturnsAsync(new Game(gameName));
            unityOfWorkMock.Setup(c => c.Games.Get()).ReturnsAsync(new List<Game>() { new Game(gameName) });
            unityOfWorkMock.Setup(c => c.Games.GetGameByName(It.IsAny<string>())).ReturnsAsync(new Game(gameName));
            unityOfWorkMock.Setup(c => c.Games.GetGamesWithBorrowed()).ReturnsAsync(new List<Game>() { new Game(gameName) });

            gameService = new GameService(unityOfWorkMock.Object);
        }

        [Test]
        public async Task Should_Get_Game()
        {
            //Arrange
            //Act
            var serviceResult = await gameService.GetGame(gameId);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(gameName, serviceResult.Result.GameName);
        }

        [Test]
        public async Task Should_Get_Game_by_Name()
        {
            //Arrange
            //Act
            var serviceResult = await gameService.GetGame(gameName);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(gameName, serviceResult.Result.GameName);
        }

        [Test]
        public async Task Should_Get_All_Games()
        {
            //Arrange
            var expectedGame = new Game(gameName);
            //Act
            var serviceResult = await gameService.GetGame();
            var game = serviceResult.Result.FirstOrDefault();

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
            game.Should().BeEquivalentTo(expectedGame);
        }

        [Test]
        public async Task Should_Validate_Unexisting_Game()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await gameService.GetGame(gameId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Game Not Found");

        }
        [Test]
        public async Task Should_Validate_Unexisting_User_By_Name_Password()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByName(It.IsAny<string>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await gameService.GetGame(gameName);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Game Not Found");
        }
    }
}