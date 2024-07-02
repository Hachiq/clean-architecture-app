using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Authorization
{
    [Route("auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")] // Use filters
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            await _authenticationService.RegisterUserAsync(request);

            return Ok();
        }

        [HttpPost("login")] // Use filters
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginUserAsync(request);

            SetCookiesRefreshToken(result.RefreshToken);

            return Ok(result.JWT);
        }

        [HttpPost("logout")] // Use filters for checking if token is not null
        public async Task<ActionResult> Logout()
        {
            await _authenticationService.LogoutAsync(Request.Cookies["refreshToken"]);

            Response.Cookies.Delete("refreshToken");

            return Ok();
        }

        [HttpGet("refresh-token")] // Use filters to check if token is expired
        public async Task<ActionResult> RefreshToken()
        {
            var jwt = await _authenticationService.RefreshTokenAsync(Request.Cookies["refreshToken"]);
            return Ok(jwt);
        }

        private void SetCookiesRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.ExpiresAt,
                SameSite = SameSiteMode.None,
                Secure = true
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}