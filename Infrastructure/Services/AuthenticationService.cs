using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Repositories;
using Domain.Entities;
using System.Net;

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
        private readonly IEmailSender _emailSender;
        private readonly IUserRoleService _userRoleService;

        public AuthenticationService(
            IPasswordService passwordService,
            IRefreshTokenService refreshTokenService,
            IUserRepository userRepository,
            IRolesRepository rolesRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IAccessTokenGenerator accessTokenGenerator,
            IEmailSender emailSender,
            IUserRoleService userRoleService)
        {
            _passwordService = passwordService;
            _refreshTokenService = refreshTokenService;
            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _emailSender = emailSender;
            _userRoleService = userRoleService;
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
                EmailConfirmationToken = Guid.NewGuid(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var newRefreshToken = _refreshTokenService.CreateToken();

            user.RefreshToken = newRefreshToken;

            var confirmationLink = $"https://localhost:7035/auth/confirm-email?userId={user.Id}&token={user.EmailConfirmationToken}";

            await _emailSender.SendEmailAsync(user.Email, "Email confirmation", "To complete the registration, please follow the confirmation link: " + confirmationLink);

            await _userRepository.AddAsync(user);

            await _userRoleService.AssignToUserRole(user.Id);
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

        public async Task ConfirmEmailAsync(Guid userId, Guid confirmationToken)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return;
            }
            if (user.EmailConfirmationToken.Equals(confirmationToken))
            {
                await _userRepository.ConfirmEmailAsync(user);
                await _userRoleService.AssignToConfirmedUserRole(user.Id);
            }
        }
    }
}