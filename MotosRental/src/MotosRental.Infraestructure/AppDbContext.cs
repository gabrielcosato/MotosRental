using Microsoft.EntityFrameworkCore;
using MotosRental.Entities;

namespace MotosRental;

public class AppDbContext : DbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    
    public DbSet<Rent> Rents { get; set; } 
    public DbSet<RentalPlan> RentalPlans { get; set; } 

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Motorcycle>()
            .HasIndex(c => c.LicensePlate)
            .IsUnique();
        
        modelBuilder.Entity<Driver>()
            .HasIndex(c => c.Cnpj)
            .IsUnique();
        
        modelBuilder.Entity<Driver>()
            .HasIndex(c => c.Email)
            .IsUnique();
        
        modelBuilder.Entity<Driver>()
            .HasIndex(c => c.DriverLicenseNumber)
            .IsUnique();
        
        modelBuilder.Entity<Driver>()
            .Property(d => d.DriverLicenseType)
            .HasConversion<string>(); 
        
        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasOne(r => r.Driver)
                .WithMany() 
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict); 

            entity.HasOne(r => r.Motorcycle)
                .WithMany() 
                .HasForeignKey(r => r.MotorcycleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Plan)
                .WithMany()
                .HasForeignKey(r => r.PlanDays);
        });

        
        modelBuilder.Entity<RentalPlan>().HasData(
            new RentalPlan { Days = 7,  DailyRate = 30.00m, EarlyReturnPenaltyRate = 0.20m },
            new RentalPlan { Days = 15, DailyRate = 28.00m, EarlyReturnPenaltyRate = 0.40m },
            new RentalPlan { Days = 30, DailyRate = 22.00m, EarlyReturnPenaltyRate = 0.00m }, 
            new RentalPlan { Days = 45, DailyRate = 20.00m, EarlyReturnPenaltyRate = 0.00m },
            new RentalPlan { Days = 50, DailyRate = 18.00m, EarlyReturnPenaltyRate = 0.00m }
        );
    }
    
    

}