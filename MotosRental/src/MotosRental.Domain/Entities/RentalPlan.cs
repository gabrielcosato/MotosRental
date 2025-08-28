using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotosRental.Entities;

[Table("rental_plans")]
public class RentalPlan
{
    [Key]
    public int Days { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyRate { get; set; }

    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal EarlyReturnPenaltyRate { get; set; } 
}