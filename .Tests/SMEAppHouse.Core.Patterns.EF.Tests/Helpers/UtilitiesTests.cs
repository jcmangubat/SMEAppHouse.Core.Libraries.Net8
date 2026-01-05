using FluentAssertions;
using SMEAppHouse.Core.Patterns.EF.Helpers;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.Helpers;

public class UtilitiesTests
{
    [Fact]
    public void UpdateValuesFrom_WithMatchingProperties_ShouldCopyValues()
    {
        // Arrange
        var source = new SourceEntity 
        { 
            Id = 1, 
            Name = "Source Name", 
            Email = "source@test.com",
            Age = 30
        };
        var destination = new DestinationEntity();

        // Act
        destination.UpdateValuesFrom(source);

        // Assert
        destination.Id.Should().Be(1);
        destination.Name.Should().Be("Source Name");
        destination.Email.Should().Be("source@test.com");
        destination.Age.Should().Be(30);
    }

    [Fact]
    public void UpdateValuesFrom_WithNonMatchingProperties_ShouldSkip()
    {
        // Arrange
        var source = new SourceEntity 
        { 
            Id = 1, 
            Name = "Source Name",
            Email = "source@test.com"
        };
        var destination = new DestinationEntity { ExtraProperty = "Original" };

        // Act
        destination.UpdateValuesFrom(source);

        // Assert
        destination.ExtraProperty.Should().Be("Original"); // Should not be overwritten
    }

    [Fact]
    public void UpdateValuesFrom_WithNullSource_ShouldThrowArgumentNullException()
    {
        // Arrange
        var destination = new DestinationEntity();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            destination.UpdateValuesFrom((SourceEntity)null!));
    }

    [Fact]
    public void UpdateValuesFrom_WithNullDestination_ShouldThrowArgumentNullException()
    {
        // Arrange
        var source = new SourceEntity();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ((DestinationEntity)null!).UpdateValuesFrom(source));
    }

    [Fact]
    public void UpdateValuesFrom_WithNullableProperties_ShouldHandleNulls()
    {
        // Arrange
        var source = new SourceEntityWithNullable 
        { 
            Id = 1, 
            Name = "Test",
            OptionalValue = null
        };
        var destination = new DestinationEntityWithNullable { OptionalValue = 100 };

        // Act
        destination.UpdateValuesFrom(source);

        // Assert
        destination.OptionalValue.Should().Be(100); // Should not be overwritten with null
    }

    [Fact]
    public void UpdateValuesFrom_WithTypeMismatch_ShouldSkip()
    {
        // Arrange
        var source = new SourceEntity { Id = 1, Name = "Test" };
        var destination = new DestinationEntityWithDifferentType { Id = 2, Name = 42 }; // Name is int

        // Act
        destination.UpdateValuesFrom(source);

        // Assert
        destination.Name.Should().Be(42); // Should not be overwritten due to type mismatch
    }

    [Fact]
    public void UpdateValuesFrom_WithCaseInsensitiveMatching_ShouldMatch()
    {
        // Arrange
        var source = new SourceEntity { Id = 1, Name = "Test" };
        var destination = new DestinationEntityCaseInsensitive { id = 0, name = "" };

        // Act
        destination.UpdateValuesFrom(source);

        // Assert
        destination.id.Should().Be(1);
        destination.name.Should().Be("Test");
    }
}

/// <summary>
/// Source entity for testing
/// </summary>
public class SourceEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
}

/// <summary>
/// Destination entity for testing
/// </summary>
public class DestinationEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public string ExtraProperty { get; set; } = string.Empty;
}

/// <summary>
/// Source entity with nullable properties
/// </summary>
public class SourceEntityWithNullable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OptionalValue { get; set; }
}

/// <summary>
/// Destination entity with nullable properties
/// </summary>
public class DestinationEntityWithNullable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OptionalValue { get; set; }
}

/// <summary>
/// Destination entity with different property types
/// </summary>
public class DestinationEntityWithDifferentType
{
    public int Id { get; set; }
    public int Name { get; set; } // Different type
}

/// <summary>
/// Destination entity with different casing
/// </summary>
public class DestinationEntityCaseInsensitive
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
}

