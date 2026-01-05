using FluentAssertions;
using SMEAppHouse.Core.GHClientLib.Model;
using Xunit;

namespace SMEAppHouse.Core.GHClientLib.Tests;

public class AddressTests
{
    [Fact]
    public void Address_Constructor_ShouldInitialize()
    {
        // Act
        var address = new Address();

        // Assert
        address.Should().NotBeNull();
    }

    [Fact]
    public void Address_Constructor_WithParameters_ShouldSetProperties()
    {
        // Arrange
        var locationId = "loc1";
        var name = "Test Address";
        var lon = 120.5;
        var lat = 14.6;

        // Act
        var address = new Address(locationId, name, lon, lat);

        // Assert
        address.LocationId.Should().Be(locationId);
        address.Name.Should().Be(name);
        address.Lon.Should().Be(lon);
        address.Lat.Should().Be(lat);
    }

    [Fact]
    public void Address_Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var address1 = new Address("loc1", "Test", 120.5, 14.6);
        var address2 = new Address("loc1", "Test", 120.5, 14.6);

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Address_Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var address1 = new Address("loc1", "Test", 120.5, 14.6);
        var address2 = new Address("loc2", "Test", 120.5, 14.6);

        // Act
        var result = address1.Equals(address2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Address_ToJson_ShouldReturnValidJson()
    {
        // Arrange
        var address = new Address("loc1", "Test", 120.5, 14.6);

        // Act
        var json = address.ToJson();

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("location_id");
        json.Should().Contain("loc1");
    }
}

