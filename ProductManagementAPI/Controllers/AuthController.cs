using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductManagementAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto loginRequest)
        {
            if (loginRequest.Username != "admin" || loginRequest.Password != "password")
            {
                return Unauthorized("Invalid username or password");
            }
            var token = GenerateToken(loginRequest.Username);
            return Ok(new { Token = token });
        }

        private string GenerateToken(string username)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var claims = new[] { new Claim(System.Security.Claims.ClaimTypes.Name, username) };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


