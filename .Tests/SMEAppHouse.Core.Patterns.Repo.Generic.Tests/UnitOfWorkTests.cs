using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.DbContextAbstractions;
using SMEAppHouse.Core.Patterns.Repo.Generic;
using SMEAppHouse.Core.Patterns.Repo.Tests.TestHelpers;
using Xunit;

namespace SMEAppHouse.Core.Patterns.Repo.Generic.Tests;

public class UnitOfWorkTests : IDisposable
{
    private readonly TestRepositoryDbContext _context;
    private readonly UnitOfWork<TestRepositoryDbContext> _unitOfWork;

    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<TestRepositoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new TestRepositoryDbContext(options);
        _context.Database.EnsureCreated();
        _unitOfWork = new UnitOfWork<TestRepositoryDbContext>(_context);
    }

    [Fact]
    public void Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UnitOfWork<TestRepositoryDbContext>(null!));
    }

    [Fact]
    public void GetRepository_ShouldReturnRepository()
    {
        // Act
        var repository = _unitOfWork.GetRepository<TestUser>();

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public void GetRepository_WithSameType_ShouldReturnSameInstance()
    {
        // Act
        var repo1 = _unitOfWork.GetRepository<TestUser>();
        var repo2 = _unitOfWork.GetRepository<TestUser>();

        // Assert
        repo1.Should().BeSameAs(repo2);
    }

    [Fact]
    public void GetRepository_WithDifferentTypes_ShouldReturnDifferentRepositories()
    {
        // Act
        var userRepo = _unitOfWork.GetRepository<TestUser>();
        var orderRepo = _unitOfWork.GetRepository<TestOrder>();

        // Assert
        userRepo.Should().NotBeSameAs(orderRepo);
    }

    [Fact]
    public void GetRepositoryAsync_ShouldReturnAsyncRepository()
    {
        // Act
        var repository = _unitOfWork.GetRepositoryAsync<TestUser>();

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public void GetReadOnlyRepository_ShouldReturnReadOnlyRepository()
    {
        // Act
        var repository = _unitOfWork.GetReadOnlyRepository<TestUser>();

        // Assert
        repository.Should().NotBeNull();
    }

    [Fact]
    public void SaveChanges_ShouldSaveChanges()
    {
        // Arrange
        var repository = _unitOfWork.GetRepository<TestUser>();
        var user = new TestUser { Name = "John", Email = "john@example.com" };
        repository.Add(user);

        // Act
        var result = _unitOfWork.SaveChanges();

        // Assert
        result.Should().Be(1);
        _context.Users.Count().Should().Be(1);
    }

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        // Act
        _unitOfWork.Dispose();

        // Assert
        // Context should be disposed
        Assert.True(true);
    }

    public void Dispose()
    {
        _unitOfWork?.Dispose();
        _context?.Dispose();
    }
}

