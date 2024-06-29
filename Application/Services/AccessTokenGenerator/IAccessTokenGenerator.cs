using Domain.Entities;

namespace Application.Services.AccessTokenGenerator
{
    public interface IAccessTokenGenerator
    {
        string CreateToken(User user, IList<Role> roles);
    }
}