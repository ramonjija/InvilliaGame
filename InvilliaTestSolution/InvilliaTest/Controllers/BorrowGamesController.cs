using Security;
using Domain.Interfaces;
using Domain.Model.Entity;
using InvilliaTest.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvilliaTest.Request;
using System.Linq;
using Domain.Model.Aggregate;
using System.Security.Claims;
using AutoMapper;
using InvilliaTest.DTO.Response;

namespace InvilliaTest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class BorrowGamesController : BaseController
    {
        private readonly ILogger<BorrowGamesController> _logger;
        private IBorrowGameService _borrowGamesService;
        private IMapper _mapper;

        public BorrowGamesController(ILogger<BorrowGamesController> logger,
            IBorrowGameService borrowGamesService,
            IMapper mapper)
        {
            _logger = logger;
            _borrowGamesService = borrowGamesService;
            _mapper = mapper;
        }
        /// <summary>
        /// This Route is Responsible to Return all Borrowed Games of the Current User
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<GetBorrowedGameDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetBorrowedGamesOfUser()
        {
            try
            {
                var serviceResult = await _borrowGamesService.GetBorrowGamesOfUser(GetUserId());
                if (!serviceResult.Success)
                {
                    return NotFound();
                }
                var borrowedGames = _mapper.Map<IList<BorrowedGame>, IList<GetBorrowedGameDto>>(serviceResult.Result);
                return Ok(borrowedGames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        ///<summary>
        /// This Route Is Used to Borrow a Game
        /// </summary>
        /// <param name="borrowedGamesIds">Ids of the Games who will be borrowed</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IList<CreateBorrowedGameDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles ="Friend")]
        public async Task<IActionResult> BorrowGame([FromBody]  List<int> borrowedGamesIds)
        {
            try
            {
                var serviceResult = await _borrowGamesService.BorrowGames(GetUserId(), borrowedGamesIds);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var result = _mapper.Map<IList<BorrowedGame>, IList<CreateBorrowedGameDto>>(serviceResult.Result);

                return Created($"BorrowGameId: {result.Select(c => c.BorrowedGameId).ToList()}", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        ///<summary>
        /// This Route Is Used to Borrow a Game
        /// </summary>
        /// <param name="returnedGamesIds">Ids of the Games who will be returned</param>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Friend")]
        public async Task<IActionResult> ReturnGame([FromBody] List<int> returnedGamesIds)
        {
            try
            {
                var serviceResult = await _borrowGamesService.ReturnGames(GetUserId(), returnedGamesIds);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var result = _mapper.Map<IList<BorrowedGame>, IList<ReturnBorrowedGameDto>>(serviceResult.Result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
