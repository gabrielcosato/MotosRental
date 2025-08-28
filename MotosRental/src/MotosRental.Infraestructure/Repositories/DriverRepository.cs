using Microsoft.EntityFrameworkCore;
using MotosRental.Entities;
using MotosRental.Exceptions;
using MotosRental.Interfaces;

namespace MotosRental.Repositories;

public class DriverRepository : IDriverRepository
{
    private readonly AppDbContext _context;

    public DriverRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Driver driver)
    {
        await _context.Drivers.AddAsync(driver);
    }

    public async Task<Driver?> FindByIdAsync(long id)
    {
        return await _context.Drivers.FindAsync(id);
    }

    public async Task<List<Driver>> FindAllAsync()
    {
        return await _context.Drivers.ToListAsync();
    }

    public async Task<bool> CnpjExistsAsync(string cnpj)
    {
        return await _context.Drivers.AnyAsync(d => d.Cnpj == cnpj);
    }
    
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Drivers.AnyAsync(d => d.Email == email);
    }
    
    public async Task<bool> DriverLicenseNumberExistsAsync(string driverLicenseNumber)
    {
        return await _context.Drivers.AnyAsync(d => d.DriverLicenseNumber == driverLicenseNumber);
    }
    
    public async Task<Driver> UploadDriverLicenseAsync(Driver driver)
    {
         await _context.Drivers.AddAsync(driver);
         return driver;
    }
    
    public void RemoveAsync(Driver driver)
    {
        
        bool hasRents = _context.Rents.Any(r => r.DriverId == driver.Id);

        if (hasRents)
        {
            throw new BusinessValidationException("Não é possível excluir esse motorista, pois existem alugueis associados a ele.");
        }
        _context.Drivers.Remove(driver);
    }
    

    public Task UpdateAsync(Driver driver)
    {
        _context.Drivers.Update(driver);
        return Task.CompletedTask; 
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}