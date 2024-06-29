using Application.Services.DateTimeProvider;

namespace Infrastructure.Services.DateTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}