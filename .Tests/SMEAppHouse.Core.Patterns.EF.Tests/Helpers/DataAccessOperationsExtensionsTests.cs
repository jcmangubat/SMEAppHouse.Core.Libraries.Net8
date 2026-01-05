using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.EntityCompositing.Base;
using SMEAppHouse.Core.Patterns.EF.Helpers;
using SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers;
using TestEntityWithGuid = SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers.TestEntityWithGuid;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.Helpers;

public class DataAccessOperationsExtensionsTests
{
    [Fact]
    public void GetPrimaryKeyProperties_WithValidEntity_ShouldReturnPrimaryKeyProperties()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();

        // Act
        var keyProperties = context.GetPrimaryKeyProperties<TestEntity>();

        // Assert
        keyProperties.Should().NotBeNull();
        keyProperties.Count.Should().Be(1);
        keyProperties[0].Name.Should().Be("Id");
    }

    [Fact]
    public void GetPrimaryKeyProperties_WithNonExistentEntity_ShouldThrowInvalidOperationException()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            context.GetPrimaryKeyProperties<NonExistentEntity>());
    }

    [Fact]
    public void GetPrimaryKeyProperties_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ((DbContext)null!).GetPrimaryKeyProperties<TestEntity>());
    }

    [Fact]
    public void FilterByPrimaryKeyPredicate_WithValidId_ShouldReturnPredicate()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        var id = new object[] { 1 };

        // Act
        var predicate = context.FilterByPrimaryKeyPredicate<TestEntity>(id);

        // Assert
        predicate.Should().NotBeNull();
    }

    [Fact]
    public void FilterByPrimaryKeyPredicate_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange
        var id = new object[] { 1 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ((DbContext)null!).FilterByPrimaryKeyPredicate<TestEntity>(id));
    }

    [Fact]
    public void FilterByPrimaryKeyPredicate_WithNullId_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            context.FilterByPrimaryKeyPredicate<TestEntity>(null!));
    }

    [Fact]
    public void FilterByPrimaryKeyPredicate_WithMismatchedIdCount_ShouldThrowArgumentException()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        var id = new object[] { 1, 2 }; // Too many values

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            context.FilterByPrimaryKeyPredicate<TestEntity>(id));
    }

    [Fact]
    public void FilterByPrimaryKey_WithValidId_ShouldReturnFilteredQueryable()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        
        var entity1 = new TestEntity { Id = 1, Name = "Entity1", Email = "e1@test.com" };
        var entity2 = new TestEntity { Id = 2, Name = "Entity2", Email = "e2@test.com" };
        
        context.TestEntities.AddRange(entity1, entity2);
        context.SaveChanges();

        // Act
        var filtered = context.TestEntities.FilterByPrimaryKey(context, new object[] { 1 });

        // Assert
        filtered.Should().NotBeNull();
        filtered.Count().Should().Be(1);
        filtered.First().Id.Should().Be(1);
    }

    [Fact]
    public void FilterByPrimaryKey_WithNullDbSet_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        var id = new object[] { 1 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ((DbSet<TestEntity>)null!).FilterByPrimaryKey(context, id));
    }

    [Fact]
    public void DetachLocal_WithValidEntity_ShouldDetachAndAttach()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        
        var existingEntity = new TestEntity { Id = 1, Name = "Original", Email = "original@test.com" };
        context.TestEntities.Add(existingEntity);
        context.SaveChanges();

        var updatedEntity = new TestEntity { Id = 1, Name = "Updated", Email = "updated@test.com" };

        // Act
        context.DetachLocal<TestEntity, int>(updatedEntity, "1");

        // Assert
        context.Entry(updatedEntity).State.Should().Be(EntityState.Modified);
    }

    [Fact]
    public void DetachLocal_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Test", Email = "test@test.com" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ((DbContext)null!).DetachLocal<TestEntity, int>(entity, "1"));
    }

    [Fact]
    public void DetachLocal_WithNullEntity_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            context.DetachLocal<TestEntity, int>(null!, "1"));
    }

    [Fact]
    public void DetachLocal_WithNullEntryId_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        var entity = new TestEntity { Id = 1, Name = "Test", Email = "test@test.com" };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            context.DetachLocal<TestEntity, int>(entity, null!));
    }

    [Fact]
    public void DetachLocal_WithGuidPrimaryKey_ShouldWork()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        
        var guidId = Guid.NewGuid();
        var existingEntity = new TestEntityWithGuid { Id = guidId, Name = "Original" };
        context.TestEntitiesWithGuid.Add(existingEntity);
        context.SaveChanges();

        var updatedEntity = new TestEntityWithGuid { Id = guidId, Name = "Updated" };

        // Act
        context.DetachLocal<TestEntityWithGuid, Guid>(updatedEntity, guidId.ToString());

        // Assert
        context.Entry(updatedEntity).State.Should().Be(EntityState.Modified);
    }

    [Fact]
    public void Paging_WithValidParameters_ShouldReturnPagedQuery()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        
        var entities = Enumerable.Range(1, 25)
            .Select(i => new TestEntity { Name = $"Entity{i}", Email = $"e{i}@test.com" })
            .ToList();
        
        context.TestEntities.AddRange(entities);
        context.SaveChanges();

        var query = context.TestEntities.AsQueryable();

        // Act
        var paged = query.Paging(10, 2);

        // Assert
        paged.Should().NotBeNull();
        paged.Count().Should().Be(10);
    }

    [Fact]
    public void Paging_WithZeroPageSize_ShouldReturnOriginalQuery()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        var query = context.TestEntities.AsQueryable();

        // Act
        var paged = query.Paging(0, 1);

        // Assert
        paged.Should().BeSameAs(query);
    }

    [Fact]
    public void Paging_WithZeroPageNumber_ShouldReturnOriginalQuery()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        var query = context.TestEntities.AsQueryable();

        // Act
        var paged = query.Paging(10, 0);

        // Assert
        paged.Should().BeSameAs(query);
    }

    [Fact]
    public void Paging_WithNullQuery_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            ((IQueryable<TestEntity>)null!).Paging(10, 1));
    }
}

/// <summary>
/// Non-existent entity for testing
/// </summary>
public class NonExistentEntity
{
    public int Id { get; set; }
}

