using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contributors.Authorization;

namespace Presentation.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthorizationController(
            IUserRepository userRepository,
            IRolesRepository rolesRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IPasswordService passwordService,
            IAccessTokenGenerator accessTokenGenerator,
            IRefreshTokenService refreshTokenService,
            IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordService = passwordService;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenService = refreshTokenService;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            _passwordService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                EmailConfirmed = false,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _userRepository.AddAsync(user);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user is null || _passwordService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("User not found or wrong password"); // Use filters instead of this 'if' check
            }

            var roles = await _rolesRepository.GetByUserIdAsync(user.Id);

            var newRefreshToken = _refreshTokenService.CreateToken();

            SetCookiesRefreshToken(newRefreshToken);
            await _userRepository.UpdateUserRefreshToken(user, newRefreshToken);

            var jwt = _accessTokenGenerator.CreateToken(user, roles);

            return Ok(jwt);
        }

        [HttpPost("logout")] // Use filters for checking if token is not null
        public async Task<ActionResult> Logout()
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(Request.Cookies["refreshToken"]);

            await _refreshTokenRepository.ExpireRefreshToken(refreshToken);

            refreshToken.ExpiresAt = _dateTimeProvider.UtcNow;
            SetCookiesRefreshToken(refreshToken);

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