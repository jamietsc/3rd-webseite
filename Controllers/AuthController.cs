using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThirdWebseite.Models.ViewModels;
using ThirdWebseite.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ThirdWebseite.Controllers
{
    [Route("api/[controller]")]
[ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration; // Add this field

        public AuthController(IConfiguration configuration) // Add this constructor
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            if (model.UserName == "root" && model.Password == "1234")
            {
                var token = GenerateJwtToken(model.UserName);
                return Ok(new LoginResponseModel { Token = token });
            }
            return null;
        }

        private string GenerateJwtToken(string userName)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "Admin")
            };
            string secret = _configuration.GetValue<string>("Jwt:Secret"); // Use _configuration here
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
