using System.ComponentModel.DataAnnotations;

namespace P01_2022CP602_2022HZ651.Models
{
    public class EspaciosParqueo
    {
        [Key]

        public int Id_espacioparqueo { get; set; }
        [Required]
        public int Numero { get; set; }
        [Required]
        public string Ubicacion { get; set; }
        [Required]
        public decimal CostoPorHora { get; set; }
        [Required]
        public string Estado { get; set; } 
        public int Id_sucursal { get; set; }
    }
}
