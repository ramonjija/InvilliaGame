using Domain.Model.Entity;
using System;

namespace Domain.Model.Aggregate
{
    public class BorrowedGame
    {
        public int BorrowedGameId { get; private set; }
        public Friend Friend { get; private set; }
        public Game Game { get; private set; }
        public DateTime BorrowDate { get; private set; }
        public BorrowedGame()
        {
        }
        public BorrowedGame(Friend friend, Game game)
        {
            Friend = friend;
            Game = game;
            BorrowDate = DateTime.Now;
            if(friend != null)
                friend.BorrowedGames.Add(this);
            if (game != null)
                game.Borrow(this);
        }
    }
}
