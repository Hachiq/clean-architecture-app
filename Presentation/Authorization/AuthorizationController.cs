using Application.Interfaces;
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
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthorizationController(IAuthenticationService authenticationService, IDateTimeProvider dateTimeProvider)
        {
            _authenticationService = authenticationService;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet("get-current-user")]
        [ServiceFilter(typeof(RefreshTokenMissingFilter))]
        [ServiceFilter(typeof(RefreshTokenInvalidFilter))]
        public async Task<ActionResult> GetCurrentUser()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _authenticationService.GetCurrentUser(refreshToken);
            return Ok(new UserResponse(user.Username, user.Email, user.FirstName, user.LastName, user.Phone));
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

        [HttpGet("logout")]
        [ServiceFilter(typeof(RefreshTokenInvalidFilter))]
        public async Task<ActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            await _authenticationService.LogoutAsync(refreshToken);

            ExpireCookiesRefreshToken(refreshToken);

            return Ok();
        }

        [HttpGet("refresh-token")]
        [ServiceFilter(typeof(RefreshTokenMissingFilter))]
        [ServiceFilter(typeof(RefreshTokenInvalidFilter))]
        public async Task<ActionResult> RefreshToken()
        {
            var jwt = await _authenticationService.RefreshTokenAsync(Request.Cookies["refreshToken"]);
            return Ok(jwt);
        }

        [HttpGet("confirm-email")]
        [ServiceFilter(typeof(EmailAlreadyConfirmedFilter))]
        public async Task<ActionResult> ConfirmEmail([FromQuery]Guid userId, [FromQuery]Guid token)
        {
            await _authenticationService.ConfirmEmailAsync(userId, token);
            return Redirect("http://localhost:4200/home");
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

        private void ExpireCookiesRefreshToken(string? refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = _dateTimeProvider.UtcNow,
                SameSite = SameSiteMode.None,
                Secure = true
            };
            Response.Cookies.Append(nameof(refreshToken), refreshToken, cookieOptions);
        }
    }
}