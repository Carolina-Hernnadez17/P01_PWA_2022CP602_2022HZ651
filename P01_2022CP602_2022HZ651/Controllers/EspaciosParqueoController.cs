using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022CP602_2022HZ651.Models;

namespace P01_2022CP602_2022HZ651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspaciosParqueoController : ControllerBase
    {
        private readonly ParqueoContext _context;

        public EspaciosParqueoController(ParqueoContext parqueoContext)
        {
            _context = parqueoContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllEspaciosParqueo()
        {
            var query = from espacio in _context.EspaciosParqueo
                        join sucursal in _context.sucursales
                          on espacio.Id_sucursal equals sucursal.Id_sucursal
                        where espacio.Estado == "Disponible"
                        select new
                        {
                            espacio.Id_espacioparqueo,
                            espacio.Numero,
                            espacio.Ubicacion,
                            espacio.CostoPorHora,
                            espacio.Estado,
                            Sucursal = sucursal.Nombre
                        };

            var EspaciosParqueo = query.ToList();

            if (!EspaciosParqueo.Any())
            {
                return NotFound("No hay espacios de parqueo disponibles.");
            }

            return Ok(EspaciosParqueo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddEspacioParqueo([FromBody] EspaciosParqueo nuevoEspacio)
        {
            try
            {
                var sucursalExiste = (from suc in _context.sucursales
                                      where suc.Id_sucursal == nuevoEspacio.Id_sucursal
                                      select suc).Any();
                if (!sucursalExiste)
                {
                    return NotFound($"No se encontró la sucursal con ID {nuevoEspacio.Id_sucursal}.");
                }

                if (nuevoEspacio.Estado != "Disponible" && nuevoEspacio.Estado != "Ocupado")
                {
                    return BadRequest("El estado del espacio de parqueo debe ser 'Disponible' o 'Ocupado'.");
                }

                _context.EspaciosParqueo.Add(nuevoEspacio);
                _context.SaveChanges();
                return Ok(nuevoEspacio);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar el espacio de parqueo: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult UpdateEspacioParqueo(int id, [FromBody] EspaciosParqueo espacioModificar)
        {
            if (id != espacioModificar.Id_espacioparqueo)
            {
                return BadRequest("El ID del espacio de parqueo no coincide.");
            }

            var query = from espacio in _context.EspaciosParqueo
                        join sucursal in _context.sucursales
                          on espacio.Id_sucursal equals sucursal.Id_sucursal
                        where espacio.Id_espacioparqueo == id
                        select espacio;

            var espacioActual = query.FirstOrDefault();

            if (espacioActual == null)
            {
                return NotFound($"No se encontró el espacio de parqueo con ID {id}.");
            }

            espacioActual.Estado = espacioModificar.Estado;
            espacioActual.Ubicacion = espacioModificar.Ubicacion;
            espacioActual.CostoPorHora = espacioModificar.CostoPorHora;

            _context.Entry(espacioActual).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(espacioActual);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteEspacioParqueo(int id)
        {
            var query = from espacio in _context.EspaciosParqueo
                        join sucursal in _context.sucursales
                          on espacio.Id_sucursal equals sucursal.Id_sucursal
                        where espacio.Id_espacioparqueo == id
                        select espacio;

            var espacioParqueo = query.FirstOrDefault();

            if (espacioParqueo == null)
            {
                return NotFound($"No se encontró el espacio de parqueo con ID {id}.");
            }

            if (espacioParqueo.Estado == "Ocupado")
            {
                return BadRequest("No se puede eliminar un espacio de parqueo ocupado.");
            }

            _context.EspaciosParqueo.Remove(espacioParqueo);
            _context.SaveChanges();

            return Ok($"Espacio de parqueo con ID {id} eliminado correctamente.");
        }


        


    }
}
