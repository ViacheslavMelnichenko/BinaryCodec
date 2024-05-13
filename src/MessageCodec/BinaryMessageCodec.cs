using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MessageCodec.Contracts;
using MessageCodec.Models;
using MessageCodec.Validation;

namespace MessageCodec;

public sealed class BinaryMessageCodec : IMessageCodec
{
    internal static readonly Encoding Encoding = Encoding.ASCII;
    private static readonly DefaultMessageValidator Validator = new();

    public IMessage Decode(byte[] data)
    {
        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream, Encoding);

        int headersCount = reader.ReadByte();
        Validator.ValidateHeadersCount(headersCount);

        var headers = new Dictionary<string, string>();
        for (var i = 0; i < headersCount; i++)
        {
            var header = ReadHeader(reader);
            headers.Add(header.Key, header.Value);
        }

        var payload = ReadPayload(reader);
        return new Message(headers, payload);
    }

    public byte[] Encode(IMessage message)
    {
        Validator.ValidateMessage(message);
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream, Encoding);

        WriteHeaders(writer, message.Headers);
        WritePayload(writer, message.Payload);

        return stream.ToArray();
    }

    private static byte[] ConvertUShortToBytes(ushort value)
    {
        return BitConverter.GetBytes(value);
    }

    private KeyValuePair<string, string> ReadHeader(BinaryReader reader)
    {
        var nameLength = reader.ReadUInt16();
        Validator.ValidateHeaderSize(nameLength);
        var name = Encoding.GetString(reader.ReadBytes(nameLength));

        var valueLength = reader.ReadUInt16();
        Validator.ValidateHeaderValueSize(valueLength);
        var value = Encoding.GetString(reader.ReadBytes(valueLength));

        return new KeyValuePair<string, string>(name, value);
    }

    private byte[] ReadPayload(BinaryReader reader)
    {
        var payloadLength = reader.ReadInt32();
        Validator.ValidatePayloadSize(payloadLength);
        return reader.ReadBytes(payloadLength);
    }

    private void WriteHeaders(BinaryWriter writer, Dictionary<string, string> headers)
    {
        writer.Write((byte) headers.Count);
        foreach (var (key, value) in headers)
        {
            var nameBytes = Encoding.GetBytes(key);
            var valueBytes = Encoding.GetBytes(value);

            writer.Write(ConvertUShortToBytes((ushort) nameBytes.Length));
            writer.Write(nameBytes);

            writer.Write(ConvertUShortToBytes((ushort) valueBytes.Length));
            writer.Write(valueBytes);
        }
    }

    private static void WritePayload(BinaryWriter writer, byte[] payload)
    {
        writer.Write((uint) payload.Length);
        writer.Write(payload);
    }
}