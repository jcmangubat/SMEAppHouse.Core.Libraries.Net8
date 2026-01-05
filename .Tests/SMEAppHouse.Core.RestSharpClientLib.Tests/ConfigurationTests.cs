using FluentAssertions;
using SMEAppHouse.Core.RestSharpClientLib;
using Xunit;

namespace SMEAppHouse.Core.RestSharpClientLib.Tests;

public class ConfigurationTests
{
    [Fact]
    public void Configuration_Default_ShouldBeAccessible()
    {
        // Act
        var config = Configuration.Default;

        // Assert
        config.Should().NotBeNull();
    }

    [Fact]
    public void Configuration_Default_ShouldBeSettable()
    {
        // Arrange
        var original = Configuration.Default;
        var newConfig = new Configuration();

        // Act
        Configuration.Default = newConfig;

        // Assert
        Configuration.Default.Should().BeSameAs(newConfig);

        // Cleanup
        Configuration.Default = original;
    }

    [Fact]
    public void Configuration_Constructor_ShouldInitialize()
    {
        // Act
        var config = new Configuration();

        // Assert
        config.Should().NotBeNull();
        config.BasePath.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Configuration_BasePath_ShouldBeSettable()
    {
        // Arrange
        var config = new Configuration();
        var basePath = "https://api.example.com";

        // Act
        config.BasePath = basePath;

        // Assert
        config.BasePath.Should().Be(basePath);
    }

    [Fact]
    public void Configuration_Timeout_ShouldBeSettable()
    {
        // Arrange
        var config = new Configuration();
        var timeout = 5000;

        // Act
        config.Timeout = timeout;

        // Assert
        config.Timeout.Should().Be(timeout);
    }

    [Fact]
    public void Configuration_UserAgent_ShouldBeSettable()
    {
        // Arrange
        var config = new Configuration();
        var userAgent = "MyApp/1.0";

        // Act
        config.UserAgent = userAgent;

        // Assert
        config.UserAgent.Should().Be(userAgent);
    }

    [Fact]
    public void Configuration_DefaultExceptionFactory_ShouldNotBeNull()
    {
        // Assert
        Configuration.DefaultExceptionFactory.Should().NotBeNull();
    }

    [Fact]
    public void Configuration_DefaultExceptionFactory_WithStatusCode400_ShouldReturnException()
    {
        // Arrange
        var response = new RestSharp.RestResponse
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest,
            Content = "Error message"
        };

        // Act
        var exception = Configuration.DefaultExceptionFactory("TestMethod", response);

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ApiException>();
    }

    [Fact]
    public void Configuration_DefaultExceptionFactory_WithStatusCode200_ShouldReturnNull()
    {
        // Arrange
        var response = new RestSharp.RestResponse
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = "Success"
        };

        // Act
        var exception = Configuration.DefaultExceptionFactory("TestMethod", response);

        // Assert
        exception.Should().BeNull();
    }
}

