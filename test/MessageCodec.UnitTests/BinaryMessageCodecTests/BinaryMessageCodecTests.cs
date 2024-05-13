using Bogus;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;
using Message = MessageCodec.Models.Message;

namespace MessageCodec.UnitTests.BinaryMessageCodecTests;

public class BinaryMessageCodecTests
{
    private readonly BinaryMessageCodec _codec = new();
    private readonly Faker _faker = new();

    [Fact]
    public void EncodeDecode_Should_ReturnCorrectMessage()
    {
        // Arrange
        var originalMessage = new Message(
            new Dictionary<string, string>
            {
                {
                    "Header1", "Value1"
                },
                {
                    "Header2", "Value2"
                }
            },
            "Payload"u8.ToArray()
        );

        // Act
        var encoded = _codec.Encode(originalMessage);
        var decodedMessage = _codec.Decode(encoded);

        // Assert
        using (new AssertionScope())
        {
            decodedMessage.Headers.Should().HaveCount(originalMessage.Headers.Count);
            decodedMessage.Headers["Header1"].Should().Be("Value1");
            decodedMessage.Headers["Header2"].Should().Be("Value2");
            decodedMessage.Payload.Should().Equal(originalMessage.Payload);
        }
    }

    [Fact]
    public void EncodeDecode_Should_Handle_EmptyMessage()
    {
        // Arrange
        var originalMessage = new Message(new Dictionary<string, string>(), []);

        // Act
        var encoded = _codec.Encode(originalMessage);
        var decodedMessage = _codec.Decode(encoded);

        // Assert
        using (new AssertionScope())
        {
            decodedMessage.Headers.Should().BeEmpty();
            decodedMessage.Payload.Should().BeEmpty();
        }
    }

    [Fact]
    public void Encode_Decode_Should_Handle_ZeroHeaders()
    {
        // Arrange
        var message = new Message(new Dictionary<string, string>(), "Some payload"u8.ToArray());

        // Act
        var encoded = _codec.Encode(message);
        var decodedMessage = _codec.Decode(encoded);

        // Assert
        using (new AssertionScope())
        {
            decodedMessage.Headers.Should().BeEmpty();
            decodedMessage.Payload.Should().NotBeEmpty();
        }
    }

    [Fact]
    public void Encode_Decode_Should_Handle_EmptyPayload()
    {
        // Arrange
        var message = new Message(new Dictionary<string, string>
        {
            {
                "Key", "Value"
            }
        }, []);

        // Act
        var encoded = _codec.Encode(message);
        var decodedMessage = _codec.Decode(encoded);

        // Assert
        using (new AssertionScope())
        {
            decodedMessage.Headers.Should().NotBeEmpty();
            decodedMessage.Payload.Should().BeEmpty();
        }
    }

    [Fact]
    public void Encode_Decode_Should_Handle_MaxHeaderSize()
    {
        // Arrange
        var maxHeaderName = new string('N', 1023);
        var maxHeaderValue = new string('V', 1023);
        var originalMessage = new Message(
            new Dictionary<string, string>
            {
                {
                    maxHeaderName, maxHeaderValue
                }
            },
            []
        );

        // Act
        var encoded = _codec.Encode(originalMessage);
        var decodedMessage = _codec.Decode(encoded);

        // Assert
        using (new AssertionScope())
        {
            decodedMessage.Headers.Should().ContainKey(maxHeaderName);
            decodedMessage.Headers[maxHeaderName].Should().Be(maxHeaderValue);
            decodedMessage.Payload.Should().BeEmpty();
        }
    }

    [Fact]
    public void Encode_Decode_Should_Handle_MaxPayloadSize()
    {
        // Arrange
        var payload = _faker.Random.Bytes(256 * 1024);
        var originalMessage = new Message(
            new Dictionary<string, string>(),
            payload
        );

        // Act
        var encoded = _codec.Encode(originalMessage);
        var decodedMessage = _codec.Decode(encoded);

        // Assert
        decodedMessage.Payload.Should().BeEquivalentTo(payload);
    }
}