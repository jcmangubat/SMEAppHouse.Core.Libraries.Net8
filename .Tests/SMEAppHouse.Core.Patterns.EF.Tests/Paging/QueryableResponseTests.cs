using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SMEAppHouse.Core.Patterns.EF.Paging;
using SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers;
using System.Linq;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.Paging;

public class QueryableResponseTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Act
        var response = new QueryableResponse<TestEntity>();

        // Assert
        response.Data.Should().NotBeNull(); // Initialized to empty queryable
        response.PageRequest.Should().BeNull();
        response.TotalRecords.Should().Be(0);
        response.TotalPages.Should().Be(0);
    }

    [Fact]
    public void QueryableResponse_WithQueryable_ShouldStoreQueryable()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        var queryable = context.TestEntities.AsQueryable();
        var pageRequest = new PageRequest { Page = 1, PageSize = 10 };

        // Act
        var response = new QueryableResponse<TestEntity>
        {
            Data = queryable,
            PageRequest = pageRequest,
            TotalRecords = 0,
            TotalPages = 0
        };

        // Assert
        response.Data.Should().NotBeNull();
        response.Data.Should().BeSameAs(queryable);
        response.PageRequest.Should().Be(pageRequest);
    }

    [Fact]
    public void QueryableResponse_WithFilteredQueryable_ShouldWork()
    {
        // Arrange
        using var context = DbContextTestHelper.CreateInMemoryContext();
        context.Database.EnsureCreated();
        
        var entities = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "Entity1", Email = "e1@test.com" },
            new TestEntity { Id = 2, Name = "Entity2", Email = "e2@test.com" }
        };
        context.TestEntities.AddRange(entities);
        context.SaveChanges();

        var filteredQuery = context.TestEntities.Where(e => e.Id > 0);
        var pageRequest = new PageRequest { Page = 1, PageSize = 10 };

        // Act
        var response = new QueryableResponse<TestEntity>
        {
            Data = filteredQuery,
            PageRequest = pageRequest,
            TotalRecords = 2,
            TotalPages = 1
        };

        // Assert
        response.Data.Should().NotBeNull();
        response.Data.Count().Should().Be(2);
        response.TotalRecords.Should().Be(2);
    }
}

