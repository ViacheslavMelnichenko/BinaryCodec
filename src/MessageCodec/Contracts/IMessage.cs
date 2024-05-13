using System.Collections.Generic;

namespace MessageCodec.Contracts;

public interface IMessage
{
    Dictionary<string, string> Headers { get; init; }
    byte[] Payload { get; init; }
}