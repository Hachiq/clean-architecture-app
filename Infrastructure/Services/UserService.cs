using Application.Interfaces.Users;
using Application.Repositories;

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
    }
}