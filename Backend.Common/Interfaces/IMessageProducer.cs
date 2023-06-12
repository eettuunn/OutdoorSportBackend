namespace Backend.Common.Interfaces;

public interface IMessageProducer
{
    public void SendMessage<T>(T message);
}