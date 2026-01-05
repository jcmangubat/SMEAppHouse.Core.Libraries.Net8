using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;
using SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.DbContextAbstractions;

public class AppDbContextExtendedTests
{
    [Fact]
    public void Constructor_WithConnectionString_ShouldCreateInstance()
    {
        // Arrange
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;";

        // Act
        var context = new AppDbContextExtended<TestDbContext>(connectionString);

        // Assert
        context.Should().NotBeNull();
        context.DbContextOptions.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullConnectionString_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new AppDbContextExtended<TestDbContext>((string)null!));
    }

    [Fact]
    public void Constructor_WithEmptyConnectionString_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new AppDbContextExtended<TestDbContext>(string.Empty));
    }

    [Fact]
    public void Constructor_WithWhitespaceConnectionString_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new AppDbContextExtended<TestDbContext>("   "));
    }

    [Fact]
    public void Constructor_WithOptions_ShouldCreateInstance()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Act
        var context = new AppDbContextExtended<TestDbContext>(options);

        // Assert
        context.Should().NotBeNull();
        context.DbContextOptions.Should().Be(options);
    }

    [Fact]
    public void Constructor_WithNullOptions_ShouldThrowArgumentNullException()
    {
        // Arrange
        DbContextOptions<TestDbContext>? nullOptions = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new AppDbContextExtended<TestDbContext>(nullOptions!));
    }

    [Fact]
    public void CreateDbContext_WithConnectionString_ShouldReturnDbContext()
    {
        // Arrange
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;";

        // Act
        var context = AppDbContextExtended<TestDbContext>.CreateDbContext(connectionString);

        // Assert
        context.Should().NotBeNull();
        context.Should().BeOfType<TestDbContext>();
    }

    [Fact]
    public void CreateDbContext_WithConnectionStringAndMigrationInfo_ShouldReturnDbContext()
    {
        // Arrange
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;";
        var migrationTable = "TestMigrations";
        var schema = "dbo";

        // Act
        var context = AppDbContextExtended<TestDbContext>.CreateDbContext(
            connectionString, migrationTable, schema);

        // Assert
        context.Should().NotBeNull();
        context.Should().BeOfType<TestDbContext>();
    }

    [Fact]
    public void GetOptions_WithConnectionString_ShouldReturnOptions()
    {
        // Arrange
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;";

        // Act
        var options = AppDbContextExtended<TestDbContext>.GetOptions(connectionString);

        // Assert
        options.Should().NotBeNull();
    }

    [Fact]
    public void GetOptions_WithConnectionStringAndMigrationInfo_ShouldReturnOptions()
    {
        // Arrange
        var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;";
        var migrationTable = "TestMigrations";
        var schema = "dbo";

        // Act
        var options = AppDbContextExtended<TestDbContext>.GetOptions(
            connectionString, migrationTable, schema);

        // Assert
        options.Should().NotBeNull();
    }

    [Fact]
    public void SaveChanges_WithValidEntity_ShouldSaveSuccessfully()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
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
        using var context = DbContextTestHelper.CreateInMemoryContext();
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
        using var context = DbContextTestHelper.CreateInMemoryContext();
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
        using var context = DbContextTestHelper.CreateInMemoryContext();
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

    [Fact]
    public void SaveChanges_WithMultipleEntities_ShouldValidateEachEntitySeparately()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        var validEntity = new TestEntity { Name = "Valid", Email = "valid@example.com" };
        var invalidEntity = new TestEntityWithValidation { Name = "", Email = "invalid" };
        
        context.TestEntities.Add(validEntity);
        context.Set<TestEntityWithValidation>().Add(invalidEntity);

        // Act & Assert
        Assert.Throws<ValidationException>(() => context.SaveChanges());
        
        // The valid entity should not be saved
        context.TestEntities.Count().Should().Be(0);
    }
}


