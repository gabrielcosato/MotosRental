using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotosRental.Entities;

[Table("ocupated_dates")]
public class MotoOcupatedDate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Column("ocupated_date")]
    public DateOnly OcupatedDate { get; set; }

    [ForeignKey(nameof(Motorcycle))]
    [Column("motorcycle_id")]
    public long MotorcycleId { get; set; }

    public virtual Motorcycle Motorcycle { get; set; } = null!;
}