using Application.Services.AccessTokenService;
using Application.Services.DateTimeProvider;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UnitTests.Services
{
    public class AccessTokenServiceTest
    {
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly AccessTokenService _accessTokenService;
        private readonly User _testUser;
        private readonly List<Role> _testRoles;
        private readonly DateTime _fixedDateTime;

        public AccessTokenServiceTest()
        {
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
            _mockConfiguration = new Mock<IConfiguration>();

            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(x => x.Value).Returns("the_secret_authentication_key_must_be_sixty_four_characters_long");

            _mockConfiguration.Setup(x => x.GetSection("AppSettings:Token")).Returns(mockConfigurationSection.Object);

            _accessTokenService = new AccessTokenService(_mockDateTimeProvider.Object);

            _testUser = new User { Id = Guid.NewGuid(), Username = "testuser" };
            _testRoles = new List<Role> { new Role { Name = "Admin" }, new Role { Name = "User" } };

            _fixedDateTime = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            _mockDateTimeProvider.Setup(x => x.UtcNow).Returns(_fixedDateTime);
        }

        private JwtSecurityToken GetJwtToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            return jwtHandler.ReadJwtToken(token);
        }

        [Fact]
        public void CreateToken_ShouldReturnNonNullToken()
        {
            // Act
            var token = _accessTokenService.CreateToken(_testUser, _testRoles, _mockConfiguration.Object);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public void CreateToken_ShouldContainNameIdentifierClaim()
        {
            // Act
            var token = _accessTokenService.CreateToken(_testUser, _testRoles, _mockConfiguration.Object);
            var jwtToken = GetJwtToken(token);

            // Assert
            var nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Assert.Equal(_testUser.Id.ToString(), nameIdentifierClaim?.Value);
        }

        [Fact]
        public void CreateToken_ShouldContainNameClaim()
        {
            // Act
            var token = _accessTokenService.CreateToken(_testUser, _testRoles, _mockConfiguration.Object);
            var jwtToken = GetJwtToken(token);

            // Assert
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            Assert.Equal(_testUser.Username, nameClaim?.Value);
        }

        [Fact]
        public void CreateToken_ShouldContainRoleClaims()
        {
            // Act
            var token = _accessTokenService.CreateToken(_testUser, _testRoles, _mockConfiguration.Object);
            var jwtToken = GetJwtToken(token);

            // Assert
            var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
            Assert.Contains(roleClaims, c => c.Value == "Admin");
            Assert.Contains(roleClaims, c => c.Value == "User");
        }

        [Fact]
        public void CreateToken_ShouldHaveCorrectExpiration()
        {
            // Act
            var token = _accessTokenService.CreateToken(_testUser, _testRoles, _mockConfiguration.Object);
            var jwtToken = GetJwtToken(token);

            // Assert
            Assert.Equal(_fixedDateTime.AddMinutes(7), jwtToken.ValidTo);
        }
    }
}