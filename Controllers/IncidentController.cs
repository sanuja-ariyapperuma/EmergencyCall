using Ambulance.Models;
using Ambulance.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

                await _context.IncidentDetails.AddAsync(incidentData);
                await _context.SaveChangesAsync();

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
                        data = _context.IncidentDetails.Where(e => e.PatientStatus != 0).OrderByDescending(c => c.PickupTime).Take(10).ToList();
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

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Treatment([FromBody] VM_Treatment treatment)
        {
            var _treatment = new Treatment();

            if (!(_context.IncidentDetails.Any(o => o.Id == treatment.IncidentId.Trim()))) return BadRequest("Invalid IncidentId");
           
            _treatment.Description = treatment.Description.Trim();
            _treatment.IncidentId = treatment.IncidentId.Trim();
            _treatment.DoctorId = Convert.ToInt32(ExtractClaims().UserId.ToString());
            _treatment.TreatmentTime = DateTime.Now;

            await _context.Treatments.AddAsync(_treatment);
            await _context.SaveChangesAsync();

            return Ok("Treatement added");
        }

        [HttpGet]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> ListTreatments(string incidentId) 
        {
            var db_treatments = _context.Treatments.Where(e => e.IncidentId == incidentId).Include(b => b.UserInfo).OrderByDescending(e => e.TreatmentTime).ToList();

            if (db_treatments != null) 
            {
                var treatment_list = new List<VM_Treatment>();

                foreach (var treatment in db_treatments)
                {
                    var treatment_1 = new VM_Treatment();

                    treatment_1.Description = treatment.Description;
                    treatment_1.TreatmentTime = treatment.TreatmentTime;
                    treatment_1.IncidentId = treatment.IncidentId;
                    treatment_1.Doctor = treatment.UserInfo.Name;

                    treatment_list.Add(treatment_1);

                }
                return Ok(treatment_list);
            }
            else 
            {
                return BadRequest("Invalid incidentId");
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
