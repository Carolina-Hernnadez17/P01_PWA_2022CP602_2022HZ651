using System.ComponentModel.DataAnnotations;

namespace P01_2022CP602_2022HZ651.Models
{
    public class Usuarios
    {
        [Key]
        public int Id_usuario { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public string Contrasena { get; set; }

        public string Rol { get; set; }
    }
}
