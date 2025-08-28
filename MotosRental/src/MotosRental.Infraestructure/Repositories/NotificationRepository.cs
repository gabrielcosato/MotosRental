using MongoDB.Driver;
using MotosRental.Entities;
using MotosRental.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MotosRental.Infraestructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly IMongoCollection<Notification> _notifications;
    private readonly IConfiguration _configuration;

    public NotificationRepository(IConfiguration configuration)
    {
        _configuration = configuration; 
        
        var connectionString = _configuration["MongoDb:ConnectionString"];
        var databaseName = _configuration["MongoDb:DatabaseName"];

        Console.WriteLine($"[NotificationRepository] Tentando conectar ao MongoDB com ConnectionString: {connectionString}, DatabaseName: {databaseName}");

        try
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _notifications = database.GetCollection<Notification>("notifications");
            Console.WriteLine("[NotificationRepository] Conexão com MongoDB e coleção 'notifications' estabelecida com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificationRepository] ERRO na CONEXÃO/COLEÇÃO MongoDB: {ex.Message}");
            
        }
    }

    public async Task AddAsync(Notification notification)
    {
        try
        {
            Console.WriteLine($"[NotificationRepository] Tentando adicionar notificação para placa: {notification.LicensePlate}");
            _notifications.InsertOneAsync(notification);
            Console.WriteLine($"[NotificationRepository] Notificação para placa {notification.LicensePlate} SALVA com sucesso no MongoDB.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificationRepository] ERRO ao adicionar notificação para placa {notification.LicensePlate}: {ex.Message}");
            
        }
    }
}
