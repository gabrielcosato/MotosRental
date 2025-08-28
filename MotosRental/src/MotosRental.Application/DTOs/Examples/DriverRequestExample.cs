using MotosRental.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace MotosRental.DTOs.Examples;

public class DriverRequestExample : IExamplesProvider<DriverRequestDTO>
{

    public DriverRequestDTO GetExamples()
    {
        return new DriverRequestDTO
        {
            Name = "Fulano 1",
            Cnpj = "77026049000148",
            Email = "email@email.com",
            BirthDate = DateOnly.Parse("2002-12-20"),
            DriverLicenseNumber = "93944745208",
            DriverLicenseType = LicenseType.A
        };
    }
}