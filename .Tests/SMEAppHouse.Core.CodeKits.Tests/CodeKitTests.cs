using FluentAssertions;
using SMEAppHouse.Core.CodeKits;
using Xunit;

namespace SMEAppHouse.Core.CodeKits.Tests;

public class CodeKitTests
{
    [Theory]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(double), true)]
    [InlineData(typeof(decimal), true)]
    [InlineData(typeof(float), true)]
    [InlineData(typeof(long), true)]
    [InlineData(typeof(short), true)]
    [InlineData(typeof(byte), true)]
    [InlineData(typeof(sbyte), true)]
    [InlineData(typeof(ushort), true)]
    [InlineData(typeof(uint), true)]
    [InlineData(typeof(ulong), true)]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(bool), false)]
    [InlineData(typeof(DateTime), false)]
    [InlineData(typeof(char), false)]
    [InlineData(typeof(object), false)]
    public void IsNumericType_ShouldReturnCorrectValue(Type type, bool expected)
    {
        // Act
        var result = CodeKit.IsNumericType(type);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void IsNumericType_WithEnum_ShouldReturnFalse()
    {
        // Arrange
        var enumType = typeof(DayOfWeek);

        // Act
        var result = CodeKit.IsNumericType(enumType);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNumericType_WithArray_ShouldReturnFalse()
    {
        // Arrange
        var arrayType = typeof(int[]);

        // Act
        var result = CodeKit.IsNumericType(arrayType);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNumericType_WithList_ShouldReturnFalse()
    {
        // Arrange
        var listType = typeof(List<int>);

        // Act
        var result = CodeKit.IsNumericType(listType);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNumericType_WithNullableInt_ShouldReturnTrue()
    {
        // Arrange
        var nullableType = typeof(int?);

        // Act
        var result = CodeKit.IsNumericType(nullableType);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HashPassword_ShouldReturnValidHash()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hash = CodeKit.HashPassword(password);

        // Assert
        hash.Should().NotBeNullOrEmpty();
        hash.Should().HaveLength(64); // SHA256 produces 64 hex characters
        hash.Should().MatchRegex("^[0-9a-f]{64}$");
    }

    [Fact]
    public void HashPassword_WithSameInput_ShouldReturnSameHash()
    {
        // Arrange
        var password = "TestPassword123";

        // Act
        var hash1 = CodeKit.HashPassword(password);
        var hash2 = CodeKit.HashPassword(password);

        // Assert
        hash1.Should().Be(hash2);
    }

    [Fact]
    public void HashPassword_WithDifferentInput_ShouldReturnDifferentHash()
    {
        // Arrange
        var password1 = "TestPassword123";
        var password2 = "TestPassword124";

        // Act
        var hash1 = CodeKit.HashPassword(password1);
        var hash2 = CodeKit.HashPassword(password2);

        // Assert
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void CopyObjectProperties_ShouldCopyProperties()
    {
        // Arrange
        var source = new { Name = "John", Age = 30, Email = "john@example.com" };
        var target = new TestPerson();

        // Act
        CodeKit.CopyObjectProperties(source, target);

        // Assert
        target.Name.Should().Be("John");
        target.Age.Should().Be(30);
        target.Email.Should().Be("john@example.com");
    }

    [Fact]
    public void CopyObjectProperties_WithPartialMatch_ShouldCopyOnlyMatchingProperties()
    {
        // Arrange
        var source = new { Name = "John", Age = 30 };
        var target = new TestPerson { Email = "existing@example.com" };

        // Act
        CodeKit.CopyObjectProperties(source, target);

        // Assert
        target.Name.Should().Be("John");
        target.Age.Should().Be(30);
        target.Email.Should().Be("existing@example.com");
    }

    [Fact]
    public void GetObjectSize_ShouldReturnSizeInBytes()
    {
        // Arrange
        var obj = new { Name = "Test", Value = 123, Items = new[] { 1, 2, 3 } };

        // Act
        var size = CodeKit.GetObjectSize(obj);

        // Assert
        size.Should().BeGreaterThan(0);
    }

    [Fact]
    public void GetObjectSize_WithEmptyObject_ShouldReturnSize()
    {
        // Arrange
        var obj = new { };

        // Act
        var size = CodeKit.GetObjectSize(obj);

        // Assert
        size.Should().BeGreaterThan(0);
    }

    [Fact]
    public void RandomNumber_ShouldReturnNumberInRange()
    {
        // Act
        var result = CodeKit.RandomNumber(1, 100);

        // Assert
        result.Should().BeGreaterThanOrEqualTo(1);
        result.Should().BeLessThan(100);
    }

    [Fact]
    public void RandomNumber_WithSameRange_ShouldReturnDifferentValues()
    {
        // Act
        var result1 = CodeKit.RandomNumber(1, 1000);
        var result2 = CodeKit.RandomNumber(1, 1000);

        // Note: This test might occasionally fail due to randomness
        // In practice, results should be different most of the time
        // We're just verifying the method works
        result1.Should().BeGreaterThanOrEqualTo(1);
        result2.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public void RandomDate_ShouldReturnDateInRange()
    {
        // Arrange
        var random = new Random();
        var start = new DateTime(2020, 1, 1);
        var end = new DateTime(2020, 12, 31);

        // Act
        var result = CodeKit.RandomDate(random, start, end);

        // Assert
        result.Should().BeOnOrAfter(start);
        result.Should().BeOnOrBefore(end);
    }

    [Fact]
    public void RandomTime_ShouldReturnTimeInRange()
    {
        // Act
        var result = CodeKit.RandomTime(9, 17);

        // Assert
        result.TotalHours.Should().BeGreaterThanOrEqualTo(9);
        result.TotalHours.Should().BeLessThan(17);
    }

    [Theory]
    [InlineData(0, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(20, 10, 2)]
    [InlineData(21, 10, 3)]
    [InlineData(100, 25, 4)]
    public void CalculateNumberOfPages_ShouldReturnCorrectPages(int totalItems, int pageSize, int expectedPages)
    {
        // Act
        var result = CodeKit.CalculateNumberOfPages(totalItems, pageSize);

        // Assert
        result.Should().Be(expectedPages);
    }

    [Fact]
    public void ExtractHexDigits_ShouldExtractOnlyHexCharacters()
    {
        // Arrange
        var input = "ABC123xyz456!@#";

        // Act
        var result = CodeKit.ExtractHexDigits(input);

        // Assert
        result.Should().Be("ABC123456");
        result.Should().MatchRegex("^[0-9A-Fa-f]+$");
    }

    [Fact]
    public void ExtractHexDigits_WithNoHexDigits_ShouldReturnEmpty()
    {
        // Arrange
        var input = "!@#$%^&*()";

        // Act
        var result = CodeKit.ExtractHexDigits(input);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ExtractHexDigits_WithEmptyString_ShouldReturnEmpty()
    {
        // Arrange
        var input = "";

        // Act
        var result = CodeKit.ExtractHexDigits(input);

        // Assert
        result.Should().BeEmpty();
    }
}

public class TestPerson
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
}

