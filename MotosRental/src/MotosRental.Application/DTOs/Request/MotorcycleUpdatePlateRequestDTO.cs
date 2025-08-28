using System.ComponentModel.DataAnnotations;

namespace MotosRental.DTOs;

public class MotorcycleUpdatePlateRequestDTO
{
    [Required(ErrorMessage = "A placa não pode estar em branco.")]
    [StringLength(8, ErrorMessage = "A placa deve ter no máximo 8 caracteres.")]
    public string LicensePlate { get; set; }
}