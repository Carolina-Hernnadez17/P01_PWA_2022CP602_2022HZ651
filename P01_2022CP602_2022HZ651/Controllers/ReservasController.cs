using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022CP602_2022HZ651.Models;

namespace P01_2022CP602_2022HZ651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ParqueoContext _ParqueoContexto;

        public ReservasController(ParqueoContext reservaContexto)
        {
            _ParqueoContexto = reservaContexto;
        }

        /// <summary>
        /// EndPoint Permitir a un usuario autenticado reservar un espacio disponible, indicando fecha, hora y cantidad de horas a reservar.
        /// </summary>
        /// <returns></returns>

        [HttpPost("ReservarEspacio")]
        public IActionResult ReservarEspacio(string correo, int idEspacio, DateTime fecha, TimeSpan horaInicio, int cantidadHoras)
        {
            //Usuario auntenticado
            var usuarioExistente = _ParqueoContexto.usuarios
                .Where(u => u.Correo == correo)
                .FirstOrDefault();

            if (usuarioExistente == null)
            {
                return NotFound("El correo proporcionado no existe.");
            }
            else
            {
                // Espacio disponible
                var espacioDisponible = _ParqueoContexto.EspaciosParqueo
                    .Where(ep => ep.Id_espacioparqueo == idEspacio && ep.Estado == "Disponible")
                    .FirstOrDefault();

                if (espacioDisponible == null)
                {
                    return BadRequest("El espacio no está disponible.");
                }
                else
                {
                    // Espacio disponible en la sucursal
                    var sucursal = _ParqueoContexto.sucursales
                        .Where(s => s.Id_sucursal == espacioDisponible.Id_sucursal && s.EspaciosDisponibles > 0)
                        .FirstOrDefault();

                    if (sucursal == null)
                    {
                        return BadRequest("No hay espacios disponibles en la sucursal.");
                    }
                    else 
                    {
                        var reserva = new Reservas
                        {
                            Id_usuario = usuarioExistente.Id_usuario,
                            Id_espacioparqueo = idEspacio,
                            Fecha = fecha,
                            HoraInicio = horaInicio,
                            CantidadHoras = cantidadHoras,
                            Estado = "Confirmada"
                        };

                        _ParqueoContexto.reservas.Add(reserva);
                        _ParqueoContexto.SaveChanges();

                        espacioDisponible.Estado = "Ocupado";
                        _ParqueoContexto.SaveChanges();

                        sucursal.EspaciosDisponibles--;
                        _ParqueoContexto.SaveChanges();

                        return Ok("Reserva realizada con éxito.");

                    }


                }

                

            }
            
        }
        /// <summary>
        /// EndPoint Mostrar una lista de reservas activas del usuario.
        /// </summary>
        /// <returns></returns>

        [HttpGet("ReservasActivas/{idUsuario}")]
        public IActionResult ObtenerReservasActivas(int idUsuario)
        {
            var reservasActivas = (from reserva in _ParqueoContexto.reservas
                                   join usuario in _ParqueoContexto.usuarios on reserva.Id_usuario equals usuario.Id_usuario
                                   join espacio in _ParqueoContexto.EspaciosParqueo on reserva.Id_espacioparqueo equals espacio.Id_espacioparqueo
                                   where reserva.Id_usuario == idUsuario && reserva.Estado == "Activa"
                                   select new
                                   {
                                       reserva.Id_reservas,
                                       reserva.Fecha,
                                       reserva.HoraInicio,
                                       reserva.CantidadHoras,
                                       reserva.Estado,
                                       UsuarioNombre = usuario.Nombre,
                                       UsuarioCorreo = usuario.Correo,
                                       UsuarioTelefono = usuario.Telefono,
                                       EspacioNumero = espacio.Numero,
                                       EspacioUbicacion = espacio.Ubicacion,
                                       EspacioCostoPorHora = espacio.CostoPorHora,
                                       EspacioEstado = espacio.Estado
                                   }).ToList();

            if (reservasActivas == null || reservasActivas.Count == 0)
            {
                return NotFound("No se encontraron reservas con ese id");
            }

            return Ok(reservasActivas);
        }
        [HttpGet]
        [Route("getBycliente/{nombreCliente}")]
        public IActionResult GetByCliente(string nombreCliente)
        {
            var resultado = (from reserva in _ParqueoContexto.reservas
                             join usuario in _ParqueoContexto.usuarios on reserva.Id_usuario equals usuario.Id_usuario
                             join espacio in _ParqueoContexto.EspaciosParqueo on reserva.Id_espacioparqueo equals espacio.Id_espacioparqueo
                             where usuario.Nombre == nombreCliente && reserva.Estado == "Activa"
                             select new
                             {
                                 reserva.Id_reservas,
                                 reserva.Fecha,
                                 reserva.HoraInicio,
                                 reserva.CantidadHoras,
                                 reserva.Estado,
                                 UsuarioNombre = usuario.Nombre,
                                 UsuarioCorreo = usuario.Correo,
                                 UsuarioTelefono = usuario.Telefono,
                                 EspacioNumero = espacio.Numero,
                                 EspacioUbicacion = espacio.Ubicacion,
                                 EspacioCostoPorHora = espacio.CostoPorHora,
                                 EspacioEstado = espacio.Estado
                             }).ToList();

            if (!resultado.Any())
            {
                return NotFound();
            }

            return Ok(resultado);
        }


    }
}
