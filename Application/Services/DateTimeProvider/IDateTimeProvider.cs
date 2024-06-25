namespace Application.Services.DateTimeProvider
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}