using FluentAssertions;
using SMEAppHouse.Core.ProcessService.Specials;
using Xunit;

namespace SMEAppHouse.Core.ProcessService.Tests;

public class AsyncHelpersTests
{
    [Fact]
    public void RunSync_WithTask_ShouldExecuteSynchronously()
    {
        // Arrange
        var executed = false;

        // Act
        AsyncHelpers.RunSync(async () =>
        {
            await Task.Delay(10);
            executed = true;
        });

        // Assert
        executed.Should().BeTrue();
    }

    [Fact]
    public void RunSync_WithTaskReturningValue_ShouldReturnValue()
    {
        // Arrange
        var expectedValue = "Test Result";

        // Act
        var result = AsyncHelpers.RunSync(async () =>
        {
            await Task.Delay(10);
            return expectedValue;
        });

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public void RunSync_WithThrowingTask_ShouldPropagateException()
    {
        // Arrange
        var exceptionMessage = "Test exception";

        // Act & Assert
        var exception = Assert.Throws<AggregateException>(() =>
        {
            AsyncHelpers.RunSync(async () =>
            {
                await Task.Delay(10);
                throw new InvalidOperationException(exceptionMessage);
            });
        });

        exception.InnerException.Should().BeOfType<InvalidOperationException>();
        exception.InnerException!.Message.Should().Be(exceptionMessage);
    }

    [Fact]
    public void RunSync_WithTaskReturningInt_ShouldReturnInt()
    {
        // Arrange
        var expectedValue = 42;

        // Act
        var result = AsyncHelpers.RunSync(async () =>
        {
            await Task.Delay(10);
            return expectedValue;
        });

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public void RunSync_WithComplexAsyncOperation_ShouldComplete()
    {
        // Arrange
        var values = new List<int>();

        // Act
        AsyncHelpers.RunSync(async () =>
        {
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(10);
                values.Add(i);
            }
        });

        // Assert
        values.Should().HaveCount(5);
        values.Should().BeEquivalentTo(new[] { 0, 1, 2, 3, 4 });
    }
}

