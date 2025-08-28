using MotosRental.Entities;

namespace MotosRental.Interfaces;

public interface IRentRepository 
{
    Task CreateAsync(Rent rent);
    Task<Rent?> GetByIdAsync(long id);
    Task<bool> IsMotorcycleRentedAsync(long motorcycleId);
    void RemoveAsync(Rent rent);
    Task SaveChangesAsync();
}