using System.Collections.Generic;
using BinaryMessageCodec.Contracts;

namespace BinaryMessageCodec.Models;

public sealed record Message(Dictionary<string, string> Headers, byte[] Payload) : IMessage;