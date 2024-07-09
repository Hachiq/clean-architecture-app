namespace Application.Interfaces.Users
{
    public record UserProfileResponse(
        Guid Id,
        string Username,
        string Email,
        string? FirstName,
        string? LastName,
        string? Phone,
        string? ProfilePictureUrl
    );
}