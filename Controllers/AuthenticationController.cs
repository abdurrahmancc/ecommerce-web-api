using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce_web_api.Helper;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/auth/")]
    public class AuthenticationController : ControllerBase
    {

        private readonly JwtService _jwtService;

        public AuthenticationController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginInfo)
        {
                var token = _jwtService.GenerateJwtToken(loginInfo.Email);
                return Ok(new { Token = token });
            
        }


    }
}