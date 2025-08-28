namespace MotosRental.DTOs;

public class RentResponseDTO
{
    public long Id { get; set; }
    public long DriverId { get; set; }
    public long MotorcycleId { get; set; }
    public int PlanDays { get; set; }
    public decimal DailyRate { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly PredictedEndDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal TotalCost { get; set; }
}