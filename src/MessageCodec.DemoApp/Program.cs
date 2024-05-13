using System.Diagnostics.CodeAnalysis;
using MessageCodec;
using MessageCodec.Contracts;
using MessageCodec.Models;

Console.WriteLine("Demo App started");

IMessage originalMessage = new Message(
    new Dictionary<string, string>
    {
        {
            "Fruit", "Apple"
        },
        {
            "Vegetable", "Tomato"
        }
    },
    "Fruits and vegetables are excellent sources of vitamins."u8.ToArray()
);

var сodec = new BinaryMessageCodec();

var encoded = сodec.Encode(originalMessage);

var decodedMessage = сodec.Decode(encoded);

Console.WriteLine("Decoded message info:");
foreach (var header in decodedMessage.Headers)
{
    Console.WriteLine($"Header: {header.Key}");
    Console.WriteLine($"Value: {header.Value}");
}

Console.WriteLine($"Payload size is: {decodedMessage.Payload.Length} bytes");

Console.WriteLine("Demo App stopped");

[ExcludeFromCodeCoverage]
public partial class Program;