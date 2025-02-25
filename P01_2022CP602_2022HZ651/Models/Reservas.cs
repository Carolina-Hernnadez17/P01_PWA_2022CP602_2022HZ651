using System.ComponentModel.DataAnnotations;

namespace P01_2022CP602_2022HZ651.Models
{
    public class Reservas
    {
        [Key]
        public int Id_reservas { get; set; }

        public int Id_usuario { get; set; }

        public int Id_espacioparqueo { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public int CantidadHoras { get; set; }

        public string Estado { get; set; }
    }
}
