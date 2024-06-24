namespace Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public Guid SectionId { get; set; }
        public Section Section { get; set; } = null!;
        public IList<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
