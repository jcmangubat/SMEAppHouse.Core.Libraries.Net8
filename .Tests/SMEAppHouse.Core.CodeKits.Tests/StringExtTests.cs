using FluentAssertions;
using SMEAppHouse.Core.CodeKits.Extensions;
using Xunit;

namespace SMEAppHouse.Core.CodeKits.Tests;

public class StringExtTests
{
    [Theory]
    [InlineData("Hello", 3, "Hel")]
    [InlineData("Hello", 5, "Hello")]
    [InlineData("Hello", 10, "Hello")]
    [InlineData("", 5, "")]
    public void Left_ShouldReturnLeftSubstring(string input, int length, string expected)
    {
        // Act
        var result = input.Left(length);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Hello", 3, "llo")]
    [InlineData("Hello", 5, "Hello")]
    [InlineData("Hello", 10, "Hello")]
    [InlineData("", 5, "")]
    public void Right_ShouldReturnRightSubstring(string input, int length, string expected)
    {
        // Act
        var result = input.Right(length);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Hello", 1, 3, "ell")]
    [InlineData("Hello", 0, 5, "Hello")]
    public void Mid_WithLength_ShouldReturnSubstring(string input, int startIndex, int length, string expected)
    {
        // Act
        var result = input.Mid(startIndex, length);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("Hello", 2, "llo")]
    [InlineData("Hello", 0, "Hello")]
    [InlineData("Hello", 10, "")]
    public void Mid_WithoutLength_ShouldReturnSubstringFromIndex(string input, int startIndex, string expected)
    {
        // Act
        var result = StringExt.Mid(input, startIndex);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("HelloWorld", "Hello World")]
    [InlineData("XMLParser", "XML Parser")]
    [InlineData("GetHTTPResponse", "Get HTTP Response")]
    [InlineData("ID", "ID")]
    public void FromCamelCase_ShouldAddSpaces(string input, string expected)
    {
        // Act
        var result = StringExt.FromCamelCase(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void FromCamelCase_WithNull_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => StringExt.FromCamelCase(null!));
    }

    [Theory]
    [InlineData("hello world", "HelloWorld")]
    [InlineData("hello-world", "HelloWorld")]
    [InlineData("hello_world", "HelloWorld")]
    [InlineData("hello.world", "HelloWorld")]
    [InlineData("  hello   world  ", "HelloWorld")]
    public void ToCamelCase_ShouldConvertToCamelCase(string input, string expected)
    {
        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToCamelCase_WithEmptyString_ShouldReturnEmpty()
    {
        // Arrange
        var input = "";

        // Act
        var result = input.ToCamelCase();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("hello", "Hello")]
    [InlineData("HELLO", "Hello")]
    [InlineData("", "")]
    public void ToLowerFirstChar_ShouldConvertFirstCharToLower(string input, string expected)
    {
        // Act
        var result = input.ToLowerFirstChar();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("hello", "Hello")]
    [InlineData("HELLO", "HELLO")]
    [InlineData("", "")]
    public void ToUpperFirstChar_ShouldConvertFirstCharToUpper(string input, string expected)
    {
        // Act
        var result = input.ToUpperFirstChar();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("hello  world", 1, "hello world")]
    [InlineData("hello    world", 1, "hello world")]
    [InlineData("  hello  world  ", 1, "hello world")]
    public void TrimOrReplaceSpaces_ShouldTrimAndReplaceMultipleSpaces(string input, int numSpace, string expected)
    {
        // Act
        var result = input.TrimOrReplaceSpaces(numSpace);

        // Assert
        result.Should().Be(expected);
    }
}

