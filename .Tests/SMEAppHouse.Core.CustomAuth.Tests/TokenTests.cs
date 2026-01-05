using FluentAssertions;
using Newtonsoft.Json;
using SMEAppHouse.Core.CustomAuth.Models;
using Xunit;

namespace SMEAppHouse.Core.CustomAuth.Tests;

public class TokenTests
{
    [Fact]
    public void Token_ShouldHaveJsonPropertyAttributes()
    {
        // Arrange
        var token = new Token
        {
            AccessToken = "access_token_123",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            RefreshToken = "refresh_token_123",
            UserData = "user_data_json"
        };

        // Act
        var json = JsonConvert.SerializeObject(token);
        var deserialized = JsonConvert.DeserializeObject<Token>(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.AccessToken.Should().Be("access_token_123");
        deserialized.TokenType.Should().Be("Bearer");
        deserialized.ExpiresIn.Should().Be(3600);
        deserialized.RefreshToken.Should().Be("refresh_token_123");
        deserialized.UserData.Should().Be("user_data_json");
    }

    [Fact]
    public void Token_JsonSerialization_ShouldUseCorrectPropertyNames()
    {
        // Arrange
        var token = new Token
        {
            AccessToken = "test_access",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            RefreshToken = "test_refresh",
            UserData = "test_data"
        };

        // Act
        var json = JsonConvert.SerializeObject(token);

        // Assert
        json.Should().Contain("access_token");
        json.Should().Contain("token_type");
        json.Should().Contain("expires_in");
        json.Should().Contain("refresh_token");
        json.Should().Contain("user_data");
    }

    [Fact]
    public void Token_JsonDeserialization_ShouldMapCorrectly()
    {
        // Arrange
        var json = @"{
            ""access_token"": ""test_access"",
            ""token_type"": ""Bearer"",
            ""expires_in"": 3600,
            ""refresh_token"": ""test_refresh"",
            ""user_data"": ""test_data""
        }";

        // Act
        var token = JsonConvert.DeserializeObject<Token>(json);

        // Assert
        token.Should().NotBeNull();
        token!.AccessToken.Should().Be("test_access");
        token.TokenType.Should().Be("Bearer");
        token.ExpiresIn.Should().Be(3600);
        token.RefreshToken.Should().Be("test_refresh");
        token.UserData.Should().Be("test_data");
    }
}

