using System.Text;
using System.Text.Json;
using MotosRental.Interfaces;
using RabbitMQ.Client;

namespace MotosRental.Messaging;

public class RabbitMQPublisher : IMessagePublisher
{
    private readonly RabbitMQConnection _connection;

    public RabbitMQPublisher(RabbitMQConnection connection)
    {
        _connection = connection;
    }

    public Task PublishAsync<T>(string exchangeName, string routingKey, T message)
    {
        var channel = _connection.GetChannel();

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: body);

        return Task.CompletedTask;
    }
}