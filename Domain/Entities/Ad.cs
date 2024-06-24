namespace Domain.Entities
{
    public class Ad
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Price { get; set; }
        public Guid UserId { get; set; }
        public Guid SubCategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AdPictureUrl { get; set; }
        public User User { get; set; } = null!;
        public SubCategory SubCategory { get; set; } = null!;
    }
}
