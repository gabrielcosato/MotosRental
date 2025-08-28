using Microsoft.EntityFrameworkCore;
using MotosRental.DTOs;
using MotosRental.Entities;
using MotosRental.Exceptions;
using MotosRental.Interfaces;

namespace MotosRental.Repositories;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly AppDbContext _context;

    public MotorcycleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Motorcycle motorcycle)
    {
        await _context.Motorcycles.AddAsync(motorcycle);
    }

    public async Task<Motorcycle?> FindByIdAsync(long id)
    {
        return await _context.Motorcycles.FindAsync(id);
    }
    
    public async Task<List<Motorcycle>> FindAllAsync()
    {
        return await _context.Motorcycles.ToListAsync();
    }

    public async Task<Motorcycle> FindByPlateAsync(string plate)
    {

        if (string.IsNullOrWhiteSpace(plate))
        {
            return null;
        }
        
        return await _context.Motorcycles
            .FirstOrDefaultAsync(m => m.LicensePlate == plate);
    }

    public async Task<bool> LicensePlateExistsAsync(string plate)
    {
        return await _context.Motorcycles.AnyAsync(m => m.LicensePlate == plate);
    }

    public void RemoveAsync(Motorcycle motorcycle)
    {
        bool hasRents = _context.Rents.Any(r => r.MotorcycleId == motorcycle.Id);

        if (hasRents)
        {
            throw new BusinessValidationException("Não é possível excluir essa moto, pois existem alugueis associados a ela.");
        }

        _context.Motorcycles.Remove(motorcycle);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}