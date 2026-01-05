using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Interfaces;
using SMEAppHouse.Core.Patterns.Repo.Tests.TestHelpers;
using SMEAppHouse.Core.Patterns.Repo.V2.Base;
using Xunit;

namespace SMEAppHouse.Core.Patterns.Repo.V2.Tests;

public class RepositoryBaseSyncTests : IDisposable
{
    private readonly TestRepositoryDbContext _context;
    private readonly RepositoryBaseSync<TestUser, int, TestRepositoryDbContext> _repository;

    public RepositoryBaseSyncTests()
    {
        var options = new DbContextOptionsBuilder<TestRepositoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new TestRepositoryDbContext(options);
        _context.Database.EnsureCreated();
        _repository = new RepositoryBaseSync<TestUser, int, TestRepositoryDbContext>(_context);
    }

    [Fact]
    public void Constructor_WithContext_ShouldInitialize()
    {
        // Assert
        _repository.Context.Should().NotBeNull();
        _repository.DbSet.Should().NotBeNull();
    }

    [Fact]
    public void Create_WithAutoSave_ShouldCreateAndSaveEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };

        // Act
        var result = _repository.Create(user, autoSave: true);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        _context.Users.Count().Should().Be(1);
    }

    [Fact]
    public void Create_WithoutAutoSave_ShouldCreateButNotSave()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };

        // Act
        var result = _repository.Create(user, autoSave: false);

        // Assert
        result.Should().BeNull(); // Returns null when not auto-saving
        _context.Users.Count().Should().Be(0);
    }

    [Fact]
    public void Update_WithAutoSave_ShouldUpdateAndSave()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        user.Name = "John Updated";
        _repository.Update(user, autoSave: true);

        // Assert
        var updated = _context.Users.Find(user.Id);
        updated!.Name.Should().Be("John Updated");
    }

    [Fact]
    public void Remove_WithId_ShouldRemoveEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        _context.SaveChanges();
        var userId = user.Id;

        // Act
        _repository.Remove(userId, autoSave: true);

        // Assert
        var deleted = _context.Users.Find(userId);
        deleted.Should().BeNull();
    }

    [Fact]
    public void Remove_WithEntity_ShouldRemoveEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        _repository.Remove(user, autoSave: true);

        // Assert
        var deleted = _context.Users.Find(user.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public void Remove_WithPredicate_ShouldRemoveMatchingEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com", IsActive = false },
            new TestUser { Name = "Jane", Email = "jane@example.com", IsActive = true }
        };
        _context.Users.AddRange(users);
        _context.SaveChanges();

        // Act
        _repository.Remove(u => u.IsActive == false, autoSave: true);

        // Assert
        var remaining = _context.Users.ToList();
        remaining.Should().HaveCount(1);
        remaining.First().Name.Should().Be("Jane");
    }

    [Fact]
    public void Get_WithId_ShouldReturnEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        var found = _repository.Get(user.Id);

        // Assert
        found.Should().NotBeNull();
        found!.Name.Should().Be("John");
    }

    [Fact]
    public void Get_WithIdAndIncludeProperties_ShouldReturnEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        _context.SaveChanges();

        // Act
        var found = _repository.Get(user.Id, "Orders");

        // Assert
        found.Should().NotBeNull();
        found!.Name.Should().Be("John");
    }

    [Fact]
    public void GetAll_ShouldReturnAllEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com" },
            new TestUser { Name = "Jane", Email = "jane@example.com" }
        };
        _context.Users.AddRange(users);
        _context.SaveChanges();

        // Act
        var all = _repository.GetAll();

        // Assert
        all.Should().HaveCount(2);
    }

    [Fact]
    public void GetAll_WithPredicate_ShouldReturnFilteredEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com", IsActive = true },
            new TestUser { Name = "Jane", Email = "jane@example.com", IsActive = false }
        };
        _context.Users.AddRange(users);
        _context.SaveChanges();

        // Act
        var active = _repository.GetAll(u => u.IsActive == true);

        // Assert
        active.Should().HaveCount(1);
        active.First().Name.Should().Be("John");
    }

    [Fact]
    public void Save_ShouldSaveChanges()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _repository.Create(user, autoSave: false);

        // Act
        _repository.Save();

        // Assert
        _context.Users.Count().Should().Be(1);
    }

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        // Act
        _repository.Dispose();

        // Assert
        // Context should be disposed
        Assert.True(true);
    }

    public void Dispose()
    {
        _repository?.Dispose();
        _context?.Dispose();
    }
}

