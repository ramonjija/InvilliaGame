using Domain.Model.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model.Entity
{
    public class Friend : User
    {
        public Friend()
        {
            BorrowedGames = new List<BorrowedGame>();
        }
        public Friend(User user)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            UserType = user.UserType;
            PasswordHash = user.PasswordHash;
            BorrowedGames = new List<BorrowedGame>();
        }
        public virtual IList<BorrowedGame> BorrowedGames { get; set; }
    }
}
