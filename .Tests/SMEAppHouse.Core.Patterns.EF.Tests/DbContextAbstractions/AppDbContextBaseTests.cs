using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;
using SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.DbContextAbstractions;

public class AppDbContextBaseTests
{
    [Fact]
    public void Constructor_WithOptions_ShouldCreateInstance()
    {
        // Arrange
        var options = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Act
        var context = new AppDbContextExt(options);

        // Assert
        context.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new AppDbContextExt(null!));
    }

    [Fact]
    public void SaveChanges_WithValidEntity_ShouldSaveSuccessfully()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        using var context = new TestDbContext(options);
        var entity = new TestEntity { Name = "Test", Email = "test@example.com" };
        context.TestEntities.Add(entity);

        // Act
        var result = context.SaveChanges();

        // Assert
        result.Should().Be(1);
        context.TestEntities.Count().Should().Be(1);
    }

    [Fact]
    public void SaveChanges_WithInvalidEntity_ShouldThrowValidationException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        using var context = new TestDbContext(options);
        context.Database.EnsureCreated();
        var entity = new TestEntityWithValidation 
        { 
            Name = "", // Invalid: Required
            Email = "invalid-email" // Invalid: Email format
        };
        context.Set<TestEntityWithValidation>().Add(entity);

        // Act & Assert
        Assert.Throws<ValidationException>(() => context.SaveChanges());
    }

    [Fact]
    public async Task SaveChangesAsync_WithValidEntity_ShouldSaveSuccessfully()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        using var context = new TestDbContext(options);
        var entity = new TestEntity { Name = "Test", Email = "test@example.com" };
        context.TestEntities.Add(entity);

        // Act
        var result = await context.SaveChangesAsync();

        // Assert
        result.Should().Be(1);
        context.TestEntities.Count().Should().Be(1);
    }

    [Fact]
    public async Task SaveChangesAsync_WithInvalidEntity_ShouldThrowValidationException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        using var context = new TestDbContext(options);
        context.Database.EnsureCreated();
        var entity = new TestEntityWithValidation 
        { 
            Name = "", // Invalid: Required
            Email = "invalid-email" // Invalid: Email format
        };
        context.Set<TestEntityWithValidation>().Add(entity);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => context.SaveChangesAsync());
    }
}

