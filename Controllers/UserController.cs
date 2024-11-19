using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.DTOs.User;
using Ecommerce_web_api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/users/")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _UserService;

        public UserController(IUserService userService)
        {
            _UserService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _UserService.GetAllUsersService();

            return Ok(ApiResponse<List<UserReadDto>>.SuccessResponse(result, 200, "Users is returned successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsers([FromBody] UserCreateDto userCreateData)
        {
            var result = await _UserService.CreateUsersService(userCreateData);

            return Ok(ApiResponse<UserReadDto>.SuccessResponse(result, 200, "Users is returned successfully"));
        }

    }
}