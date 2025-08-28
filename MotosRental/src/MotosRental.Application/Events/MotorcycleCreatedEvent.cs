namespace MotosRental.MotosRental.Application.Events;

public class MotorcycleCreatedEvent
{
    public long Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Model { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}