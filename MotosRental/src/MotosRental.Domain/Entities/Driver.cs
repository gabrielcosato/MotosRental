using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MotosRental.Enums;

namespace MotosRental.Entities;

[Table("drivers")]
public class Driver
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; } 

    [Required]
    [Column("cnpj")]
    [StringLength(14)]
    public string Cnpj { get; set; }
    
    [Required]
    [Column("email")]
    public string Email { get; set; }

    [Required]
    [Column("birth_date", TypeName = "date")]
    public DateOnly BirthDate { get; set; }

    [Required]
    [Column("driver_license_number")]
    public string DriverLicenseNumber { get; set; } 

    [Required]
    [Column("driver_license_type")]
    public LicenseType DriverLicenseType { get; set; }

    
    [Column("driver_license_image")]
    public string? DriverLicenseImage { get; set; } 
    

    public Driver()
    {
        
    }
    
    
}