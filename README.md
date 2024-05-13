[![main](https://github.com/ViacheslavMelnichenko/BinaryCodec/actions/workflows/main.yml/badge.svg)](https://github.com/ViacheslavMelnichenko/BinaryCodec/actions/workflows/main.yml/badge.svg)
[![codecov](https://codecov.io/gh/ViacheslavMelnichenko/BinaryCodec/graph/badge.svg?token=HXY46DM74W)](https://codecov.io/gh/ViacheslavMelnichenko/BinaryCodec)

# BinaryMessageCodec

## Overview

> BinaryMessageCodec is a C# library designed
> to provide efficient binary message
> encoding and decoding functionalities.

## Features

- For now BinaryMessageCodec supports only ASCII encoding.
- Encode and decode messages containing multiple headers and a binary payload.
- Handle up to 63 headers and payloads up to 256 KiB.
- Optimized for low memory usage and high performance.

## Getting Started

### Prerequisites

- .NET SDK (Version 8.0 or later recommended)
- JetBrains Rider or any compatible .NET IDE

### Usage

> By default ``BinaryMessageCodec`` uses ``DefaultMessageValidator`` and ``ASCII`` encoding.

```csharp
Message originalMessage = new Message(
    new Dictionary<string, string>
    {
        {
            "Header1", "Value1"
        },
        {
            "Header2", "Value2"
        }
    },
    "Place here your payload byte array"u8.ToArray()
);
BinaryMessageCodec сodec = new BinaryMessageCodec();

byte[] encoded = сodec.Encode(originalMessage);

Message decodedMessage = сodec.Decode(encoded);
```

## Exception Handling

> BinaryMessageCodec employs a validation system to ensure
> that all components of a message adhere to specified constraints.
> Below are the exceptions that might be thrown during the encoding and decoding processes,
> along with descriptions and scenarios in which they could occur:

#### InvalidOperationException

This exception has several contexts where it might be thrown, detailed as follows:

#### String contains non-ASCII characters

- Thrown when: Any part of the header names or values contain characters that are not valid ASCII characters.
- Message: "String contains non-ASCII characters. String: [offending string]"
- How to handle: Ensure that all header names and values are composed of ASCII characters only (characters within the
  range of 0 to 127).

#### Header Count Exceeded

- Thrown when: The number of headers in a message exceeds the maximum allowed limit.
- Message: "Headers quantity cannot exceed 63."
- How to handle: Reduce the number of headers in your message to 63 or fewer.

#### Header Size Exceeded

- Thrown when: The size of any header name exceeds the maximum allowed bytes.
- Message: "Header name cannot exceed 1023 bytes."
- How to handle: Ensure that each header name is within 1023 bytes.

#### Header Value Size Exceeded

- Thrown when: The size of any header value exceeds the maximum allowed bytes.
- Message: "Header value cannot exceed 1023 bytes."
- How to handle: Ensure that each header value is within 1023 bytes.

#### Payload Size Exceeded

- Thrown when: The payload size exceeds the maximum allowed limit.
- Message: "Payload cannot exceed 262144 bytes."
- How to handle: Ensure that the payload is no larger than 262144 bytes (256 KiB).

## License

* [MIT License](https://github.com/ViacheslavMelnichenko/BinaryCodec/blob/main/LICENSE)

Contributors
---------
Created by [Viacheslav Melnichenko](https://github.com/ViacheslavMelnichenko).