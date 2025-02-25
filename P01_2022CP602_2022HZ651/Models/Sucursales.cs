using System.ComponentModel.DataAnnotations;

namespace P01_2022CP602_2022HZ651.Models
{
    public class Sucursales
    {
        [Key]

        public int Id_sucursal { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Telefono { get; set; }

        public int Id_usuario { get; set; }
        [Required]
        public int EspaciosDisponibles { get; set; }
    }
}
