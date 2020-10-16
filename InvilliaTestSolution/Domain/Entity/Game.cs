using Domain.Model.Aggregate;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Model.Entity
{
    public class Game
    {
        public int GameId { get; private set; }
        public string GameName { get; private set; }
        public bool Available { get; private set; }
        public BorrowedGame BorrowedGame { get; private set; }

        public Game()
        {
        }

        public Game(string gameName)
        {
            GameName = gameName;
            Available = true;
        }

        public void Update(string gameName)
        {
            GameName = gameName;
        }

        public void Borrow(BorrowedGame borrowedGame)
        {
            this.BorrowedGame = borrowedGame;
            this.Available = false;
        }
        public void Return()
        {
            BorrowedGame = null;
            Available = true;
        }

    }

}