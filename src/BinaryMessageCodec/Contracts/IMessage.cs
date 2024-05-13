using System.Collections.Generic;

namespace BinaryMessageCodec.Contracts;

public interface IMessage
{
    Dictionary<string, string> Headers { get; init; }
    byte[] Payload { get; init; }
}