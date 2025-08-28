using Swashbuckle.AspNetCore.Filters;

namespace MotosRental.DTOs.Examples;

public class RentRequestExample : IExamplesProvider<RentRequestDTO>
{
    
    public RentRequestDTO GetExamples()
    {
        return new RentRequestDTO()
        {
            DriverId = 1,
            MotorcycleId = 1,
            PlanDays = 7,
        };
    }
}