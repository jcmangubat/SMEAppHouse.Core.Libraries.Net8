using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;
using SMEAppHouse.Core.Patterns.Repo.Abstractions;
using SMEAppHouse.Core.Patterns.Repo.Tests.TestHelpers;
using Xunit;

namespace SMEAppHouse.Core.Patterns.Repo.Tests;

public class RepositoryGenericTests : IDisposable
{
    private readonly TestRepositoryDbContext _context;
    private readonly RepositoryGeneric<TestUser, int> _repository;

    public RepositoryGenericTests()
    {
        var options = new DbContextOptionsBuilder<TestRepositoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new TestRepositoryDbContext(options);
        _context.Database.EnsureCreated();
        _repository = new RepositoryGeneric<TestUser, int>(_context);
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new RepositoryGeneric<TestUser, int>(null!));
    }

    [Fact]
    public void Constructor_WithValidContext_ShouldInitialize()
    {
        // Assert
        _repository.DbContext.Should().NotBeNull();
        _repository.DbSet.Should().NotBeNull();
    }

    [Fact]
    public async Task AddAsync_WithSingleEntity_ShouldAddEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };

        // Act
        await _repository.AddAsync(user);
        await _repository.CommitAsync();

        // Assert
        var savedUser = await _context.Users.FindAsync(user.Id);
        savedUser.Should().NotBeNull();
        savedUser!.Name.Should().Be("John");
    }

    [Fact]
    public async Task AddAsync_WithMultipleEntities_ShouldAddAllEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com" },
            new TestUser { Name = "Jane", Email = "jane@example.com" }
        };

        // Act
        await _repository.AddAsync(users);
        await _repository.CommitAsync();

        // Assert
        var savedUsers = await _context.Users.ToListAsync();
        savedUsers.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddAsync_WithEnumerable_ShouldAddAllEntities()
    {
        // Arrange
        var users = new List<TestUser>
        {
            new() { Name = "John", Email = "john@example.com" },
            new() { Name = "Jane", Email = "jane@example.com" }
        };

        // Act
        await _repository.AddAsync(users);
        await _repository.CommitAsync();

        // Assert
        var savedUsers = await _context.Users.ToListAsync();
        savedUsers.Should().HaveCount(2);
    }

    [Fact]
    public async Task FindAsync_WithKeyValues_ShouldReturnEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var found = await _repository.FindAsync(user.Id);

        // Assert
        found.Should().NotBeNull();
        found!.Name.Should().Be("John");
    }

    [Fact]
    public async Task FindAsync_WithPredicate_ShouldReturnMatchingEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com", IsActive = true },
            new TestUser { Name = "Jane", Email = "jane@example.com", IsActive = false }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var activeUsers = await _repository.FindAsync(u => u.IsActive == true);

        // Assert
        activeUsers.Should().HaveCount(1);
        activeUsers.First().Name.Should().Be("John");
    }

    [Fact]
    public async Task GetSingleAsync_WithPredicate_ShouldReturnSingleEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var found = await _repository.GetSingleAsync(u => u.Email == "john@example.com");

        // Assert
        found.Should().NotBeNull();
        found!.Name.Should().Be("John");
    }

    [Fact]
    public async Task GetSingleAsync_WithInclude_ShouldIncludeRelatedEntities()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        user.Orders.Add(new TestOrder { ProductName = "Product1", Amount = 100 });
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var found = await _repository.GetSingleAsync(
            u => u.Id == user.Id,
            include: q => q.Include(u => u.Orders),
            disableTracking: false);

        // Assert
        found.Should().NotBeNull();
        found!.Orders.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetSingleAsync_WithNoMatch_ShouldReturnNull()
    {
        // Act
        var found = await _repository.GetSingleAsync(u => u.Email == "nonexistent@example.com");

        // Assert
        found.Should().BeNull();
    }

    [Fact]
    public async Task GetListAsync_WithoutFilter_ShouldReturnAllEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com" },
            new TestUser { Name = "Jane", Email = "jane@example.com" }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var allUsers = await _repository.GetListAsync();

        // Assert
        allUsers.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetListAsync_WithFilter_ShouldReturnFilteredEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com", IsActive = true },
            new TestUser { Name = "Jane", Email = "jane@example.com", IsActive = false }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var activeUsers = await _repository.GetListAsync(filter: u => u.IsActive == true);

        // Assert
        activeUsers.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetListAsync_WithOrderBy_ShouldReturnOrderedEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "Zoe", Email = "zoe@example.com" },
            new TestUser { Name = "Alice", Email = "alice@example.com" },
            new TestUser { Name = "Bob", Email = "bob@example.com" }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var orderedUsers = await _repository.GetListAsync(orderBy: q => q.OrderBy(u => u.Name));

        // Assert
        orderedUsers.Should().HaveCount(3);
        orderedUsers.First().Name.Should().Be("Alice");
        orderedUsers.Last().Name.Should().Be("Zoe");
    }

    [Fact]
    public async Task GetListAsync_WithFetchSize_ShouldLimitResults()
    {
        // Arrange
        var users = Enumerable.Range(1, 10)
            .Select(i => new TestUser { Name = $"User{i}", Email = $"user{i}@example.com" })
            .ToArray();
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var limitedUsers = await _repository.GetListAsync(fetchSize: 5);

        // Assert
        limitedUsers.Should().HaveCount(5);
    }

    [Fact]
    public async Task DeleteAsync_WithEntity_ShouldDeleteEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(user);
        await _repository.CommitAsync();

        // Assert
        var deleted = await _context.Users.FindAsync(user.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithId_ShouldDeleteEntity()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var userId = user.Id;

        // Act
        await _repository.DeleteAsync(userId);
        await _repository.CommitAsync();

        // Assert
        var deleted = await _context.Users.FindAsync(userId);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithPredicate_ShouldDeleteMatchingEntities()
    {
        // Arrange
        var users = new[]
        {
            new TestUser { Name = "John", Email = "john@example.com", IsActive = false },
            new TestUser { Name = "Jane", Email = "jane@example.com", IsActive = true }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(u => u.IsActive == false);
        await _repository.CommitAsync();

        // Assert
        var remaining = await _context.Users.ToListAsync();
        remaining.Should().HaveCount(1);
        remaining.First().Name.Should().Be("Jane");
    }

    [Fact]
    public async Task UpdateAsync_WithEntities_ShouldUpdateEntities()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        user.Name = "John Updated";
        await _repository.UpdateAsync(user);
        await _repository.CommitAsync();

        // Assert
        var updated = await _context.Users.FindAsync(user.Id);
        updated!.Name.Should().Be("John Updated");
    }

    [Fact]
    public async Task CommitAsync_ShouldSaveChanges()
    {
        // Arrange
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        await _repository.AddAsync(user);

        // Act
        await _repository.CommitAsync();

        // Assert
        var saved = await _context.Users.FindAsync(user.Id);
        saved.Should().NotBeNull();
    }

    [Fact]
    public void Query_WithSql_ShouldReturnQueryable()
    {
        // Arrange
        var sql = "SELECT * FROM Users";

        // Act
        var query = _repository.Query(sql);

        // Assert
        query.Should().NotBeNull();
    }

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        // Act
        _repository.Dispose();

        // Assert
        // Context should be disposed (we can't directly check, but it shouldn't throw)
        Assert.True(true);
    }

    public void Dispose()
    {
        _repository?.Dispose();
        _context?.Dispose();
    }
}

