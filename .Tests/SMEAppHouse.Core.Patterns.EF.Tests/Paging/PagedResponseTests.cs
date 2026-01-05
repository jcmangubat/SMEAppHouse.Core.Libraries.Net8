using FluentAssertions;
using SMEAppHouse.Core.Patterns.EF.Paging;
using SMEAppHouse.Core.Patterns.EF.Tests.TestHelpers;
using SMEAppHouse.Core.Patterns.Repo.Paging;
using System.Linq.Dynamic.Core;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.Paging;

public class PagedResponseTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Act
        var response = new PagedResult<TestEntity>();

        // Assert
        response.Data.Should().BeNull();
        response.PageRequest.Should().BeNull();
        response.TotalRecords.Should().Be(0);
        response.TotalPages.Should().Be(0);
    }

    [Fact]
    public void PagedResponse_WithData_ShouldStoreData()
    {
        // Arrange
        var entities = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "Entity1", Email = "e1@test.com" },
            new TestEntity { Id = 2, Name = "Entity2", Email = "e2@test.com" }
        };
        var pageRequest = new PageRequest { Page = 1, PageSize = 10 };

        // Act
        var response = new PagedResponse<TestEntity>
        {
            Data = entities,
            PageRequest = pageRequest,
            TotalRecords = 2,
            TotalPages = 1
        };

        // Assert
        response.Data.Should().HaveCount(2);
        response.PageRequest.Should().Be(pageRequest);
        response.TotalRecords.Should().Be(2);
        response.TotalPages.Should().Be(1);
    }

    [Fact]
    public void PagedResponse_WithEmptyData_ShouldWork()
    {
        // Arrange & Act
        var response = new PagedResponse<TestEntity>
        {
            Data = new List<TestEntity>(),
            PageRequest = new PageRequest { Page = 1, PageSize = 10 },
            TotalRecords = 0,
            TotalPages = 0
        };

        // Assert
        response.Data.Should().BeEmpty();
        response.TotalRecords.Should().Be(0);
        response.TotalPages.Should().Be(0);
    }
}

