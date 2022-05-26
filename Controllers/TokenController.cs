using Ambulance.Models;
using Ambulance.Models.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ambulance.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseControllerClass
    {
        public IConfiguration _configuration;
        private readonly DatabaseContext _context;

        public TokenController(IConfiguration config, DatabaseContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(VM_UserRole _userData) 
        {
            if(_userData != null && _userData.Email != null && _userData.Password != null) 
            {
                UserInfo user = await GetUser(_userData.Email, _userData.Password);

                if(user != null) 
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Name", user.Name.ToString()),
                        new Claim("UserRole", user.UserRole.Name.ToString()),
                        new Claim(ClaimTypes.Role, user.UserRole.Name.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        private async Task<UserInfo> GetUser(string email, string password)
        {
            var user = await _context.UserInfos.Include(x => x.UserRole).FirstOrDefaultAsync(u => u.Email == email && u.Is_active);

            return (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password)) ? user : null;
            
        }

        
    }
}
