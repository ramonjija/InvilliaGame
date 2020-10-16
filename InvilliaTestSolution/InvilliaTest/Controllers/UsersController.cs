using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Security;
using Domain.Interfaces;
using Domain.Model.Aggregate;
using Domain.Model.Entity;
using InvilliaTest.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using AutoMapper;
using InvilliaTest.Mapping;
using InvilliaTest.DTO.Response;

namespace InvilliaTest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : BaseController
    {
        private readonly ILogger<UsersController> _logger;
        private IUserService _userservice;
        private readonly IMapper _mapper;


        public UsersController(ILogger<UsersController> logger,
            IUserService userService,
            IMapper mapper)
        {
            _logger = logger;
            _userservice = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// This Route is Responsible to Return a User by Id
        /// </summary>
        /// <param name="userId">Id of The User</param>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute(Name = "userId")] int userId)
        {
            try
            {
                if (GetUserId() != userId && GetUserRole() != Roles.Administrator)
                    return Forbid();

                var serviceResult = await _userservice.GetUser(userId);
                if (!serviceResult.Success)
                {
                    return NotFound();
                }
                var mappedUser = _mapper.Map<UserDto>(serviceResult.Result);

                return Ok(mappedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Route is Responsible to Return All Users
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var serviceResult = await _userservice.GetUser();
                var mappedUser = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(serviceResult.Result);

                return Ok(mappedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Route is Responsible for the Login of the User
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoggedUserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(LoggedUserDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LogUserDto logUserDto)
        {
            try
            {
                var serviceResult = await _userservice.GetUser(logUserDto.UserName, logUserDto.Password);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var token = TokenService.GenerateToken(serviceResult.Result);

                return Ok(new LoggedUserDto(serviceResult.Result.UserName, token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Route is Used to Create Users
        /// </summary>
        /// <param name="postUserDto">
        /// Name = Name of the User
        /// Password = Password of the User
        /// UserType = Type of User:
        /// 1 - Administrator
        /// 2 - Friend
        /// </param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] PostUserDto postUserDto)
        {
            try
            {
                var serviceResult = await _userservice.CreateUser(postUserDto.Name, postUserDto.Password, (int)postUserDto.UserType);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var result = _mapper.Map<UserDto>(serviceResult.Result);

                return Created($"User: {result.UserId}", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Route Is Used to Edit Users
        /// </summary>
        /// <param name="userId">Id of The User</param>
        /// <param name="putUserDto">
        /// Name = Name of the User
        /// Password = Password of the User
        /// UserType = Type of User:
        /// 1 - Administrator
        /// 2 - Friend
        /// </param>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Put([FromRoute(Name = "userId")] int userId, [FromBody] PutUserDto putUserDto)
        {
            try
            {
                if (GetUserId() != userId && GetUserRole() != Roles.Administrator)
                    return Forbid();

                var serviceResult = await _userservice.UpdateUser(userId, putUserDto.Name, putUserDto.Password, putUserDto.UserTypeId);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);

                var result = _mapper.Map<UserDto>(serviceResult.Result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This Route is Responsible to Delete an User
        /// </summary>
        /// <param name="userId">Id of the User</param>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute(Name = "userId")] int userId)
        {
            try
            {
                if (GetUserId() != userId && GetUserRole() != Roles.Administrator)
                    return Forbid();

                var serviceResult = await _userservice.DeleteUser(userId);
                if (!serviceResult.Success)
                    return BadRequest(serviceResult.ValidationMessages);
                
                var result = _mapper.Map<UserDto>(serviceResult.Result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
