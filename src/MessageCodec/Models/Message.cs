using System.Collections.Generic;
using MessageCodec.Contracts;

namespace MessageCodec.Models;

public sealed record Message(Dictionary<string, string> Headers, byte[] Payload) : IMessage;