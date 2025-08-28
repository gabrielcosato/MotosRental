using MotosRental.Entities;

namespace MotosRental.Interfaces;

public interface IPlanRepository
{
    Task<RentalPlan?> FindByDaysAsync(int id);
}