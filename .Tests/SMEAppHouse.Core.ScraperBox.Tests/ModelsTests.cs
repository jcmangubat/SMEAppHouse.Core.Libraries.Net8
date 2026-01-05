using FluentAssertions;
using SMEAppHouse.Core.ScraperBox.Models;
using System;
using System.Net;
using Xunit;

namespace SMEAppHouse.Core.ScraperBox.Tests;

public class IPProxyTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithNewGuid()
    {
        // Act
        var proxy = new IPProxy();

        // Assert
        proxy.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void ToWebProxy_ShouldCreateWebProxy()
    {
        // Arrange
        var proxy = new IPProxy
        {
            IPAddress = "192.168.1.1",
            PortNo = 8080
        };

        // Act
        var webProxy = proxy.ToWebProxy();

        // Assert
        webProxy.Should().NotBeNull();
        var webProxyCast = webProxy as WebProxy;
        webProxyCast.Should().NotBeNull();
        webProxyCast!.Address.Should().Be(new Uri("http://192.168.1.1:8080"));
    }

    [Fact]
    public void ToNetworkCredential_WithCredential_ShouldReturnNetworkCredential()
    {
        // Arrange
        var proxy = new IPProxy
        {
            Credential = new Tuple<string, string>("username", "password")
        };

        // Act
        var credential = proxy.ToNetworkCredential();

        // Assert
        credential.Should().NotBeNull();
        credential!.UserName.Should().Be("username");
        credential.Password.Should().Be("password");
    }

    [Fact]
    public void ToNetworkCredential_WithoutCredential_ShouldReturnNull()
    {
        // Arrange
        var proxy = new IPProxy();

        // Act
        var credential = proxy.ToNetworkCredential();

        // Assert
        credential.Should().BeNull();
    }

    [Fact]
    public void AsTuple_ShouldReturnIPAndPortAsTuple()
    {
        // Arrange
        var proxy = new IPProxy
        {
            IPAddress = "192.168.1.1",
            PortNo = 8080
        };

        // Act
        var tuple = proxy.AsTuple();

        // Assert
        tuple.Should().NotBeNull();
        tuple.Item1.Should().Be("192.168.1.1");
        tuple.Item2.Should().Be("8080");
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var proxy = new IPProxy
        {
            ProviderId = "TestProvider",
            IPAddress = "192.168.1.1",
            PortNo = 8080
        };

        // Act
        var result = proxy.ToString();

        // Assert
        result.Should().Be("TestProvider -> 192.168.1.1:8080");
    }

    [Fact]
    public void GetLastValidationElapsedTime_ShouldReturnTimeSpan()
    {
        // Arrange
        var proxy = new IPProxy
        {
            LastChecked = DateTime.Now.AddMinutes(-5)
        };

        // Act
        var elapsed = proxy.GetLastValidationElapsedTime();

        // Assert
        elapsed.Should().BeCloseTo(TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void SpeedTimeSpan_ShouldConvertSpeedRateToTimeSpan()
    {
        // Arrange
        var proxy = new IPProxy
        {
            SpeedRate = 1500 // milliseconds
        };

        // Act & Assert
        proxy.SpeedTimeSpan.Should().Be(TimeSpan.FromMilliseconds(1500));
    }
}

public class CrawlerOptionsTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var options = new CrawlerOptions();

        // Assert
        options.InNewPage.Should().BeFalse();
        options.NoImage.Should().BeFalse();
        options.UseProxy.Should().BeFalse();
        options.IPProxy.Should().BeNull();
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var proxy = new IPProxy();
        var options = new CrawlerOptions();

        // Act
        options.InNewPage = true;
        options.NoImage = true;
        options.UseProxy = true;
        options.IPProxy = proxy;

        // Assert
        options.InNewPage.Should().BeTrue();
        options.NoImage.Should().BeTrue();
        options.UseProxy.Should().BeTrue();
        options.IPProxy.Should().Be(proxy);
    }
}

public class CrawlerResultTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var result = new CrawlerResult();

        // Assert
        result.HasFailed.Should().BeFalse();
        result.CrawlerException.Should().BeNull();
        result.PageContent.Should().BeNull();
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var exception = new Exception("Test exception");
        var result = new CrawlerResult();

        // Act
        result.HasFailed = true;
        result.CrawlerException = exception;
        result.PageContent = "<html>content</html>";

        // Assert
        result.HasFailed.Should().BeTrue();
        result.CrawlerException.Should().Be(exception);
        result.PageContent.Should().Be("<html>content</html>");
    }
}

public class PageInstructionTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var instruction = new PageInstruction();

        // Assert
        instruction.PadCharacter.Should().Be('\0');
        instruction.PadLength.Should().Be(0);
        instruction.PaddingDirection.Should().Be(PageInstruction.PaddingDirectionsEnum.ToLeft);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var instruction = new PageInstruction();

        // Act
        instruction.PadCharacter = '0';
        instruction.PadLength = 3;
        instruction.PaddingDirection = PageInstruction.PaddingDirectionsEnum.ToRight;

        // Assert
        instruction.PadCharacter.Should().Be('0');
        instruction.PadLength.Should().Be(3);
        instruction.PaddingDirection.Should().Be(PageInstruction.PaddingDirectionsEnum.ToRight);
    }
}

