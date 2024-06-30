using Infrastructure.Services;

namespace UnitTests.Services
{
    public class DateTimeProviderTest
    {
        private readonly DateTimeProvider _dateTimeProvider;

        public DateTimeProviderTest()
        {
            _dateTimeProvider = new DateTimeProvider();
        }

        [Fact]
        public void UtcNow_ShouldReturnCurrentUtcDateTime()
        {
            // Arrange
            var before = DateTime.UtcNow;

            // Act
            var result = _dateTimeProvider.UtcNow;

            // Assert
            var after = DateTime.UtcNow;

            Assert.InRange(result, before, after);
        }
    }
}