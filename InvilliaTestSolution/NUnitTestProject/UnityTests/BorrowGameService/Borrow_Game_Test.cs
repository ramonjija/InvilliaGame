using Domain.Interfaces;
using Domain.Model.Aggregate;
using Domain.Model.Entity;
using Domain.Service;
using Domain.Service.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.BorrowGameServiceTest
{
    [TestFixture]
    public class Borrow_Game_Test
    {
        private Mock<IUnityOfWork> unityOfWorkMock;
        private IBorrowGameService borrowGameService;
        private int gameId = 1;
        private string gameName = "Game Test";
        private int userId = 1;
        private string userName = "User Test";
        private Friend friend;
        private Game game;
        private Game borrowedGame;

        [SetUp]
        public void Setup()
        {
            var user = new User(userName, new UserType(2), "1234");
            friend = new Friend(user);
            game = new Game(gameName);
            borrowedGame = new Game(gameName);
            unityOfWorkMock = new Mock<IUnityOfWork>();
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<int>())).ReturnsAsync(friend);
            unityOfWorkMock.Setup(c => c.Games.GetById(It.IsAny<int>())).ReturnsAsync(game);
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.IsAny<int>())).ReturnsAsync(game);
            unityOfWorkMock.Setup(c => c.BorrowedGames.Create(It.IsAny<BorrowedGame>())).ReturnsAsync(new BorrowedGame(friend, borrowedGame));
            unityOfWorkMock.Setup(c => c.BorrowedGames.Delete(It.IsAny<int>())).ReturnsAsync(new BorrowedGame(friend, borrowedGame));
            unityOfWorkMock.Setup(c => c.BorrowedGames.GetBorrowedGameById(It.IsAny<int>())).ReturnsAsync(new BorrowedGame(friend, borrowedGame));
            unityOfWorkMock.Setup(c => c.BorrowedGames.GetBorrowedGamesByUserId(It.IsAny<int>())).ReturnsAsync(new List<BorrowedGame>() { new BorrowedGame(friend, borrowedGame) });
            borrowGameService = new BorrowGameService(unityOfWorkMock.Object);
        }

        [Test]
        public async Task Should_Borrow_Game()
        {
            //Arrange
            //Act
            var serviceResult = await borrowGameService.BorrowGame(userId, gameId);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
        }

        [Test]
        public async Task Should_Borrow_Games()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.Is<int>(c => c == 1))).ReturnsAsync(new Game("game 1"));
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.Is<int>(c => c == 2))).ReturnsAsync(new Game("game 2"));

            //Act
            var serviceResult = await borrowGameService.BorrowGames(userId, new List<int> { 1,2 });

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsEmpty(serviceResult.ValidationMessages);
        }

        [Test]
        public async Task Should_Validate_User_Borrow_Game()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await borrowGameService.BorrowGame(userId, gameId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "User Not Found");
        }

        [Test]
        public async Task Should_Validate_User_Borrow_Games()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Users.GetUserWithType(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await borrowGameService.BorrowGames(userId, new List<int> { 1, 2 });

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "User Not Found");
        }

        [Test]
        public async Task Should_Validate_Game_Available_Borrow_Game()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await borrowGameService.BorrowGame(userId, gameId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Game Not Found");
        }

        [Test]
        public async Task Should_Validate_Game_Available_Borrow_Games()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var serviceResult = await borrowGameService.BorrowGames(userId, new List<int> { 1, 2 });

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Game Not Found");
        }

        [Test]
        public async Task Should_Validate_Game_Borrow_Game()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.IsAny<int>())).ReturnsAsync(new Game());

            //Act
            var serviceResult = await borrowGameService.BorrowGame(userId, gameId);

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Game Not Available");
        }

        [Test]
        public async Task Should_Validate_Game_Borrow_Games()
        {
            //Arrange
            unityOfWorkMock.Setup(c => c.Games.GetGameByIdWithBorrowed(It.IsAny<int>())).ReturnsAsync(new Game());

            //Act
            var serviceResult = await borrowGameService.BorrowGames(userId, new List<int> { 1, 2 });

            //Assert
            Assert.IsFalse(serviceResult.Success);
            Assert.IsNull(serviceResult.Result);
            Assert.IsNotEmpty(serviceResult.ValidationMessages);
            Assert.AreEqual(serviceResult.ValidationMessages.FirstOrDefault(), "Game Not Available");
        }


        [Test]
        public async Task Should_Validate_Get_Games_User()
        {
            //Arrange
            //Act
            var serviceResult = await borrowGameService.GetBorrowGamesOfUser(userId);

            //Assert
            Assert.IsTrue(serviceResult.Success);
            Assert.IsNotNull(serviceResult.Result);
            Assert.IsEmpty(serviceResult.ValidationMessages);
        }



    }
}