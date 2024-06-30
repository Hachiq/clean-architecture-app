using Application.Interfaces;
using Infrastructure.Services;
using Moq;

namespace UnitTests.Services
{
    public class RefreshTokenServiceTest
    {
        [Fact]
        public void CreateToken_ShouldReturnValidRefreshToken()
        {
            // Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var fixedDateTime = new DateTime(2023, 1, 1);
            mockDateTimeProvider.Setup(x => x.UtcNow).Returns(fixedDateTime);

            var refreshTokenService = new RefreshTokenService(mockDateTimeProvider.Object);

            // Act
            var refreshToken = refreshTokenService.CreateToken();

            // Assert
            Assert.NotEqual(Guid.Empty, refreshToken.Id);
            Assert.NotEmpty(refreshToken.Token);
            Assert.Equal(fixedDateTime, refreshToken.CreatedAt);
            Assert.Equal(fixedDateTime.AddDays(30), refreshToken.ExpiresAt);
        }
    }
}