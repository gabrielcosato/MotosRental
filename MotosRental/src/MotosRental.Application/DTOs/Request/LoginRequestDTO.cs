using System.ComponentModel.DataAnnotations;

namespace MotosRental.DTOs;

public class LoginRequestDTO
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}