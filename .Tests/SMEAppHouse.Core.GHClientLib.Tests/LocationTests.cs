using FluentAssertions;
using SMEAppHouse.Core.GHClientLib.Model;
using Xunit;

namespace SMEAppHouse.Core.GHClientLib.Tests;

public class LocationTests
{
    [Fact]
    public void Location_Constructor_ShouldInitialize()
    {
        // Act
        var location = new Location();

        // Assert
        location.Should().NotBeNull();
    }

    [Fact]
    public void Location_Constructor_WithParameters_ShouldSetProperties()
    {
        // Arrange
        var locationId = "loc1";
        var lon = 120.5;
        var lat = 14.6;

        // Act
        var location = new Location(locationId, lon, lat);

        // Assert
        location.LocationId.Should().Be(locationId);
        location.Lon.Should().Be(lon);
        location.Lat.Should().Be(lat);
    }

    [Fact]
    public void Location_FullAddress_ShouldBeSettable()
    {
        // Arrange
        var location = new Location();
        var address = "123 Main St, City";

        // Act
        location.FullAddress = address;

        // Assert
        location.FullAddress.Should().Be(address);
    }

    [Fact]
    public void Location_ToLineText_ShouldReturnFormattedString()
    {
        // Arrange
        var location = new Location("loc1", 120.5, 14.6)
        {
            FullAddress = "123 Main St"
        };

        // Act
        var text = location.ToLineText();

        // Assert
        text.Should().Contain("loc1");
        text.Should().Contain("123 Main St");
        text.Should().Contain("120.5");
        text.Should().Contain("14.6");
    }

    [Fact]
    public void Location_Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var location1 = new Location("loc1", 120.5, 14.6);
        var location2 = new Location("loc1", 120.5, 14.6);

        // Act
        var result = location1.Equals(location2);

        // Assert
        result.Should().BeTrue();
    }
}

