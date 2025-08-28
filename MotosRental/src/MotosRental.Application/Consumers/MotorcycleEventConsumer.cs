

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection; 
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using MotosRental.Interfaces;
using MotosRental.Entities;
using MotosRental.MotosRental.Application.Events;

namespace MotosRental.Application.Consumers;

public class MotorcycleEventConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MotorcycleEventConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMQ:Hostname"],
            UserName = configuration["RabbitMQ:Username"],
            Password = configuration["RabbitMQ:Password"],
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "motorcycle.events", type: ExchangeType.Topic, durable: true);
        _channel.QueueDeclare(queue: "motorcycle.notification.queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: "motorcycle.notification.queue", exchange: "motorcycle.events", routingKey: "motorcycle.created");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[MotorcycleEventConsumer] JSON recebido: {message}");
            var motorcycleEvent = JsonSerializer.Deserialize<MotorcycleCreatedEvent>(message);

            if (motorcycleEvent != null && motorcycleEvent.Year == 2024)
            {
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                    var notification = new Notification
                    {
                        MotorcycleId = motorcycleEvent.Id,
                        LicensePlate = motorcycleEvent.LicensePlate,
                        Year = motorcycleEvent.Year,
                        Message = $"Moto {motorcycleEvent.LicensePlate} cadastrada (Ano {motorcycleEvent.Year})",
                        ReceivedAt = DateTime.UtcNow
                    };

                    await notificationRepository.AddAsync(notification);
                }
            }
            
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: "motorcycle.notification.queue", autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
