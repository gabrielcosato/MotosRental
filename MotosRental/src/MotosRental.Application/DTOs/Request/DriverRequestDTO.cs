using System.ComponentModel.DataAnnotations;
using MotosRental.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace MotosRental.DTOs;

public class DriverRequestDTO
{
    
    [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
    
    public string Name { get; set; } 

    [Required(ErrorMessage = "O Cnpj não pode estar em branco.")]
    [StringLength(14, ErrorMessage = "O Cnpj  deve ter no máximo 14 caracteres.")]
    public string Cnpj { get; set; }

    [Required(ErrorMessage = "O Email não pode estar em branco.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "A data de nascimento não pode estar em branco.")]
    public DateOnly BirthDate { get; set; }
    
    [Required(ErrorMessage = "O número da CNH não pode estar em branco.")]
    [StringLength(11, ErrorMessage = "A CNH  deve ter no máximo 11 caracteres.")]
    public string DriverLicenseNumber { get; set; } 
    
    [Required(ErrorMessage = "O tipo da CNH não pode estar em branco.")]
    public LicenseType DriverLicenseType { get; set; }
    
}