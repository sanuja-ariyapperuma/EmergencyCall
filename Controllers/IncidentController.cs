using Ambulance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambulance.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public IncidentController(IConfiguration config, DatabaseContext context)
        {
            _configuration = config;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> New([FromBody] IncidentDetail _incidentData)
        {
            try
            {
                _incidentData.Id = GenerateIncidentID();
                _incidentData.PickupTime = DateTime.Now;
                _incidentData.DischargedDoctorId = null;

                var result = await _context.IncidentDetails.AddAsync(_incidentData);
                var saveStatus = await _context.SaveChangesAsync();

                return Ok("Incident Added");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string GenerateIncidentID() 
        {
            var recordCount = _context.IncidentDetails.ToList().Count;

            var today = DateTime.Now;
            string year = today.Year.ToString();
            string month = today.Month.ToString();
            string day = today.Day.ToString();

            return String.Concat("AP",year,month,day,(recordCount+1).ToString("D3"));

        }
    }
}
