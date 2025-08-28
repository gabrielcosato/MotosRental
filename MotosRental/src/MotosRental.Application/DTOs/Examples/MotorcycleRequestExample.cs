using Swashbuckle.AspNetCore.Filters;

namespace MotosRental.DTOs.Examples;

public class MotorcycleRequestExample : IExamplesProvider<MotorcycleRequestDTO>
{
    public MotorcycleRequestDTO GetExamples()
    {
        return new MotorcycleRequestDTO
        {
            LicensePlate = "LBJ-4279",
            Color = "Vermelha",
            Model = "Honda CG 160",
            Year = 2022
        };
    }
}