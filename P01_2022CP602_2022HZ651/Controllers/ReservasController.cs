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
                                   where usuario.Id_usuario == idUsuario && reserva.Estado == "Confirmada"
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
        // <summary>
        /// EndPoint para cancelar una reserva antes de su uso.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("CancelarReserva/{idReserva}")]
        public IActionResult CancelarReserva(int idReserva)
        {
            // Buscar la reserva por su ID
            var reserva = _ParqueoContexto.reservas
                .Where(r => r.Id_reservas == idReserva && r.Estado == "Confirmada")
                .FirstOrDefault();

            if (reserva == null)
            {
                return NotFound("Reserva no encontrada o ya no puede ser cancelada.");
            }

            reserva.Estado = "Cancelada";
            _ParqueoContexto.SaveChanges();

            var espacio = _ParqueoContexto.EspaciosParqueo
                .Where(ep => ep.Id_espacioparqueo == reserva.Id_espacioparqueo)
                .FirstOrDefault();

            if (espacio != null)
            {
                espacio.Estado = "Disponible";
                _ParqueoContexto.SaveChanges();
            }

            var sucursal = _ParqueoContexto.sucursales
                .Where(s => s.Id_sucursal == espacio.Id_sucursal)
                .FirstOrDefault();

            if (sucursal != null)
            {
                sucursal.EspaciosDisponibles++;
                _ParqueoContexto.SaveChanges();
            }

            return Ok("Reserva cancelada exitosamente.");
        }
        /// <summary>
        /// EndPoint para mostrar todos los espacios reservados en un día específico.
        /// </summary>
        [HttpGet("ReservasPorDia/{fecha}")]
        public IActionResult ReservasPorDia(DateTime fecha)
        {
            var reservasDelDia = (from reserva in _ParqueoContexto.reservas
                                  join espacio in _ParqueoContexto.EspaciosParqueo on reserva.Id_espacioparqueo equals espacio.Id_espacioparqueo
                                  join sucursal in _ParqueoContexto.sucursales on espacio.Id_sucursal equals sucursal.Id_sucursal
                                  where reserva.Fecha == fecha
                                  select new
                                  {
                                      reserva.Id_reservas,
                                      reserva.Fecha,
                                      reserva.HoraInicio,
                                      reserva.CantidadHoras,
                                      reserva.Estado,
                                      EspacioNumero = espacio.Numero,
                                      EspacioUbicacion = espacio.Ubicacion,
                                      EspacioCostoPorHora = espacio.CostoPorHora,
                                      SucursalNombre = sucursal.Nombre
                                  }).ToList();

            if (!reservasDelDia.Any())
            {
                return NotFound("No se encontraron reservas para ese día.");
            }

            return Ok(reservasDelDia);
        }
        /// <summary>
        /// EndPoint para mostrar los espacios reservados entre dos fechas dadas de una sucursal específica.
        /// </summary>
        /// <returns></returns>
        [HttpGet("ReservasEntreFechas/{idSucursal}/{fechaInicio}/{fechaFin}")]
        public IActionResult ReservasEntreFechas(int idSucursal, DateTime fechaInicio, DateTime fechaFin)
        {
            var reservasEntreFechas = (from reserva in _ParqueoContexto.reservas
                                       join espacio in _ParqueoContexto.EspaciosParqueo on reserva.Id_espacioparqueo equals espacio.Id_espacioparqueo
                                       where espacio.Id_sucursal == idSucursal && reserva.Fecha >= fechaInicio && reserva.Fecha <= fechaFin
                                       select new
                                       {
                                           reserva.Id_reservas,
                                           reserva.Fecha,
                                           reserva.HoraInicio,
                                           reserva.CantidadHoras,
                                           reserva.Estado,
                                           EspacioNumero = espacio.Numero,
                                           EspacioUbicacion = espacio.Ubicacion,
                                           EspacioCostoPorHora = espacio.CostoPorHora
                                       }).ToList();

            if (!reservasEntreFechas.Any())
            {
                return NotFound("No se encontraron reservas en ese rango de fechas para la sucursal especificada.");
            }

            return Ok(reservasEntreFechas);
        }


    }
}
