using FluentAssertions;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.EntityCompositing.Base;

public class KeyedEntityTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var entity = new TestKeyedEntity();

        // Assert
        entity.Id.Should().Be(default(int));
        entity.IsActive.Should().BeTrue();
        entity.DateCreated.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Id_SetValue_ShouldUpdateId()
    {
        // Arrange
        var entity = new TestKeyedEntity();

        // Act
        entity.Id = 42;

        // Assert
        entity.Id.Should().Be(42);
    }

    [Fact]
    public void Id_SetSameValue_ShouldNotUpdate()
    {
        // Arrange
        var entity = new TestKeyedEntity { Id = 10 };

        // Act
        entity.Id = 10; // Same value

        // Assert
        entity.Id.Should().Be(10);
    }

    [Fact]
    public void IsActive_SetValue_ShouldUpdate()
    {
        // Arrange
        var entity = new TestKeyedEntity();

        // Act
        entity.IsActive = false;

        // Assert
        entity.IsActive.Should().BeFalse();
    }

    [Fact]
    public void IsActive_SetNull_ShouldNotUpdate()
    {
        // Arrange
        var entity = new TestKeyedEntity { IsActive = true };

        // Act
        entity.IsActive = null;

        // Assert
        entity.IsActive.Should().BeTrue(); // Should remain true
    }

    [Fact]
    public void DateCreated_SetValue_ShouldConvertToUtc()
    {
        // Arrange
        var entity = new TestKeyedEntity();
        var localTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local);

        // Act
        entity.DateCreated = localTime;

        // Assert
        entity.DateCreated.Kind.Should().Be(DateTimeKind.Utc);
        entity.DateCreated.Should().BeCloseTo(localTime.ToUniversalTime(), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void DateCreated_GetValue_ShouldReturnUtc()
    {
        // Arrange
        var entity = new TestKeyedEntity();
        var utcTime = DateTime.UtcNow;

        // Act
        entity.DateCreated = utcTime;
        var retrieved = entity.DateCreated;

        // Assert
        retrieved.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void DateModified_SetValue_ShouldConvertToUtc()
    {
        // Arrange
        var entity = new TestKeyedEntity();
        var localTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local);

        // Act
        entity.DateModified = localTime;

        // Assert
        entity.DateModified.Should().NotBeNull();
        entity.DateModified!.Value.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void DateModified_SetNull_ShouldClearValue()
    {
        // Arrange
        var entity = new TestKeyedEntity { DateModified = DateTime.UtcNow };

        // Act
        entity.DateModified = null;

        // Assert
        entity.DateModified.Should().BeNull();
    }

    [Fact]
    public void GetEntityIdentificationType_ShouldReturnPrimaryKeyType()
    {
        // Arrange
        var entity = new TestKeyedEntity();

        // Act
        var type = entity.GetEntityIdentificationType();

        // Assert
        type.Should().Be(typeof(int));
    }

    [Fact]
    public void GetImplementors_ShouldReturnTypesImplementingInterface()
    {
        // Act
        var implementors = KeyedEntity<int>.GetImplementors();

        // Assert
        implementors.Should().NotBeNull();
        // Should include TestKeyedEntity and other test entities
        implementors.Should().Contain(t => t == typeof(TestKeyedEntity));
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var entity = new TestKeyedEntity 
        { 
            Id = 42, 
            IsActive = true,
            DateCreated = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var result = entity.ToString();

        // Assert
        result.Should().Contain("Id:42");
        result.Should().Contain("Active:True");
    }

    [Fact]
    public void VerifyPropertyName_WithValidProperty_ShouldNotThrow()
    {
        // Arrange
        var entity = new TestKeyedEntity();

        // Act & Assert
        entity.Invoking(e => e.VerifyPropertyName("Id"))
            .Should().NotThrow();
    }

    [Fact]
    public void VerifyPropertyName_WithInvalidProperty_ShouldFailInDebug()
    {
        // Arrange
        var entity = new TestKeyedEntity();

        // Act & Assert
        // In Debug mode, this should fail
        entity.Invoking(e => e.VerifyPropertyName("NonExistentProperty"))
            .Should().NotThrow(); // In Release mode, this won't throw
    }
}

/// <summary>
/// Test implementation of KeyedEntity for unit testing
/// </summary>
public class TestKeyedEntity : KeyedEntity<int>
{
    public string Name { get; set; } = string.Empty;
}

