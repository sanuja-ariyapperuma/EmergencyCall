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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Hospital hospital) 
        {
            try
            {
                if (String.IsNullOrEmpty(hospital.Name.Trim())) return BadRequest("No hospital name found");
                if ((_context.Hospitals.Any(o => o.Name == hospital.Name.Trim()))) return BadRequest("Hospital name already exists");

                var newHospital = await _context.Hospitals.AddAsync(new Hospital() { Name = hospital.Name });
                await _context.SaveChangesAsync();

                return Ok(_context.Hospitals.Where(e => e.Name == hospital.Name.Trim()).FirstOrDefault().Id);
            }
            catch (Exception)
            {
                return StatusCode(500,"Something went wrong");
                //throw;
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromBody] Hospital hospital)
        {
            try
            {
                var hospitalDetails = _context.Hospitals.Where(e => e.Id == hospital.Id).FirstOrDefault();

                if (hospitalDetails == null) return BadRequest("No hospital found for the id");

                hospitalDetails.Name = hospital.Name.ToString();
                await _context.SaveChangesAsync();

                return Ok("Hospital name changed successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }


        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string hospitalId) 
        {
            try
            {
                var hospital = _context.Hospitals.Where(o => o.Id == Convert.ToInt16(hospitalId)).FirstOrDefault();

                if (hospital == null) return BadRequest("No hospital found");

                _context.Hospitals.Remove(hospital);
                await _context.SaveChangesAsync();

                return Ok("Hospital deleted successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
