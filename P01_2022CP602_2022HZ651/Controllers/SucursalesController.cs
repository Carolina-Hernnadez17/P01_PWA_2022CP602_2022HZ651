using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022CP602_2022HZ651.Models;

namespace P01_2022CP602_2022HZ651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalesController : ControllerBase
    {
        private readonly ParqueoContext _ParqueoContext;

        public SucursalesController(ParqueoContext parqueoContext)
        {
            _ParqueoContext = parqueoContext;
        }

        // EndPont para obtener todas las sucursales
        [HttpGet]
        [Route("GetAll")]

        public IActionResult GetAllSucursal() 
        {
            var suculsales = _ParqueoContext.sucursales.ToList();
            if (!suculsales.Any())
            {
                return NotFound("No hay sucursales registradas.");
            }
            return Ok(suculsales);
        }

        // Endpoint para btener una sucursal por ID
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetSucursal(int id)
        {
            var sucursal =  _ParqueoContext.sucursales.
                Where(s => s.Id_sucursal == id).ToList();

            if (!sucursal.Any())
            {
                return NotFound($"No se encontró la sucursal con ID {id}.");
            }

            return Ok(sucursal);
        }

        // Endpoint para agregar una sucursal 
        [HttpPost]
        [Route("Add")]
        public IActionResult AddSucursal([FromBody] Sucursales sucursal)
        {
            try
            {
                _ParqueoContext.sucursales.Add(sucursal);
                _ParqueoContext.SaveChanges();
                return Ok(sucursal); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar la sucursal: {ex.Message}");
            }
        }

        // Endpoint para actualizar una sucursal 
        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult UpdateSucursal(int id, [FromBody] Sucursales sucursalModificar)
        {
            if (id != sucursalModificar.Id_sucursal)
            {
                return BadRequest("El ID de la sucursal no coincide.");
            }

            var sucursalActual = _ParqueoContext.sucursales.Find(id);

            if (sucursalActual == null)
            {
                return NotFound($"No se encontró la sucursal con ID {id}.");
            }

            sucursalActual.Nombre = sucursalModificar.Nombre;
            sucursalActual.Direccion = sucursalModificar.Direccion;
            sucursalActual.Telefono = sucursalModificar.Telefono;
            sucursalActual.Id_usuario = sucursalModificar.Id_usuario;
            sucursalActual.EspaciosDisponibles = sucursalModificar.EspaciosDisponibles;

            _ParqueoContext.Entry(sucursalActual).State = EntityState.Modified;
            _ParqueoContext.SaveChanges();

            return Ok(sucursalActual); 
        }

        // Endpoint para elimnar una sucursal 
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteSucursal(int id)
        {
            var sucursal = _ParqueoContext.sucursales.Find(id);

            if (sucursal == null)
            {
                return NotFound($"No se encontró la sucursal con ID {id}.");
            }

            _ParqueoContext.sucursales.Remove(sucursal);
            _ParqueoContext.SaveChanges();

            return Ok($"Sucursal con ID {id} eliminada correctamente.");
        }

    }
}
