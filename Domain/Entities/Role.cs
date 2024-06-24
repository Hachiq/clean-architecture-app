namespace Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public IList<UserRole>? UserRoles { get; set; }
    }
}
