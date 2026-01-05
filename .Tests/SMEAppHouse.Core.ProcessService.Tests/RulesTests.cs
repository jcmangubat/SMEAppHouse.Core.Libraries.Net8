using FluentAssertions;
using SMEAppHouse.Core.ProcessService;
using Xunit;

namespace SMEAppHouse.Core.ProcessService.Tests;

public class RulesTests
{
    [Fact]
    public void EngineStatusEnum_ShouldHaveExpectedValues()
    {
        // Assert
        Enum.GetValues<EngineStatusEnum>().Should().Contain(EngineStatusEnum.NonState);
        Enum.GetValues<EngineStatusEnum>().Should().Contain(EngineStatusEnum.RunningState);
        Enum.GetValues<EngineStatusEnum>().Should().Contain(EngineStatusEnum.PausedState);
    }

    [Fact]
    public void EngineStatusChangedEventArgs_ShouldSetEngineStatus()
    {
        // Arrange
        var status = EngineStatusEnum.RunningState;

        // Act
        var args = new EngineStatusChangedEventArgs(status);

        // Assert
        args.EngineStatus.Should().Be(status);
    }

    [Fact]
    public void BeforeEngineStatusChangedEventArgs_ShouldInheritFromEngineStatusChangedEventArgs()
    {
        // Arrange
        var status = EngineStatusEnum.PausedState;

        // Act
        var args = new BeforeEngineStatusChangedEventArgs(status);

        // Assert
        args.Should().BeAssignableTo<EngineStatusChangedEventArgs>();
        args.EngineStatus.Should().Be(status);
        args.Cancel.Should().BeFalse();
    }

    [Fact]
    public void BeforeEngineStatusChangedEventArgs_Cancel_ShouldBeSettable()
    {
        // Arrange
        var args = new BeforeEngineStatusChangedEventArgs(EngineStatusEnum.RunningState);

        // Act
        args.Cancel = true;

        // Assert
        args.Cancel.Should().BeTrue();
    }
}

