using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvilliaTest.DTO.Response
{
    public class GetBorrowedGameDto
    {
        public int BorrowedGameId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }

    }
}
