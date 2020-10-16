using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvilliaTest.DTO.Response
{
    public class GetGameDto
    {
        public int GameId { get; set; }
        public string GameName { get; set; }
        public bool Available { get;  set; }
        public string BorrowedUserId { get; set; }
        public string BorrowedUserName { get; set; }

    }
}
