using System.ComponentModel.DataAnnotations;

namespace MotosRental.DTOs;

public class ReturnRentRequestDTO
{
    [Required]
    public DateOnly ReturnDate { get; set; } 
}