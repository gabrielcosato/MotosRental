using Microsoft.EntityFrameworkCore;
using MotosRental.Entities;
using MotosRental.Interfaces;

namespace MotosRental.Repositories;

public class PlanRepository : IPlanRepository
{
    private readonly AppDbContext _context;

    public PlanRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<RentalPlan?> FindByDaysAsync(int id)
    {
        return await _context.RentalPlans.FindAsync(id);
    }
}