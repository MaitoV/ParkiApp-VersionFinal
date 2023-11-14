using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCBasic.Models
{
    public class Cochera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int NumeroCochera { get; set; }
        [Required]
        [EnumDataType(typeof(Piso))]
        public Piso Piso { get; set; }
        public DateTime FechaIngreso { get; set; }

        [Required]
        [EnumDataType(typeof(TipoCochera))]
        public TipoCochera TipoCochera { get; set; }

        // Relación uno a uno con Vehiculo
        public int? VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; }

    }
}
