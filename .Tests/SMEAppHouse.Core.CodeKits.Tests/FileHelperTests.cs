using FluentAssertions;
using SMEAppHouse.Core.CodeKits.Helpers;
using Xunit;

namespace SMEAppHouse.Core.CodeKits.Tests;

public class FileHelperTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _testFilePath;

    public FileHelperTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        _testFilePath = Path.Combine(_testDirectory, "test.txt");
    }

    [Fact]
    public void FormatShortDateForfilename_WithoutTime_ShouldReturnDateOnly()
    {
        // Arrange
        var date = new DateTime(2024, 3, 15);

        // Act
        var result = FileHelper.FormatShortDateForfilename(date, false);

        // Assert
        result.Should().Be("20240315");
    }

    [Fact]
    public void FormatShortDateForfilename_WithTime_ShouldReturnDateAndTime()
    {
        // Arrange
        var date = new DateTime(2024, 3, 15, 14, 30, 0);

        // Act
        var result = FileHelper.FormatShortDateForfilename(date, true);

        // Assert
        result.Should().Be("202403151430");
    }

    [Fact]
    public void FormatShortDateForfilename_WithPrefixSuffix_ShouldFormatCorrectly()
    {
        // Arrange
        var date = new DateTime(2024, 3, 15);

        // Act
        var result = FileHelper.FormatShortDateForfilename(date, "prefix", "suffix", "txt", false);

        // Assert
        result.Should().Contain("prefix");
        result.Should().Contain("suffix");
        result.Should().Contain(".txt");
        result.Should().Contain("2024-03-15");
    }

    [Fact]
    public void WriteToFile_ShouldCreateFile()
    {
        // Arrange
        var content = "Test content";

        // Act
        FileHelper.WriteToFile(_testFilePath, content, false);

        // Assert
        File.Exists(_testFilePath).Should().BeTrue();
        File.ReadAllText(_testFilePath).Should().Contain(content);
    }

    [Fact]
    public void WriteToFile_WithAppend_ShouldAppendContent()
    {
        // Arrange
        var content1 = "First line";
        var content2 = "Second line";

        // Act
        FileHelper.WriteToFile(_testFilePath, content1, false);
        FileHelper.WriteToFile(_testFilePath, content2, true);

        // Assert
        var fileContent = File.ReadAllText(_testFilePath);
        fileContent.Should().Contain(content1);
        fileContent.Should().Contain(content2);
    }

    [Fact]
    public void WriteToFile2_ShouldCreateFile()
    {
        // Arrange
        var content = "Test content";

        // Act
        FileHelper.WriteToFile2(_testFilePath, content);

        // Assert
        File.Exists(_testFilePath).Should().BeTrue();
        File.ReadAllText(_testFilePath).Should().Contain(content);
    }

    [Fact]
    public void ReadFromFileEachLine_ShouldReturnAllLines()
    {
        // Arrange
        var content = "Test content\nLine 2";
        File.WriteAllText(_testFilePath, content);

        // Act
        var result = FileHelper.ReadFromFileEachLine(_testFilePath);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Should().Be("Test content");
        result[1].Should().Be("Line 2");
    }

    [Fact]
    public void ReadFromFileEachLine_ShouldReturnLines()
    {
        // Arrange
        var lines = new[] { "Line 1", "Line 2", "Line 3" };
        File.WriteAllLines(_testFilePath, lines);

        // Act
        var result = FileHelper.ReadFromFileEachLine(_testFilePath);

        // Assert
        result.Should().HaveCount(3);
        result[0].Should().Be("Line 1");
        result[1].Should().Be("Line 2");
        result[2].Should().Be("Line 3");
    }

    [Fact]
    public void ReadFromFileEachLine_WithNonExistentFile_ShouldThrow()
    {
        // Arrange
        var nonExistentPath = Path.Combine(_testDirectory, "nonexistent.txt");

        // Act & Assert
        Assert.Throws<FileNotFoundException>(() => FileHelper.ReadFromFileEachLine(nonExistentPath));
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}

