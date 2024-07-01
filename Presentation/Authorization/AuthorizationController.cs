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
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAuthenticationService _authenticationService;

        public AuthorizationController(
            IUserRepository userRepository,
            IRolesRepository rolesRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAccessTokenGenerator accessTokenGenerator,
            IDateTimeProvider dateTimeProvider,
            IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _dateTimeProvider = dateTimeProvider;
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

        [HttpGet("refresh-token/{userId}")]
        public async Task<ActionResult> RefreshToken(Guid userId)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(Request.Cookies["refreshToken"]);

            var user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (refreshToken?.ExpiresAt < _dateTimeProvider.UtcNow)
            {
                return Unauthorized("Token expired.");
            }

            var roles = await _rolesRepository.GetByUserIdAsync(userId);

            string token = _accessTokenGenerator.CreateToken(user, roles);
            return Ok(token);
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