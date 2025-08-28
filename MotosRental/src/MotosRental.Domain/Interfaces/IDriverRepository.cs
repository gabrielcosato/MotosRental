using MotosRental.Entities;

namespace MotosRental.Interfaces;

public interface IDriverRepository
{
    Task CreateAsync(Driver driver);
    Task<Driver?> FindByIdAsync(long id);
    Task<List<Driver>> FindAllAsync();
    Task<bool> CnpjExistsAsync(string cnpj); 
    Task<bool> EmailExistsAsync(string email);
    Task<bool> DriverLicenseNumberExistsAsync(string licenseNumber);
    Task<Driver> UploadDriverLicenseAsync(Driver driver); 
    void RemoveAsync(Driver driver);
    Task SaveChangesAsync();
}