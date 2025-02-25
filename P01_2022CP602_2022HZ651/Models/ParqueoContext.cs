using Microsoft.EntityFrameworkCore;

namespace P01_2022CP602_2022HZ651.Models
{
    public class ParqueoContext : DbContext
    {
        public ParqueoContext(DbContextOptions<ParqueoContext> options) : base(options)
        {

        }
        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Sucursales> sucursales { get; set; }
        public DbSet<Reservas> reservas { get; set; }
        public DbSet<EspaciosParqueo> EspaciosParqueo { get; set; }
    }
}
