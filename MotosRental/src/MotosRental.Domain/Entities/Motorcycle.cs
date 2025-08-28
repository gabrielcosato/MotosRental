using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotosRental.Entities;

[Table("motorcycles")]
public class Motorcycle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    [Column("id")]
    public long Id { get; set; }


    [Required]
    [Column("license_plate", TypeName = "varchar(20)")] 
    public string LicensePlate { get; set; }

    
    [Column("color", TypeName = "varchar(30)")]
    public string? Color { get; set; }
    
    [Required]
    [Column("model", TypeName = "varchar(200)")]
    public string Model { get; set; }
    
    [Required]
    [Column("year")]
    public int Year { get; set; } 
    
    
    public virtual ICollection<MotoOcupatedDate> ListOcupatedDates { get; set; } = new List<MotoOcupatedDate>();


    public Motorcycle()
    {
        
    }
    

    public bool IsAvailableToRent(DateOnly startDate, DateOnly returnDate)
    {
        foreach (var d in ListOcupatedDates)
        {
            if (d.OcupatedDate >= startDate && d.OcupatedDate <= returnDate)
                return false;
        }
        return true;
    }
    
    public void BlockDates(DateOnly startDate, DateOnly returnDate)
    {
        var date = startDate;
        while (date <= returnDate)
        {

            if (!ListOcupatedDates.Any(x => x.OcupatedDate == date))
            {
                ListOcupatedDates.Add(new MotoOcupatedDate() { MotorcycleId = Id, OcupatedDate = date });
            }
                
            date = date.AddDays(1);
        }
    }
}