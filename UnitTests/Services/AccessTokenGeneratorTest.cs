using Application.Interfaces;
using Domain.Entities;
using Infrastructure.AccessTokens;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UnitTests.Services
{
    public class AccessTokenGeneratorTest
    {
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
        private readonly Mock<IOptions<JwtSettings>> _mockJwtOptions;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly User _testUser;
        private readonly List<Role> _testRoles;
        private readonly DateTime _fixedDateTime;
        private readonly JwtSettings _jwtSettings;

        public AccessTokenGeneratorTest()
        {
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
            _mockJwtOptions = new Mock<IOptions<JwtSettings>>();

            _jwtSettings = new JwtSettings
            {
                Secret = "the_secret_authentication_key_must_be_sixty_four_characters_long",
                Issuer = "Slando",
                Audience = "Slando",
                TokenExpirationInMinutes = 7
            };

            _mockJwtOptions.Setup(x => x.Value).Returns(_jwtSettings);

            _accessTokenGenerator = new AccessTokenGenerator(_mockDateTimeProvider.Object, _mockJwtOptions.Object);

            _testUser = new User { Id = Guid.NewGuid(), Username = "testuser", Email = "testuser@example.com" };
            _testRoles = new List<Role> { new Role { Name = "Admin" }, new Role { Name = "User" } };

            _fixedDateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
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
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);

            // Assert
            Assert.NotNull(token);
        }

        [Fact]
        public void CreateToken_ShouldContainIdClaim()
        {
            // Act
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);
            var jwtToken = GetJwtToken(token);

            // Assert
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "id");
            Assert.Equal(_testUser.Id.ToString(), idClaim?.Value);
        }

        [Fact]
        public void CreateToken_ShouldContainNameClaim()
        {
            // Act
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);
            var jwtToken = GetJwtToken(token);

            // Assert
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            Assert.Equal(_testUser.Username, nameClaim?.Value);
        }

        [Fact]
        public void CreateToken_ShouldContainEmailClaim()
        {
            // Act
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);
            var jwtToken = GetJwtToken(token);

            // Assert
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            Assert.Equal(_testUser.Email, emailClaim?.Value);
        }

        [Fact]
        public void CreateToken_ShouldContainRoleClaims()
        {
            // Act
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);
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
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);
            var jwtToken = GetJwtToken(token);

            // Assert
            Assert.Equal(_fixedDateTime.AddMinutes(_jwtSettings.TokenExpirationInMinutes), jwtToken.ValidTo);
        }

        [Fact]
        public void CreateToken_ShouldHaveCorrectIssuer()
        {
            // Act
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);
            var jwtToken = GetJwtToken(token);

            // Assert
            Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
        }

        [Fact]
        public void CreateToken_ShouldHaveCorrectAudience()
        {
            // Act
            var token = _accessTokenGenerator.CreateToken(_testUser, _testRoles);
            var jwtToken = GetJwtToken(token);

            // Assert
            Assert.Contains(jwtToken.Audiences, audience => audience == _jwtSettings.Audience);
        }
    }
}