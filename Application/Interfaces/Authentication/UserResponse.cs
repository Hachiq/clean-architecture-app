namespace Application.Interfaces.Authentication
{
    public record UserResponse(Guid Id, string Username, string Email, string? FirstName, string? LastName, string? Phone);
}