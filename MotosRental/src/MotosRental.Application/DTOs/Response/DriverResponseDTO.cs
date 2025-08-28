using MotosRental.Enums;

namespace MotosRental.DTOs;

public class DriverResponseDTO
{
    public long Id { get; set; }
    

    public string Name { get; set; } 


    public string Cnpj { get; set; }
    

    public string Email { get; set; }


    public DateOnly BirthDate { get; set; }

    
    public string DriverLicenseNumber { get; set; } 


    public LicenseType DriverLicenseType { get; set; }

  
    public string DriverLicenseImage { get; set; }


}