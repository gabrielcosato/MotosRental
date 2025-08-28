namespace MotosRental.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchangeName, string routingKey, T message);
}