using Domain.Entities;

namespace Application.Interfaces
{
    public interface IAccessTokenGenerator
    {
        string CreateToken(User user, IList<Role> roles);
    }
}