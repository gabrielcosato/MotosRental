using Microsoft.EntityFrameworkCore;
using MotosRental.Entities;
using MotosRental.Interfaces;

namespace MotosRental.Repositories;

public class RentRepository : IRentRepository
{
    private readonly AppDbContext _context;

    public RentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Rent rent)
    {
        await _context.Rents.AddAsync(rent);
    }

    public async Task<Rent?> GetByIdAsync(long id)
    {
        return await _context.Rents
            .Include(r => r.Plan)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> IsMotorcycleRentedAsync(long motorcycleId)
    {
        return await _context.Rents
            .AnyAsync(r => r.MotorcycleId == motorcycleId && r.EndDate == null);
    }
    
    public void RemoveAsync(Rent rent)
    {
        _context.Rents.Remove(rent);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}