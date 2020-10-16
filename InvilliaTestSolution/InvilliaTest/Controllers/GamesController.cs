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
using AutoMapper;
using InvilliaTest.DTO.Response;

namespace InvilliaTest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class GamesController : BaseController
    {
        private readonly ILogger<GamesController> _logger;
        private IGameService _gamesService;
        private IMapper _mapper;

        public GamesController(ILogger<GamesController> logger,
            IGameService gamesService,
            IMapper mapper)
        {
            _logger = logger;
            _gamesService = gamesService;
            _mapper = mapper;
        }

        /// <summary>
        /// This Route is Responsible to Return a Game by Id
        /// </summary>
        /// <param name="gameId">Id of the Game</param>
        [HttpGet("{gameId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetGameDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute(Name = "gameId")] int gameId)
        {
            try
            {
                var serviceResult = await _gamesService.GetGame(gameId);
                if (!serviceResult.Success)
                {
                    return NotFound();
                }
                var game = _mapper.Map<GetGameDto>(serviceResult.Result);
                return Ok(game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// This Route is Responsible to Return All Games
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetGameDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var serviceResult = await _gamesService.GetGame();

                var games = _mapper.Map<IEnumerable<Game>, IEnumerable<GetGameDto>>(serviceResult.Result);

                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Route Is Used to Create a Game
        /// </summary>
        /// <param name="gameName">Name of the Game To be Created</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateGameDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Post([FromBody] string gameName)
        {
            try
            {
                var serviceResult = await _gamesService.CreateGame(gameName);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var result = _mapper.Map<CreateGameDto>(serviceResult.Result);

                return Created($"Game: {result.GameId}", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// This Route Is Used to Edit a Game
        /// </summary>
        /// <param name="gameId">Id of the Game</param>
        /// <param name="gameName">Updated Name of Game</param>
        [HttpPut("{gameId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateGameDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Put([FromRoute(Name = "gameId")] int gameId, [FromBody] string gameName)
        {

            try
            {
                var serviceResult = await _gamesService.UpdateGame(gameId, gameName);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var result = _mapper.Map<CreateGameDto>(serviceResult.Result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        /// <summary>
        /// This Route is Responsible to Delete a Game
        /// </summary>
        /// <param name="gameId">Id of the Game</param>
        [HttpDelete("{gameId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteGameDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete([FromRoute(Name = "gameId")] int gameId)
        {
            try
            {
                var serviceResult = await _gamesService.DeleteGame(gameId);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var result = _mapper.Map<DeleteGameDto>(serviceResult.Result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
