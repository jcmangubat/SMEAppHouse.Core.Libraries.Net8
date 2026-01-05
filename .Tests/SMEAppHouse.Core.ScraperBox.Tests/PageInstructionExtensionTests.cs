using FluentAssertions;
using SMEAppHouse.Core.ScraperBox;
using SMEAppHouse.Core.ScraperBox.Models;
using Xunit;

namespace SMEAppHouse.Core.ScraperBox.Tests;

public class PageInstructionExtensionTests
{
    [Theory]
    [InlineData(1, 3, PageInstruction.PaddingDirectionsEnum.ToLeft, '0', "001")]
    [InlineData(12, 3, PageInstruction.PaddingDirectionsEnum.ToLeft, '0', "012")]
    [InlineData(123, 3, PageInstruction.PaddingDirectionsEnum.ToLeft, '0', "123")]
    [InlineData(1, 3, PageInstruction.PaddingDirectionsEnum.ToRight, '0', "100")]
    [InlineData(12, 3, PageInstruction.PaddingDirectionsEnum.ToRight, '0', "120")]
    [InlineData(123, 3, PageInstruction.PaddingDirectionsEnum.ToRight, '0', "123")]
    [InlineData(5, 4, PageInstruction.PaddingDirectionsEnum.ToLeft, ' ', "   5")]
    public void PageNo_ShouldFormatPageNumberCorrectly(
        int pageNo,
        int padLength,
        PageInstruction.PaddingDirectionsEnum direction,
        char padChar,
        string expected)
    {
        // Arrange
        var instruction = new PageInstruction
        {
            PadLength = padLength,
            PaddingDirection = direction,
            PadCharacter = padChar
        };

        // Act
        var result = Helper.PageNo(instruction, pageNo);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void PageNo_WithPageNumberExceedingPadLength_ShouldThrowException()
    {
        // Arrange
        var instruction = new PageInstruction
        {
            PadLength = 2,
            PaddingDirection = PageInstruction.PaddingDirectionsEnum.ToLeft,
            PadCharacter = '0'
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => Helper.PageNo(instruction, 123));
        exception.Message.Should().Contain("Page number character length exceeds page instruction pad length");
    }
}

