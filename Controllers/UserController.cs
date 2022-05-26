using Ambulance.Models;
using Ambulance.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ambulance.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public UserController(IConfiguration config, DatabaseContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get() 
        {
            var users = _context.UserInfos.Include(e => e.UserRole).ToList();

            foreach (var user in users) 
            {
                user.Password = "";
            }

            return Ok(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] UserInfo user)
        {
            if (user == null) return BadRequest("No data found");
            if (String.IsNullOrEmpty(user.Name)) return BadRequest("No name found");
            if (String.IsNullOrEmpty(user.Email)) return BadRequest("No email found");
            if (_context.UserInfos.AnyAsync(e => e.Email == user.Email).Result) return BadRequest("Email already exists");
            if (!IsValidEmail(user.Email.Trim())) return BadRequest("Invalid email found");
            if (!IsDigitsOnly(user.Contact)) return BadRequest("Invalid contact found");
            if (String.IsNullOrEmpty(user.Password)) return BadRequest("No password found");
            if(user.User_role == 0 || !(_context.UserRoles.AnyAsync(e => e.Id == user.User_role).Result)) return BadRequest("Invalid user role found");

            await _context.UserInfos.AddAsync(
                new UserInfo() 
                {
                    Name = user.Name,
                    Email = user.Email,
                    Contact = user.Contact,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    Is_active = user.Is_active,
                    User_role = user.User_role
                }
                );
            await _context.SaveChangesAsync();

            var usersId = _context.UserInfos.Where(e => e.Email == user.Email).FirstOrDefaultAsync().Result.Id;
            return Ok(usersId);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromBody] VM_StatusChange user) 
        {
            var userdata = _context.UserInfos.Where(e => e.Id == user.Id).FirstOrDefaultAsync().Result;

            if (userdata == null) return BadRequest("Invalid userid found");

            userdata.Is_active = user.Is_active;

            await _context.SaveChangesAsync();

            return Ok("User active status changed successfully");
        }

        private bool IsValidEmail(string email) 
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


    }
}
