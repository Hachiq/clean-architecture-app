namespace Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserProfileResponse> GetUserByIdAsync(Guid id);
        Task UpdateUserContactsAsync(Guid id, UserContactsRequest contacts);
        Task UpdateUserEmailAsync(Guid id, UserEmailRequest request);
    }
}