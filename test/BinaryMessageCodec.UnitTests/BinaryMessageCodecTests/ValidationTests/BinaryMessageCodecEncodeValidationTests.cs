using Bogus;
using FluentAssertions;
using BinaryMessageCodec.Models;
using Xunit;

namespace BinaryMessageCodec.UnitTests.BinaryMessageCodecTests.ValidationTests;

public class BinaryMessageCodecEncodeValidationTests
{
    private readonly BinaryMessageCodec _messageCodec = new();
    private readonly Faker _faker = new();

    [Theory]
    [InlineData(64)]
    public void Encode_Should_ThrowException_When_HeaderCountExceedsLimit(int headerCount)
    {
        // Arrange
        var headers = Enumerable.Range(0, headerCount)
            .ToDictionary(i => $"Header{i}", i => $"Value{i}");
        var message = new Message(headers, []);

        // Act
        Action act = () => _messageCodec.Encode(message);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Headers quantity cannot exceed 63.");
    }

    [Theory]
    [InlineData(1024)]
    public void Encode_Should_ThrowException_When_HeaderNameExceedsLimit(int headerNameLength)
    {
        // Arrange
        var longHeaderName = new string('a', headerNameLength);
        var message = new Message(
            new Dictionary<string, string>
            {
                {
                    longHeaderName, "Value"
                }
            },
            []
        );

        // Act
        Action act = () => _messageCodec.Encode(message);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Header name cannot exceed 1023 bytes*");
    }

    [Theory]
    [InlineData(1024)]
    public void Encode_Should_ThrowException_When_HeaderValueExceedsLimit(int headerValueLength)
    {
        // Arrange
        var longHeaderValue = new string('a', headerValueLength);
        var message = new Message(
            new Dictionary<string, string>
            {
                {
                    "HeaderName", longHeaderValue
                }
            },
            []
        );

        // Act
        Action act = () => _messageCodec.Encode(message);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Header value cannot exceed 1023 bytes*");
    }

    [Theory]
    [InlineData(256 * 1024)]
    public void Encode_Should_ThrowException_When_PayloadExceedsLimit(int payloadMaxKiBLength)
    {
        // Arrange
        var payload = _faker.Random.Bytes(payloadMaxKiBLength + 1);
        var message = new Message(new Dictionary<string, string>(), payload);

        // Act
        Action act = () => _messageCodec.Encode(message);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Payload cannot exceed {payloadMaxKiBLength} bytes.");
    }

    [Fact]
    public void Encode_Should_ThrowException_When_HeaderNameContainsNonASCIICharacters()
    {
        // Arrange
        var headers = new Dictionary<string, string>
        {
            {
                "Header", "Valüe"
            }
        };
        var message = new Message(headers, []);
        var codec = new BinaryMessageCodec();

        // Act
        Action act = () => codec.Encode(message);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("String contains non-ASCII characters. String: Valüe");
    }

    [Fact]
    public void Encode_Should_ThrowException_When_HeaderValueContainsNonASCIICharacters()
    {
        // Arrange
        var headers = new Dictionary<string, string>
        {
            {
                "Hüder", "Value"
            }
        };
        var message = new Message(headers, []);
        var codec = new BinaryMessageCodec();

        // Act
        Action act = () => codec.Encode(message);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("String contains non-ASCII characters. String: Hüder");
    }
}