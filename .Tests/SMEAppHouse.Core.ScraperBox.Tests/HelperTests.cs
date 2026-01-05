using FluentAssertions;
using SMEAppHouse.Core.CodeKits;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;
using Xunit;

namespace SMEAppHouse.Core.ScraperBox.Tests;

public class HelperTests
{
    [Theory]
    [InlineData("//example.com", "http://example.com")]
    [InlineData("http://example.com", "http://example.com")]
    [InlineData("https://example.com", "https://example.com")]
    [InlineData("//www.example.com/path", "http://www.example.com/path")]
    public void ResolveHttpUrl_ShouldResolveProtocolRelativeUrls(string input, string expected)
    {
        // Act
        var result = Helper.ResolveHttpUrl(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("https://www.example.com/path/page", false, "www.example.com")]
    [InlineData("https://www.example.com/path/page", true, "https://www.example.com")]
    [InlineData("http://subdomain.example.com", false, "subdomain.example.com")]
    [InlineData("http://example.com", false, "example.com")]
    public void ExtractDomainNameFromUrl_ShouldExtractDomain(string url, bool retainHttPrefix, string expected)
    {
        // Act
        var result = Helper.ExtractDomainNameFromUrl(url, retainHttPrefix);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("https://www.example.com", true)]
    [InlineData("http://example.com/path", true)]
    [InlineData("ftp://example.com", true)]
    [InlineData("invalid-url", false)]
    [InlineData("", false)]
    [InlineData("not a url", false)]
    [InlineData("//example.com", false)] // Protocol-relative URLs are not absolute
    public void IsURLValid_WithBruteFalse_ShouldValidateUrlFormat(string url, bool expected)
    {
        // Act
        var result = Helper.IsURLValid(url, brute: false);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("&amp;", "&")]
    [InlineData("test%3A", "test:")]
    [InlineData("test%2F", "test/")]
    [InlineData("&#034;", "\"")]
    [InlineData("&#039;", "'")]
    [InlineData("<br />", "\n")]
    [InlineData("<br/>", "\n")]
    [InlineData("&nbsp;", " ")]
    [InlineData("test,", "test")]
    public void Resolve_ShouldDecodeHtmlEntities(string input, string expected)
    {
        // Act
        var result = Helper.Resolve(input);

        // Assert
        result.Should().Contain(expected);
    }

    [Fact]
    public void Resolve_WithAllTrim_ShouldTrimWhitespace()
    {
        // Arrange
        var input = "  test  ";

        // Act
        var result = Helper.Resolve(input, allTrim: true);

        // Assert
        result.Should().Be("test");
    }

    [Fact]
    public void Resolve_WithOtherElementsToClear_ShouldRemoveElements()
    {
        // Arrange
        var input = "test<script>alert('xss')</script>";

        // Act
        var result = Helper.Resolve(input, otherElementsToClear: new[] { "<script>", "</script>" });

        // Assert
        result.Should().NotContain("<script>");
        result.Should().NotContain("</script>");
    }

    [Theory]
    [InlineData("&amp;", "")]
    [InlineData("test%3A", "test")]
    [InlineData("test%2F", "test")]
    [InlineData("&#034;", "")]
    [InlineData("&#039;", "")]
    [InlineData("<br />", "")]
    [InlineData("<br/>", "")]
    [InlineData("&nbsp;", "")]
    [InlineData("test,", "test")]
    [InlineData("amp;", "")]
    [InlineData("#shId", "")]
    public void CleanupHtmlStrains_ShouldRemoveHtmlStrains(string input, string expected)
    {
        // Act
        var result = Helper.CleanupHtmlStrains(input);

        // Assert
        result.Should().NotContainAny(expected.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
    }

    [Fact]
    public void CleanupHtmlStrains_WithAllTrim_ShouldTrimWhitespace()
    {
        // Arrange
        var input = "  test  ";

        // Act
        var result = Helper.CleanupHtmlStrains(input, allTrim: true);

        // Assert
        result.Should().Be("test");
    }

    [Theory]
    [InlineData("test query", "test%20query")]
    [InlineData("test & query", "test%20%26%20query")]
    [InlineData("  test  ", "test")]
    public void EncodeQueryStringSegment_ShouldEncodeQueryString(string input, string expected)
    {
        // Act
        var result = Helper.EncodeQueryStringSegment(input);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("United States", Rules.WorldCountriesEnum.UNITED_STATES)]
    [InlineData("united", Rules.WorldCountriesEnum.UNITED_STATES)]
    [InlineData("UNITED", Rules.WorldCountriesEnum.UNITED_STATES)]
    [InlineData("Canada", Rules.WorldCountriesEnum.CANADA)]
    [InlineData("can", Rules.WorldCountriesEnum.CANADA)]
    [InlineData("InvalidCountry", Rules.WorldCountriesEnum.UNKNOWN)]
    [InlineData("", Rules.WorldCountriesEnum.UNKNOWN)]
    public void FindProxyCountryFromPartial_ShouldFindCountry(string partial, Rules.WorldCountriesEnum expected)
    {
        // Act
        var result = Helper.FindProxyCountryFromPartial(partial);

        // Assert
        result.Should().Be(expected);
    }
}

