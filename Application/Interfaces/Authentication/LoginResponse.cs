using Domain.Entities;

namespace Application.Interfaces.Authentication
{
    public record LoginResponse(string JWT, RefreshToken RefreshToken);
}