using MotosRental.Entities;

namespace MotosRental.DTOs;

public class MotorcycleResponseDTO
{
    
    public long Id { get; set; }

    
    public string LicensePlate { get; set; }

    
    public string? Color { get; set; }
    

    public string Model { get; set; }
    

    public int Year { get; set; } 
    

}