using FluentAssertions;
using SMEAppHouse.Core.CustomAuth.Models;
using Xunit;

namespace SMEAppHouse.Core.CustomAuth.Tests;

public class UserGrantTests
{
    [Fact]
    public void UserGrant_ShouldInitializeWithDefaultValues()
    {
        // Act
        var userGrant = new UserGrant();

        // Assert
        userGrant.Should().NotBeNull();
        userGrant.UserId.Should().Be(0);
        userGrant.Disabled.Should().BeFalse();
        userGrant.ClientId.Should().Be(0);
    }

    [Fact]
    public void UserGrant_ShouldSetAllProperties()
    {
        // Arrange
        var userGrant = new UserGrant
        {
            UserId = 1,
            Username = "testuser",
            Disabled = false,
            Email = "test@example.com",
            Password = "password123",
            ClientId = 100,
            OptOutEmail = "optout@example.com",
            AllowedCountries = "US,CA",
            AuthToken = "auth_token_123",
            DateTimeCreated = new DateTime(2024, 1, 1),
            AvatarUrl = "https://example.com/avatar.jpg",
            Language_ID = "en",
            PersistentToken = "persistent_token_123"
        };

        // Assert
        userGrant.UserId.Should().Be(1);
        userGrant.Username.Should().Be("testuser");
        userGrant.Disabled.Should().BeFalse();
        userGrant.Email.Should().Be("test@example.com");
        userGrant.Password.Should().Be("password123");
        userGrant.ClientId.Should().Be(100);
        userGrant.OptOutEmail.Should().Be("optout@example.com");
        userGrant.AllowedCountries.Should().Be("US,CA");
        userGrant.AuthToken.Should().Be("auth_token_123");
        userGrant.DateTimeCreated.Should().Be(new DateTime(2024, 1, 1));
        userGrant.AvatarUrl.Should().Be("https://example.com/avatar.jpg");
        userGrant.Language_ID.Should().Be("en");
        userGrant.PersistentToken.Should().Be("persistent_token_123");
    }
}

