using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvilliaTest.Request
{
    public class BorrowGameDto
    {
        public IEnumerable<int> GamesIds { get; set; }
    }
}
