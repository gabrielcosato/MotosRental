using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MotosRental.Entities;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public long MotorcycleId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime ReceivedAt { get; set; }
}