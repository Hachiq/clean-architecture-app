using Application.Interfaces.Users;
using Application.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<UserProfileResponse> GetUserByIdAsync(Guid id)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            return new UserProfileResponse(
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Phone,
                user.ProfilePictureUrl
            );
        }

        public async Task UpdateUserContactsAsync(Guid id, UserContactsRequest contacts)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            if (!contacts.FirstName.IsNullOrEmpty())
            {
                user.FirstName = contacts.FirstName;
            }
            if (!contacts.LastName.IsNullOrEmpty())
            {
                user.LastName = contacts.LastName;
            }
            await _usersRepository.UpdateAsync(user);
        }
    }
}