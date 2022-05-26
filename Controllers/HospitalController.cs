using Ambulance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambulance.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public HospitalController(IConfiguration config, DatabaseContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = _context.Hospitals.OrderBy(c => c.Name).ToList();

            return data != null ? Ok(data) : BadRequest("No hospitals found");
        }
    }
}
