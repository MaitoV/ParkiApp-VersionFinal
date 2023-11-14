using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCBasic.Models
{
    public class Vehiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(7, MinimumLength = 6, ErrorMessage = "La patente debe tener entre 6 y 7 caracteres.")]
        public string Patente { get; set; }

        [Required]
        [EnumDataType(typeof(TipoVehiculo))]
        public TipoVehiculo Tipo { get; set; }

    }
}
