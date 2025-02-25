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

    }
}
