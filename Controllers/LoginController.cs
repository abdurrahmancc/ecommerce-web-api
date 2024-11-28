using Ecommerce_web_api.Helper;
using Ecommerce_web_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_web_api.Controllers
{
    [ApiController]
    [Route("v1/api/login/")]
    public class LoginController: ControllerBase
    {

        private readonly JwtService _jwtService;

        public LoginController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Login( LoginModel loginData )
        {

            // Validate the input (basic check)
            if (loginData == null || string.IsNullOrWhiteSpace(loginData.Email))
            {
                return BadRequest("Invalid login data.");
            }
            var token = _jwtService.GenerateJwtToken(loginData.Email);

           return Ok(token);
        }
    }
}
