using FluentAssertions;
using Xunit;

namespace MessageCodec.UnitTests.BinaryMessageCodecTests.ValidationTests;

public class BinaryMessageCodecDecodeValidationTests
{
    private readonly BinaryMessageCodec _codec = new();

    [Fact]
    public void Decode_Should_ThrowException_When_DataIsCorrupted()
    {
        // Arrange
        var corruptedData = new byte[]
        {
            0x01, 0x00
        };

        // Act
        Action act = () => _codec.Decode(corruptedData);

        // Assert
        act.Should().Throw<EndOfStreamException>();
    }
}