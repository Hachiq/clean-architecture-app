using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Services.AccessTokenService
{
    public interface IAccessTokenService
    {
        string CreateToken(User user, IList<Role> roles, IConfiguration configuration);
    }
}