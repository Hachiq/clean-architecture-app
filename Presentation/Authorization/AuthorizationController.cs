using Application.Interfaces.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Presentation.Authorization.Filters;

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

        [HttpPost("register")]
        [ServiceFilter(typeof(UsernameInvalidFilter))]
        [ServiceFilter(typeof(PasswordTooShortFilter))]
        [ServiceFilter(typeof(UsernameTakenFilter))]
        [ServiceFilter(typeof(EmailTakenFilter))]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            await _authenticationService.RegisterUserAsync(request);

            return Ok();
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(UsernameInvalidFilter))]
        [ServiceFilter(typeof(PasswordTooShortFilter))]
        [ServiceFilter(typeof(UserNotFoundOrWrongPasswordFilter))]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginUserAsync(request);

            SetCookiesRefreshToken(result.RefreshToken);

            return Ok(result.JWT);
        }

        [HttpPost("logout")]
        [ServiceFilter(typeof(RefreshTokenInvalidFilter))]
        public async Task<ActionResult> Logout()
        {
            await _authenticationService.LogoutAsync(Request.Cookies["refreshToken"]);

            Response.Cookies.Delete("refreshToken");

            return Ok();
        }

        [HttpGet("refresh-token")]
        [ServiceFilter(typeof(RefreshTokenInvalidFilter))]
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