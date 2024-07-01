namespace Application.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task RegisterUserAsync(RegisterRequest request);
        Task<LoginResponse> LoginUserAsync(LoginRequest request);
        Task LogoutAsync(string? refreshToken);
    }
}