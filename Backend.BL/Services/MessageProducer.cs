using System.Text;
using System.Text.Json;
using Backend.Common.Interfaces;
using RabbitMQ.Client;

namespace Backend.BL.Services;

public class MessageProducer : IMessageProducer
{
    private readonly IConnection _connection;

    public MessageProducer(IConnection connection)
    {
        _connection = connection;
    }

    public void SendMessage<T>(T message)
    {
        using var channel = _connection.CreateModel();

        channel.QueueDeclare("reports", durable: true, exclusive: false);

        var jsonStr = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(jsonStr);
        
        channel.BasicPublish("", "reports", body: body);
    }
}