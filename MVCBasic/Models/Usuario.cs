using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCBasic.Models
{
    public abstract class Usuario
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            [Required]
            public string Nombre { get; set; }
            [Required]
            public string Apellido { get; set; }
            [Required]
            public string password { get; set; }
            [StringLength(11, MinimumLength = 11, ErrorMessage = "El número de teléfono debe tener 11 dígitos")]
            [RegularExpression("^[0-9]*$", ErrorMessage = "El número de teléfono debe contener solo números")]
            public string Telefono { get; set; }
    }
}
