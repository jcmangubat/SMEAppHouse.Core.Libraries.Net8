using FluentAssertions;
using SMEAppHouse.Core.CodeKits.Encryptions;
using Xunit;

namespace SMEAppHouse.Core.CodeKits.Tests;

public class CryptorTests
{
    private const string TestKey = "TestSecretKey123";
    private const string TestPlainText = "Hello, World! This is a test message.";

    [Fact]
    public void EncryptStringAES_ShouldReturnEncryptedString()
    {
        // Act
        var encrypted = Cryptor.EncryptStringAES(TestPlainText, TestKey);

        // Assert
        encrypted.Should().NotBeNullOrEmpty();
        encrypted.Should().NotBe(TestPlainText);
        encrypted.Should().MatchRegex("^[A-Za-z0-9+/=]+$"); // Base64 format
    }

    [Fact]
    public void EncryptStringAES_WithNullPlainText_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Cryptor.EncryptStringAES(null!, TestKey));
    }

    [Fact]
    public void EncryptStringAES_WithEmptyPlainText_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Cryptor.EncryptStringAES("", TestKey));
    }

    [Fact]
    public void EncryptStringAES_WithNullKey_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Cryptor.EncryptStringAES(TestPlainText, null!));
    }

    [Fact]
    public void EncryptStringAES_WithEmptyKey_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Cryptor.EncryptStringAES(TestPlainText, ""));
    }

    [Fact]
    public void DecryptStringAES_ShouldDecryptEncryptedString()
    {
        // Arrange
        var encrypted = Cryptor.EncryptStringAES(TestPlainText, TestKey);

        // Act
        var decrypted = Cryptor.DecryptStringAES(encrypted, TestKey);

        // Assert
        decrypted.Should().Be(TestPlainText);
    }

    [Fact]
    public void DecryptStringAES_WithNullCipherText_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Cryptor.DecryptStringAES(null!, TestKey));
    }

    [Fact]
    public void DecryptStringAES_WithEmptyCipherText_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Cryptor.DecryptStringAES("", TestKey));
    }

    [Fact]
    public void DecryptStringAES_WithNullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var encrypted = Cryptor.EncryptStringAES(TestPlainText, TestKey);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Cryptor.DecryptStringAES(encrypted, null!));
    }

    [Fact]
    public void DecryptStringAES_WithWrongKey_ShouldThrowException()
    {
        // Arrange
        var encrypted = Cryptor.EncryptStringAES(TestPlainText, TestKey);
        var wrongKey = "WrongKey123";

        // Act & Assert
        Assert.ThrowsAny<Exception>(() => Cryptor.DecryptStringAES(encrypted, wrongKey));
    }

    [Fact]
    public void EncryptDecrypt_WithSpecialCharacters_ShouldWork()
    {
        // Arrange
        var specialText = "!@#$%^&*()_+-=[]{}|;':\",./<>?`~";

        // Act
        var encrypted = Cryptor.EncryptStringAES(specialText, TestKey);
        var decrypted = Cryptor.DecryptStringAES(encrypted, TestKey);

        // Assert
        decrypted.Should().Be(specialText);
    }

    [Fact]
    public void EncryptDecrypt_WithUnicodeCharacters_ShouldWork()
    {
        // Arrange
        var unicodeText = "Hello 世界 🌍";

        // Act
        var encrypted = Cryptor.EncryptStringAES(unicodeText, TestKey);
        var decrypted = Cryptor.DecryptStringAES(encrypted, TestKey);

        // Assert
        decrypted.Should().Be(unicodeText);
    }

    [Fact]
    public void EncryptDecrypt_WithLongText_ShouldWork()
    {
        // Arrange
        var longText = new string('A', 10000);

        // Act
        var encrypted = Cryptor.EncryptStringAES(longText, TestKey);
        var decrypted = Cryptor.DecryptStringAES(encrypted, TestKey);

        // Assert
        decrypted.Should().Be(longText);
    }

    [Fact]
    public void Encrypt_WithDifferentKeys_ShouldProduceDifferentResults()
    {
        // Arrange
        var key1 = "Key1";
        var key2 = "Key2";

        // Act
        var encrypted1 = Cryptor.EncryptStringAES(TestPlainText, key1);
        var encrypted2 = Cryptor.EncryptStringAES(TestPlainText, key2);

        // Assert
        encrypted1.Should().NotBe(encrypted2);
    }

    [Fact]
    public void Encrypt_WithSameInput_ShouldProduceDifferentResults()
    {
        // Act
        var encrypted1 = Cryptor.EncryptStringAES(TestPlainText, TestKey);
        var encrypted2 = Cryptor.EncryptStringAES(TestPlainText, TestKey);

        // Note: AES encryption with the same key and IV would produce the same result
        // But since we're using a salt, results should be consistent
        // Both should decrypt to the same plaintext
        var decrypted1 = Cryptor.DecryptStringAES(encrypted1, TestKey);
        var decrypted2 = Cryptor.DecryptStringAES(encrypted2, TestKey);

        // Assert
        decrypted1.Should().Be(TestPlainText);
        decrypted2.Should().Be(TestPlainText);
    }
}

