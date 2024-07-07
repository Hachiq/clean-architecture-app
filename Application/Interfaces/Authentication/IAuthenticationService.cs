using Domain.Entities;

namespace Application.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task<User> GetCurrentUser(string refreshToken);
        Task RegisterUserAsync(RegisterRequest request);
        Task<LoginResponse> LoginUserAsync(LoginRequest request);
        Task LogoutAsync(string? refreshToken);
        Task<string> RefreshTokenAsync(string? refreshToken);
        Task ConfirmEmailAsync(Guid userId, Guid confirmationToken);
    }
}