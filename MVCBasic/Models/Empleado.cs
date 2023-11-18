using System.ComponentModel.DataAnnotations;

namespace MVCBasic.Models
{
    public class Empleado : Usuario
    {
        [Required]
        public int NumeroLegajo { get; set; }
        [Required]
        [EnumDataType(typeof(Turno))]
        public string Turno { get; set; } 
    }
}
