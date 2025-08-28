using MotosRental.DTOs;

namespace MotosRental.Interfaces;

public interface IRentService
{
    Task<RentResponseDTO> CreateAsync(RentRequestDTO dto);
    Task<RentResponseDTO> GetByIdAsync(long id);
    Task<RentResponseDTO> ProcessReturnAsync(long id, ReturnRentRequestDTO dto);
    
    Task RemoveAsync(long id);
}