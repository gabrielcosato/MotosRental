using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MotosRental.DTOs;

public class MotorcycleRequestDTO
{
    [Required(ErrorMessage = "A placa não pode estar em branco.")]
    [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres.")]
    [SwaggerSchema(Description = "Placa da moto")]
    public string LicensePlate { get; set; }
    
    [StringLength(30, ErrorMessage = "A cor deve ter no máximo 30 caracteres.")]
    [SwaggerSchema(Description = "Cor da moto")]
    public string? Color { get; set; }
    
    [Required(ErrorMessage = "O modelo não pode estar em branco.")]
    [SwaggerSchema(Description = "Modelo da moto")]
    public string Model { get; set; }
    
    [Required(ErrorMessage = "O ano não pode estar em branco.")]
    [SwaggerSchema(Description = "Ano de fabricação")]
    public int Year { get; set; } 
    
}