namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public Guid EmailConfirmationToken { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public Guid RefreshTokenId { get; set; }
        public RefreshToken RefreshToken { get; set; } = null!;
        public IList<UserRole>? UserRoles { get; set; }
        public IList<Ad>? Ads { get; set; }
    }
}
