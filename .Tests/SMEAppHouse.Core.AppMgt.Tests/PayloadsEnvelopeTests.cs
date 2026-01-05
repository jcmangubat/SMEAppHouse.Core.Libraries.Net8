using FluentAssertions;
using SMEAppHouse.Core.AppMgt.Messaging;
using Xunit;

namespace SMEAppHouse.Core.AppMgt.Tests;

public class PayloadsEnvelopeTests
{
    [Fact]
    public void PayloadsEnvelope_ShouldInitialize()
    {
        // Act
        var envelope = new PayloadsEnvelope();

        // Assert
        envelope.Should().NotBeNull();
    }

    [Fact]
    public void PayloadsEnvelope_QueuePayload_ShouldEnqueuePayload()
    {
        // Arrange
        var envelope = new PayloadsEnvelope();
        var preamble = "test";
        var payload = new { Data = "value" };

        // Act
        envelope.QueuePayload(preamble, payload);

        // Assert
        envelope.PayloadsQueue.Count.Should().Be(1);
    }

    [Fact]
    public void PayloadsEnvelope_DequeuePayload_ShouldDequeuePayload()
    {
        // Arrange
        var envelope = new PayloadsEnvelope();
        var preamble = "test";
        var payload = new { Data = "value" };
        envelope.QueuePayload(preamble, payload);

        // Act
        var result = envelope.DequeuePayload();

        // Assert
        result.Should().NotBeNull();
        result!.Item1.Should().Be(preamble);
        envelope.PayloadsQueue.Count.Should().Be(0);
    }

    [Fact]
    public void PayloadsEnvelope_DequeuePayload_WhenEmpty_ShouldReturnNull()
    {
        // Arrange
        var envelope = new PayloadsEnvelope();

        // Act
        var result = envelope.DequeuePayload();

        // Assert
        result.Should().BeNull();
    }
}

