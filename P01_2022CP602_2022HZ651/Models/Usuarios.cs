using System.ComponentModel.DataAnnotations;

namespace P01_2022CP602_2022HZ651.Models
{
    public class Usuarios
    {
        [Key]
        public int Id_usuario { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]

        public string Correo { get; set; }
        [Required]

        public string Telefono { get; set; }
        [Required]

        public string Contrasena { get; set; }
        [Required]
        [RegularExpression("^(Empleado|Usuario)$", ErrorMessage = "El rol debe ser 'Empleado' o 'Usuario'.")]
        public string Rol { get; set; }
    }
}
