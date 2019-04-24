using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public async Task<ActionResult<String>> ReadAsync()
        {
            return "{\"Test\":\"Test\"}";
        }
    }
}
