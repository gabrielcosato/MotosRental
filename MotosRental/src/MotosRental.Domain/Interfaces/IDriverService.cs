using MotosRental.DTOs;
using MotosRental.Entities;

namespace MotosRental.Interfaces;

public interface IDriverService
{
    Task<DriverResponseDTO> CreateAsync(DriverRequestDTO driverDto);
    Task<List<DriverResponseDTO>> FindAllAsync();
    Task<DriverResponseDTO> FindByIdAsync(long id);
    Task<DriverResponseDTO> UploadDriverLicenseAsync(long id, IFormFile imageFile);
    Task RemoveAsync(long id);
}