using FluentAssertions;
using SMEAppHouse.Core.Patterns.EF.Paging;
using Xunit;

namespace SMEAppHouse.Core.Patterns.EF.Tests.Paging;

public class PageRequestTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var request = new PageRequest();

        // Assert
        request.Page.Should().Be(1);
        request.PageSize.Should().Be(10);
    }

    [Fact]
    public void Page_SetValue_ShouldUpdate()
    {
        // Arrange
        var request = new PageRequest();

        // Act
        request.Page = 5;

        // Assert
        request.Page.Should().Be(5);
    }

    [Fact]
    public void PageSize_SetValue_ShouldUpdate()
    {
        // Arrange
        var request = new PageRequest();

        // Act
        request.PageSize = 20;

        // Assert
        request.PageSize.Should().Be(20);
    }

    [Fact]
    public void PageRequest_WithCustomValues_ShouldStoreValues()
    {
        // Arrange & Act
        var request = new PageRequest 
        { 
            Page = 3, 
            PageSize = 25 
        };

        // Assert
        request.Page.Should().Be(3);
        request.PageSize.Should().Be(25);
    }
}

