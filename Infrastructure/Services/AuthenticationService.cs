using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Repositories;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordService _passwordService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;

        public AuthenticationService(
            IPasswordService passwordService,
            IRefreshTokenService refreshTokenService,
            IUserRepository userRepository,
            IRolesRepository rolesRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAccessTokenGenerator accessTokenGenerator)
        {
            _passwordService = passwordService;
            _refreshTokenService = refreshTokenService;
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
        }

        public async Task RegisterUserAsync(RegisterRequest request)
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

            var newRefreshToken = _refreshTokenService.CreateToken();

            user.RefreshToken = newRefreshToken;

            await _userRepository.AddAsync(user);
        }
        public async Task<LoginResponse> LoginUserAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            var roles = await _rolesRepository.GetByUserIdAsync(user.Id);
            var newRefreshToken = _refreshTokenService.CreateToken();

            await _refreshTokenRepository.UpdateRefreshToken(user.RefreshToken, newRefreshToken);

            var jwt = _accessTokenGenerator.CreateToken(user, roles);

            return new LoginResponse(jwt, newRefreshToken);
        }

        public async Task LogoutAsync(string? token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            await _refreshTokenRepository.ExpireRefreshToken(refreshToken);
        }

        public async Task<string> RefreshTokenAsync(string? token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            var user = await _userRepository.GetByRefreshTokenIdAsync(refreshToken.Id);
            var roles = await _rolesRepository.GetByUserIdAsync(user.Id);

            string jwt = _accessTokenGenerator.CreateToken(user, roles);
            return jwt;
        }
    }
}