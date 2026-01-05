using FluentAssertions;
using SMEAppHouse.Core.Patterns.EF.Exceptions;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.Exceptions;

public class EntityNotFoundExceptionTests
{
    [Fact]
    public void Constructor_Default_ShouldCreateException()
    {
        // Act
        var exception = new EntityNotFoundException<TestEntity>();

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        // Arrange
        var message = "Custom error message";

        // Act
        var exception = new EntityNotFoundException<TestEntity>(message);

        // Assert
        exception.Message.Should().Be(message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
    {
        // Arrange
        var message = "Custom error message";
        var innerException = new InvalidOperationException("Inner exception");

        // Act
        var exception = new EntityNotFoundException<TestEntity>(message, innerException);

        // Assert
        exception.Message.Should().Be(message);
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void Constructor_WithId_ShouldCreateExceptionWithId()
    {
        // Arrange
        var id = 42;

        // Act
        var exception = new EntityNotFoundException<TestEntity>(id);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Contain("42");
        exception.Message.Should().Contain(typeof(TestEntity).Name);
    }

    [Fact]
    public void Constructor_WithGuidId_ShouldCreateExceptionWithId()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var exception = new EntityNotFoundException<TestEntity>(id);

        // Assert
        exception.Should().NotBeNull();
        exception.Message.Should().Contain(id.ToString());
    }
}

/// <summary>
/// Test entity for exception tests
/// </summary>
public class TestEntity
{
    public int Id { get; set; }
}

