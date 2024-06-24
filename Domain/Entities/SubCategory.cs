namespace Domain.Entities
{
    public class SubCategory
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public IList<Ad> Ads { get; set; } = new List<Ad>();
    }
}
