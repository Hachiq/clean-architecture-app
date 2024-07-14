using Application.Interfaces;
using Application.Interfaces.Users;
using Application.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IEmailSender _emailSender;
        private readonly IUserRoleService _userRoleService;

        public UserService(
            IUsersRepository usersRepository,
            IEmailSender emailSender,
            IUserRoleService userRoleService)
        {
            _usersRepository = usersRepository;
            _emailSender = emailSender;
            _userRoleService = userRoleService;
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

        public async Task UpdateUserEmailAsync(Guid id, UserEmailRequest request)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            if (request.Email is not null)
            {
                user.Email = request.Email;
                user.EmailConfirmed = false;
                await _userRoleService.RemoveFromConfirmedUserRole(user.Id);
                var confirmationLink = $"https://localhost:7035/auth/confirm-email?userId={user.Id}&token={user.EmailConfirmationToken}";
                await _emailSender.SendEmailAsync(user.Email, "Email confirmation", "To complete the registration, please follow the confirmation link: " + confirmationLink);
                await _usersRepository.UpdateAsync(user);
            }
        }
    }
}