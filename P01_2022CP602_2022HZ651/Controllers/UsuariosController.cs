using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022CP602_2022HZ651.Models;

namespace P01_2022CP602_2022HZ651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly  ParqueoContext _ParqueoContexto;

        public UsuariosController(ParqueoContext usuarioContexto)
        {
            _ParqueoContexto = usuarioContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado de todos los usuarios existentes en la bd
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Usuarios> listadousuarios = (from e in _ParqueoContexto.usuarios
                                          select e).ToList();
            if (listadousuarios.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadousuarios);
        }

        /// <summary>
        /// EndPoint guardar usuarios
        /// </summary>
        /// <returns></returns>



        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarUsuarios([FromBody] Usuarios usuarios)
        {
            try
            {
                _ParqueoContexto.usuarios.Add(usuarios);
                _ParqueoContexto.SaveChanges();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al guardar el usuario: {ex.Message}");
            }
        }
        /// <summary>
        /// EndPoint actualizar usuarios
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuarios(int id, [FromBody] Usuarios usuariosModificar)
        {
            Usuarios? usuariosActual = (from e in _ParqueoContexto.usuarios
                                    where e.Id_usuario == id
                                    select e).FirstOrDefault();

            if (usuariosActual == null)
            {
                return NotFound();
            }

            usuariosActual.Nombre = usuariosModificar.Nombre;
            usuariosActual.Correo = usuariosModificar.Correo;
            usuariosActual.Telefono = usuariosModificar.Telefono;
            usuariosActual.Contrasena = usuariosModificar.Contrasena;
            usuariosActual.Rol = usuariosModificar.Rol;




            _ParqueoContexto.Entry(usuariosActual).State = EntityState.Modified;
            _ParqueoContexto.SaveChanges();

            return Ok(usuariosModificar);
        }
        /// <summary>
        /// EndPoint eliminar Usuarios
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            Usuarios? usuario = (from e in _ParqueoContexto.usuarios
                              where e.Id_usuario == id
                              select e).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            _ParqueoContexto.usuarios.Attach(usuario);
            _ParqueoContexto.usuarios.Remove(usuario);
            _ParqueoContexto.SaveChanges();

            return Ok(usuario);
        }
        /// <summary>
        /// EndPoint para retornar el listado de los platos filtrados cuando el precio sea menor de un valor dado.
        /// </summary>
        /// <returns></returns>

    }
}
