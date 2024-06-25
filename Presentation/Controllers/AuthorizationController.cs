using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contributors.Authorization;

namespace Presentation.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            return Ok();
        }

        [HttpGet("refresh-token")]
        public async Task<ActionResult> RefreshToken()
        {
            return Ok();
        }
    }
}