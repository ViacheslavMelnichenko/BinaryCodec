using System;
using System.Collections.Generic;
using System.Linq;
using MessageCodec.Contracts;

namespace MessageCodec.Validation;

public sealed class DefaultMessageValidator
{
    private const int BytesPerKilobyte = 1024;
    private const int MaxHeaderQuantity = 63;
    private const int MaxHeaderSizeInBytes = 1023;
    private const int MaxHeaderValueSizeInBytes = 1023;
    private const int MaxPayloadSizeInKilobytes = 256;
    private const int MaxPayloadSizeInBytes = MaxPayloadSizeInKilobytes * BytesPerKilobyte;

    public void ValidateEncoding(string data)
    {
        if (data.Any(c => c > 127))
            throw new InvalidOperationException($"String contains non-ASCII characters. String: {data}");
    }

    public void ValidateHeadersCount(int count)
    {
        if (count > MaxHeaderQuantity)
        {
            throw new InvalidOperationException($"Headers quantity cannot exceed {MaxHeaderQuantity}.");
        }
    }

    public void ValidateHeaderSize(int headerSize)
    {
        if (headerSize > MaxHeaderSizeInBytes)
        {
            throw new InvalidOperationException($"Header name cannot exceed {MaxHeaderSizeInBytes} bytes.");
        }
    }

    public void ValidateHeaderValueSize(int headerValueSize)
    {
        if (headerValueSize > MaxHeaderValueSizeInBytes)
        {
            throw new InvalidOperationException($"Header value cannot exceed {MaxHeaderValueSizeInBytes} bytes.");
        }
    }

    public void ValidateHeaders(Dictionary<string, string> headers)
    {
        foreach (var header in headers)
        {
            ValidateEncoding(header.Key);
            ValidateEncoding(header.Value);
            ValidateHeaderSize(BinaryMessageCodec.Encoding.GetByteCount(header.Key));
            ValidateHeaderValueSize(BinaryMessageCodec.Encoding.GetByteCount(header.Value));
        }
    }

    public void ValidatePayloadSize(int size)
    {
        if (size > MaxPayloadSizeInBytes)
        {
            throw new InvalidOperationException($"Payload cannot exceed {MaxPayloadSizeInBytes} bytes.");
        }
    }

    public void ValidateMessage(IMessage message)
    {
        ValidateHeadersCount(message.Headers.Count);
        ValidateHeaders(message.Headers);
        ValidatePayloadSize(message.Payload.Length);
    }
}