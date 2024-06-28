using Application.Services.PasswordService;

namespace UnitTests.Services
{
    public class PasswordServiceTest
    {
        private readonly IPasswordService _passwordService;

        public PasswordServiceTest()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void CreatePasswordHash_ShouldCreateHasAndSalt()
        {
            // Arrange
            var password = "testingPassword";

            // Act
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Assert
            Assert.NotNull(passwordHash);
            Assert.NotNull(passwordSalt);
            Assert.NotEmpty(passwordHash);
            Assert.NotEmpty(passwordSalt);
        }

        [Fact]
        public void VerifyPasswordHash_ShouldReturnTrue_ForValidPassword()
        {
            // Arrange
            var password = "testingPassword";
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Act
            var result = _passwordService.VerifyPasswordHash(password, passwordHash, passwordSalt);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPasswordHash_ShouldReturnFalse_ForInvalidPassword()
        {
            // Arrange
            var password = "testingPassword";
            var wrongPassword = "wrongTestingPassword";
            _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Act
            var result = _passwordService.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

            // Assert
            Assert.False(result);
        }
    }
}