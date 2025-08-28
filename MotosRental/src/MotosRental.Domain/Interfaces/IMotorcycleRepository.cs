using MotosRental.DTOs;
using MotosRental.Entities;

namespace MotosRental.Interfaces;

public interface IMotorcycleRepository
{
    Task CreateAsync(Motorcycle motorcycle);
    Task<Motorcycle?> FindByIdAsync(long id);
    
    Task<List<Motorcycle>> FindAllAsync();

    Task<Motorcycle> FindByPlateAsync(string plate);
    Task<bool> LicensePlateExistsAsync(string plate);
    void RemoveAsync(Motorcycle motorcycle);
    Task SaveChangesAsync();
}