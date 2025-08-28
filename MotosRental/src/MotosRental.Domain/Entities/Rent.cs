using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotosRental.Entities;

[Table("rents")]
public class Rent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("driver_id")]
    public long DriverId { get; set; }
    public virtual Driver Driver { get; set; }

    [Required]
    [Column("motorcycle_id")]
    public long MotorcycleId { get; set; }
    public virtual Motorcycle Motorcycle { get; set; }

    [Required]
    [Column("plan_days")]
    public int PlanDays { get; set; }
    public virtual RentalPlan Plan { get; set; }

    [Required]
    [Column("start_date" , TypeName = "date")]
    public DateOnly StartDate{ get; set; }

    [Required]
    [Column("predicted_end_date")]
    public DateOnly PredictedEndDate { get; set; }

    [Column("end_date")]
    public DateOnly? EndDate { get; set; } 

    [Required]
    [Column("total_cost")]
    public decimal TotalCost { get; set; }

    public Rent()
    {
        
    }
    
}