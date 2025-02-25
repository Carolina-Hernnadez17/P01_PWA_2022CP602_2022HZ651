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
        private readonly ParqueoContext _ParqueoContext;

        public EspaciosParqueoController(ParqueoContext parqueoContext)
        {
            _ParqueoContext = parqueoContext;
        }

        //EndPoint para listar todos lo espacios en el parqueo
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAllEspaciosParqueo()
        {
            var espacios = _ParqueoContext.espaciosParqueos.ToList();
            if (!espacios.Any())
            {
                return NotFound("No hay espacios de parqueo registrados.");
            }
            return Ok(espacios);
        }

        //EndPoint para listar los parqueos por ID
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult GetEspacioParqueo(int id)
        {
            var espacio = _ParqueoContext.espaciosParqueos
                .Where(e => e.Id_espacioparqueo == id).ToList();

            if (!espacio.Any())
            {
                return NotFound($"No se encontró el espacio de parqueo con ID {id}.");
            }

            return Ok(espacio);
        }

        //EndPonit para agregar un nuevo parqueo
        [HttpPost]
        [Route("Add")]
        public IActionResult AddEspacioParqueo([FromBody] EspaciosParqueo espacio)
        {
            try
            {
                _ParqueoContext.espaciosParqueos.Add(espacio);
                _ParqueoContext.SaveChanges();
                return Ok(espacio);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al agregar el espacio de parqueo: {ex.Message}");
            }
        }

        //EndPonit para editar un parqueo
        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult UpdateEspacioParqueo(int id, [FromBody] EspaciosParqueo espacioModificar)
        {
            if (id != espacioModificar.Id_espacioparqueo)
            {
                return BadRequest("El ID del espacio de parqueo no coincide.");
            }

            var espacioActual = _ParqueoContext.espaciosParqueos.Find(id);

            if (espacioActual == null)
            {
                return NotFound($"No se encontró el espacio de parqueo con ID {id}.");
            }

            espacioActual.Estado = espacioModificar.Estado;
            espacioActual.Ubicacion = espacioModificar.Ubicacion;

            _ParqueoContext.Entry(espacioActual).State = EntityState.Modified;
            _ParqueoContext.SaveChanges();

            return Ok(espacioActual);
        }

        //EndPoint para eliminar un parqueo
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteEspacioParqueo(int id)
        {
            var espacio = _ParqueoContext.espaciosParqueos.Find(id);

            if (espacio == null)
            {
                return NotFound($"No se encontró el espacio de parqueo con ID {id}.");
            }

            _ParqueoContext.espaciosParqueos.Remove(espacio);
            _ParqueoContext.SaveChanges();

            return Ok($"Espacio de parqueo con ID {id} eliminado correctamente.");
        }


    }
}
