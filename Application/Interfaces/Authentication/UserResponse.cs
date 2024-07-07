namespace Application.Interfaces.Authentication
{
    public record UserResponse(string Username, string Email, string? FirstName, string? LastName, string? Phone);
}