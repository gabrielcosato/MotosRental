//using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text;


namespace MotosRental.Messaging;

public class RabbitMQConnection : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQConnection(string hostname, string username, string password)
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostname,
            UserName = username,
            Password = password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public IModel GetChannel() => _channel;

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}