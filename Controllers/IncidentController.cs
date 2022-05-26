using Ambulance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ambulance.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IncidentController : BaseControllerClass
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public IncidentController(IConfiguration config, DatabaseContext context)
        {
            _configuration = config;
            _context = context;
        }


        [HttpPost]
        [Authorize(Roles = "Ambulance")]
        public async Task<IActionResult> Create([FromBody] IncidentDetail incidentData)
        {
            try
            {
                incidentData.Id = GenerateIncidentID();
                incidentData.PickupTime = DateTime.Now;
                incidentData.DischargedDoctorId = null;
                incidentData.AmbulanceId = Convert.ToInt16(ExtractClaims().UserId.ToString());

                var result = await _context.IncidentDetails.AddAsync(incidentData);
                var saveStatus = await _context.SaveChangesAsync();

                return Ok(incidentData.Id);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Ambulance,Doctor")]
        public async Task<IActionResult> List() 
        {
            var claims = ExtractClaims();

            if(claims != null) 
            {
                List<IncidentDetail> data = null;

                switch (claims.Userrole)
                {
                    case "Ambulance":
                        data = _context.IncidentDetails.Where(e => e.AmbulanceId == Convert.ToInt32(claims.UserId)).OrderByDescending(c => c.PickupTime).Take(10).ToList();
                        break;
                    case "Doctor":
                        data = _context.IncidentDetails.OrderByDescending(c => c.PickupTime).Take(10).ToList();
                        break;
                }


                return (data != null) ? Ok(data) : BadRequest("No records found");

            }
            else 
            {
                return BadRequest();
            }
            
            
        }

        [HttpPost]
        [Authorize(Roles = "Ambulance")]
        public async Task<IActionResult> DropOff(string incidentId) 
        {
            var incident = _context.IncidentDetails.Where(e => e.Id == incidentId).First();

            if (incident != null)
            {
                incident.DropOffTime = DateTime.Now;
                incident.PatientStatus = 1;

                var saveStatus = await _context.SaveChangesAsync();

                return Ok("Incident updated");
            }
            else 
            {
                return BadRequest("No incidents found");
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
