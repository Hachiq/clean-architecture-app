namespace Domain.Entities
{
    public class Section
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public IList<Category> Categories { get; set; } = new List<Category>();
    }
}
