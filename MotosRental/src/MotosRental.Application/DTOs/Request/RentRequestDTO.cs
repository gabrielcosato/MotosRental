using System.ComponentModel.DataAnnotations;

namespace MotosRental.DTOs;

public class RentRequestDTO
{
    [Required]
    public long DriverId { get; set; }

    [Required]
    public long MotorcycleId { get; set; } 

    [Required]
    public int PlanDays { get; set; } 
}