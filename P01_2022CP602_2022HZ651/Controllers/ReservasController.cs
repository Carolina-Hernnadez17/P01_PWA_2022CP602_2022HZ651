using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
