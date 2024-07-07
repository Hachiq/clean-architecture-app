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
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IUserRoleService _userRoleService;

        public AuthenticationService(
            IPasswordService passwordService,
            IRefreshTokenService refreshTokenService,
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository,
            IRefreshTokensRepository refreshTokensRepository,
            IAccessTokenGenerator accessTokenGenerator,
            IEmailSender emailSender,
            IUserRoleService userRoleService)
        {
            _passwordService = passwordService;
            _refreshTokenService = refreshTokenService;
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _refreshTokensRepository = refreshTokensRepository;
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

            await _usersRepository.AddAsync(user);

            await _userRoleService.AssignToUserRole(user.Id);
        }
        public async Task<LoginResponse> LoginUserAsync(LoginRequest request)
        {
            var user = await _usersRepository.GetByUsernameAsync(request.Username);

            var roles = await _rolesRepository.GetByUserIdAsync(user.Id);
            var newRefreshToken = _refreshTokenService.CreateToken();

            await _refreshTokensRepository.UpdateRefreshToken(user.RefreshToken, newRefreshToken);

            var jwt = _accessTokenGenerator.CreateToken(user, roles);

            return new LoginResponse(jwt, newRefreshToken);
        }

        public async Task LogoutAsync(string? token)
        {
            var refreshToken = await _refreshTokensRepository.GetByTokenAsync(token);
            await _refreshTokensRepository.ExpireRefreshToken(refreshToken);
        }

        public async Task<string> RefreshTokenAsync(string? token)
        {
            var refreshToken = await _refreshTokensRepository.GetByTokenAsync(token);
            var user = await _usersRepository.GetByRefreshTokenIdAsync(refreshToken.Id);
            var roles = await _rolesRepository.GetByUserIdAsync(user.Id);

            string jwt = _accessTokenGenerator.CreateToken(user, roles);
            return jwt;
        }

        public async Task ConfirmEmailAsync(Guid userId, Guid confirmationToken)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return;
            }
            if (user.EmailConfirmationToken.Equals(confirmationToken))
            {
                await _usersRepository.ConfirmEmailAsync(user);
                await _userRoleService.AssignToConfirmedUserRole(user.Id);
            }
        }
    }
}