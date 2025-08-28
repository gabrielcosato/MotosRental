using System.ComponentModel.DataAnnotations;

namespace MotosRental.DTOs;

public class DriverUpdateImageRequestDTO
{
    [Required(ErrorMessage = "O Arquivo da imagem não pode estar em branco.")]
    public IFormFile DriverLicenseImage { get; set; } 
}