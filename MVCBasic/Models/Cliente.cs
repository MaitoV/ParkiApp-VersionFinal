using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCBasic.Models
{
    public class Cliente : Usuario
    {
        [Required]
        public string Email { get; set; }
        public List<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    }
}
