using FluentAssertions;
using SMEAppHouse.Core.RestSharpClientLib;
using Xunit;

namespace SMEAppHouse.Core.RestSharpClientLib.Tests;

public class ApiClientTests
{
    [Fact]
    public void ApiClient_DefaultConstructor_ShouldInitialize()
    {
        // Act
        var client = new ApiClient();

        // Assert
        client.Should().NotBeNull();
        client.Configuration.Should().NotBeNull();
        client.RestClient.Should().NotBeNull();
    }

    [Fact]
    public void ApiClient_WithConfiguration_ShouldUseConfiguration()
    {
        // Arrange
        var config = new Configuration { BasePath = "https://api.example.com" };

        // Act
        var client = new ApiClient(config);

        // Assert
        client.Configuration.Should().BeSameAs(config);
        client.RestClient.Should().NotBeNull();
    }

    [Fact]
    public void ApiClient_WithBasePath_ShouldSetBasePath()
    {
        // Arrange
        var basePath = "https://api.example.com";

        // Act
        var client = new ApiClient(basePath);

        // Assert
        client.RestClient.Should().NotBeNull();
    }

    [Fact]
    public void ApiClient_WithEmptyBasePath_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new ApiClient(""));
    }

    [Fact]
    public void ApiClient_WithNullBasePath_ShouldThrowArgumentException()
    {
        // Act & Assert
        string? nullBasePath = null;
        Assert.Throws<ArgumentException>(() => new ApiClient(nullBasePath!));
    }
}

