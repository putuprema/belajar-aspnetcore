using System;
using System.Threading.Tasks;
using AspNetAuth.API.Interfaces;
using AspNetAuth.Shared.Classes.Request;
using AspNetAuth.Shared.Classes.Response;
using AspNetAuth.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAuth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("{userId}/Status")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ChangeUserStatus([FromQuery] bool active, string userId)
        {
            try
            {
                await _userService.ChangeUserActiveStatus(userId, active);
                return Ok();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }

        [HttpGet("Me")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            try
            {
                var result = await _userService.GetCurrentUserProfile();
                return Ok(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest dto)
        {
            try
            {
                var result = await _userService.Login(dto);
                return Ok(result);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest dto)
        {
            try
            {
                await _userService.RegisterUser(dto);
                return Ok(new BaseResponse("Register user success"));
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new BaseResponse(e, e.Message));
            }
        }
    }
}