using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotosRental.DTOs;
using MotosRental.Entities;
using MotosRental.Enums;
using MotosRental.Exceptions;
using MotosRental.Interfaces;

namespace MotosRental.Services;

public class RentService : IRentService
{
    private readonly IRentRepository _rentRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IPlanRepository _planRepository;
    private readonly IMapper _mapper;

    public RentService(
        IRentRepository rentRepository,
        IDriverRepository driverRepository,
        IMotorcycleRepository motorcycleRepository,
        IPlanRepository planRepository,
        IMapper mapper)
    {
        _rentRepository = rentRepository;
        _driverRepository = driverRepository;
        _motorcycleRepository = motorcycleRepository;
        _planRepository = planRepository;
        _mapper = mapper;
    }

    public async Task<RentResponseDTO> CreateAsync(RentRequestDTO dto)
    {
        var driver = await _driverRepository.FindByIdAsync(dto.DriverId)
                     ?? throw new KeyNotFoundException("Motorista não encontrado para o ID informado.");

        if (driver.DriverLicenseType != LicenseType.A )
            throw new BusinessValidationException("Motorista não possui a CNH do tipo A para realizar alugueis.");

        var plan = await _planRepository.FindByDaysAsync(dto.PlanDays)
                   ?? throw new KeyNotFoundException("O plano selecionado não existe. (escolha 7, 15, 30, 45 ou 50)");

        var motorcycle = await _motorcycleRepository.FindByIdAsync(dto.MotorcycleId)
                         ?? throw new KeyNotFoundException("Moto não encontrada para o ID informado.");
        
        
        if (await _rentRepository.IsMotorcycleRentedAsync(dto.MotorcycleId))
        {
            throw new BusinessValidationException("A moto selecionada já está alugada no momento.");
        }
        
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
        var predictedEndDate = startDate.AddDays(plan.Days - 1);

        var rent = new Rent
        {
            DriverId = driver.Id,
            MotorcycleId = dto.MotorcycleId,
            PlanDays = plan.Days,
            StartDate = startDate,
            PredictedEndDate = predictedEndDate,
            EndDate = null,
            TotalCost = plan.Days * plan.DailyRate
        };

        await _rentRepository.CreateAsync(rent);
        await _rentRepository.SaveChangesAsync();

        return MapToResponseDTO(rent, plan.DailyRate);
    }


    public async Task<RentResponseDTO> GetByIdAsync(long id)
    {
        var rent = await _rentRepository.GetByIdAsync(id)
                   ?? throw new KeyNotFoundException("Aluguel não encontrado para o ID informado.");
        
        return MapToResponseDTO(rent, rent.Plan.DailyRate);
    }

    public async Task<RentResponseDTO> ProcessReturnAsync(long id, ReturnRentRequestDTO dto)
    {
        var rent = await _rentRepository.GetByIdAsync(id)
                   ?? throw new KeyNotFoundException("Aluguel não encontrado para o ID informado.");

        if (rent.EndDate.HasValue)
            throw new BusinessValidationException("O Aluguel selecionado já foi finalizado.");

        if (dto.ReturnDate < rent.StartDate)
        {
            throw new BusinessValidationException("Data de devolução inválida. Precisa ser uma data posterior a data de início.");
        }
        rent.EndDate = dto.ReturnDate;
        rent.TotalCost = CalculateFinalCost(rent, dto.ReturnDate);

        await _rentRepository.SaveChangesAsync();
        return MapToResponseDTO(rent, rent.Plan.DailyRate);
    }
    
    public async Task RemoveAsync(long id)
    {
        var rent = await _rentRepository.GetByIdAsync(id)
                   ?? throw new KeyNotFoundException("Aluguel não encontrado para o ID informado.");
            
        _rentRepository.RemoveAsync(rent);
        await _rentRepository.SaveChangesAsync();
    }

    private decimal CalculateFinalCost(Rent rent, DateOnly returnDate)
    {
        int daysRented = (returnDate.DayNumber - rent.StartDate.DayNumber) + 1;

        if (returnDate < rent.PredictedEndDate)
        {

           
            int daysNotUsed = rent.PredictedEndDate.DayNumber - returnDate.DayNumber;
            decimal dailyRate = rent.Plan.DailyRate;
            
            
            decimal usedDaysCost = daysRented * dailyRate;
            
            if (rent.PlanDays == 7 || rent.PlanDays == 15)
            {
                decimal penaltyRate = rent.Plan.EarlyReturnPenaltyRate;
                decimal penaltyCost = daysNotUsed * dailyRate * penaltyRate;
                
                return usedDaysCost + penaltyCost;
            }
            
            
            return usedDaysCost;
        }
        else if (returnDate > rent.PredictedEndDate) 
        {
            int extraDays = returnDate.DayNumber - rent.PredictedEndDate.DayNumber;
            decimal planCost = rent.Plan.Days * rent.Plan.DailyRate;
            decimal penaltyCost = extraDays * 50.00m;

            return planCost + penaltyCost;
        }
        
       
        return rent.Plan.Days * rent.Plan.DailyRate;
    }

    private RentResponseDTO MapToResponseDTO(Rent rent, decimal dailyRate)
    {
        return new RentResponseDTO
        {
            Id = rent.Id,
            DriverId = rent.DriverId,
            MotorcycleId = rent.MotorcycleId,
            PlanDays = rent.PlanDays,
            DailyRate = dailyRate,
            StartDate = rent.StartDate,
            PredictedEndDate = rent.PredictedEndDate,
            EndDate = rent.EndDate,
            TotalCost = rent.TotalCost
        };
    }
}
