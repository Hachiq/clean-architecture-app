namespace Application.Interfaces.Authentication
{
    public record RegisterRequest(string Username, string Email, string Password);
}