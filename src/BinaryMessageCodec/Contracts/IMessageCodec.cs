namespace BinaryMessageCodec.Contracts;

public interface IMessageCodec
{
    byte[] Encode(IMessage message);
    IMessage Decode(byte[] data);
}