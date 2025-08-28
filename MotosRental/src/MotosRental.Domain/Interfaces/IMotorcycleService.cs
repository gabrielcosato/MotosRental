using MotosRental.DTOs;
using MotosRental.Entities;

namespace MotosRental.Interfaces;

public interface IMotorcycleService
{
    Task<MotorcycleResponseDTO> CreateAsync(MotorcycleRequestDTO dto);
    Task<List<MotorcycleResponseDTO>> FindAllAsync();
    Task<MotorcycleResponseDTO> FindByIdAsync(long id);
    Task<MotorcycleResponseDTO> FindByPlateAsync(string plate);
    Task<MotorcycleResponseDTO> UpdatePlateAsync(long id, MotorcycleUpdatePlateRequestDTO dto);
    Task RemoveAsync(long id);
}