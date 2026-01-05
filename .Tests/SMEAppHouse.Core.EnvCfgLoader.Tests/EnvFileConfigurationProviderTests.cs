using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SMEAppHouse.Core.EnvCfgLoader;
using Xunit;

namespace SMEAppHouse.Core.EnvCfgLoader.Tests;

public class EnvFileConfigurationProviderTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _testEnvFile;

    public EnvFileConfigurationProviderTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        _testEnvFile = Path.Combine(_testDirectory, ".env");
    }

    [Fact]
    public void Load_WithValidEnvFile_ShouldLoadConfiguration()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "KEY1=value1\nKEY2=value2");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out var value1).Should().BeTrue();
        value1.Should().Be("value1");
        provider.TryGet("KEY2", out var value2).Should().BeTrue();
        value2.Should().Be("value2");
    }

    [Fact]
    public void Load_WithComments_ShouldIgnoreComments()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "# This is a comment\nKEY1=value1\n# Another comment\nKEY2=value2");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out var value1).Should().BeTrue();
        value1.Should().Be("value1");
        provider.TryGet("KEY2", out var value2).Should().BeTrue();
        value2.Should().Be("value2");
        provider.TryGet("# This is a comment", out _).Should().BeFalse();
    }

    [Fact]
    public void Load_WithEmptyLines_ShouldIgnoreEmptyLines()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "KEY1=value1\n\nKEY2=value2\n\n");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out var value1).Should().BeTrue();
        value1.Should().Be("value1");
        provider.TryGet("KEY2", out var value2).Should().BeTrue();
        value2.Should().Be("value2");
    }

    [Fact]
    public void Load_WithWhitespace_ShouldTrimKeysAndValues()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "  KEY1  =  value1  \nKEY2=value2");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out var value1).Should().BeTrue();
        value1.Should().Be("value1");
    }

    [Fact]
    public void Load_WithDoubleUnderscore_ShouldConvertToColon()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "DATABASE__CONNECTION__STRING=Server=localhost");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("DATABASE:CONNECTION:STRING", out var value).Should().BeTrue();
        value.Should().Be("Server=localhost");
    }

    [Fact]
    public void Load_WithEmptyValue_ShouldAllowEmptyValue()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "KEY1=\nKEY2=value2");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out var value1).Should().BeTrue();
        value1.Should().BeEmpty();
    }

    [Fact]
    public void Load_WithNoEqualsSign_ShouldSkipLine()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "KEY1=value1\nINVALID_LINE\nKEY2=value2");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out _).Should().BeTrue();
        provider.TryGet("INVALID_LINE", out _).Should().BeFalse();
        provider.TryGet("KEY2", out _).Should().BeTrue();
    }

    [Fact]
    public void Load_WithEmptyKey_ShouldSkipLine()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "KEY1=value1\n=value2\nKEY3=value3");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out _).Should().BeTrue();
        provider.TryGet("KEY3", out _).Should().BeTrue();
    }

    [Fact]
    public void Load_WithMultipleEquals_ShouldUseFirstEqualsAsSeparator()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "KEY1=value=with=equals");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out var value).Should().BeTrue();
        value.Should().Be("value=with=equals");
    }

    [Fact]
    public void Load_WithNonExistentFile_ShouldNotThrow()
    {
        // Arrange
        var nonExistentFile = Path.Combine(_testDirectory, "nonexistent.env");
        var provider = new EnvFileConfigurationProvider(nonExistentFile);

        // Act
        var act = () => provider.Load();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Load_WithOptionalFile_ShouldNotThrowWhenFileMissing()
    {
        // Arrange
        var nonExistentFile = Path.Combine(_testDirectory, "nonexistent.env");
        var provider = new EnvFileConfigurationProvider(nonExistentFile, optional: true);

        // Act
        var act = () => provider.Load();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Load_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "KEY1=value with spaces\nKEY2=value\"with\"quotes\nKEY3=value\\with\\backslashes");

        var provider = new EnvFileConfigurationProvider(_testEnvFile);

        // Act
        provider.Load();

        // Assert
        provider.TryGet("KEY1", out var value1).Should().BeTrue();
        value1.Should().Be("value with spaces");
    }

    [Fact]
    public void ConfigurationBuilder_AddEnvFile_ShouldLoadConfiguration()
    {
        // Arrange
        File.WriteAllText(_testEnvFile, "TEST_KEY=test_value");

        var builder = new ConfigurationBuilder();

        // Act
        builder.AddEnvFile(_testEnvFile);
        var configuration = builder.Build();

        // Assert
        configuration["TEST_KEY"].Should().Be("test_value");
    }

    [Fact]
    public void ConfigurationBuilder_AddEnvFile_WithDefaultPath_ShouldUseDefault()
    {
        // Arrange
        var defaultEnvFile = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (File.Exists(defaultEnvFile))
        {
            File.Delete(defaultEnvFile);
        }

        File.WriteAllText(defaultEnvFile, "DEFAULT_KEY=default_value");

        try
        {
            var builder = new ConfigurationBuilder();

            // Act
            builder.AddEnvFile();
            var configuration = builder.Build();

            // Assert
            configuration["DEFAULT_KEY"].Should().Be("default_value");
        }
        finally
        {
            if (File.Exists(defaultEnvFile))
            {
                File.Delete(defaultEnvFile);
            }
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}

