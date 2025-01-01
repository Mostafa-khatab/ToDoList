using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Models;
using System.Security.Cryptography;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly JwtOptions _jwtOptions;
        private readonly AppDbContext _db;

        public UsersController(JwtOptions jwtOptions, AppDbContext db)
        {
            _jwtOptions = jwtOptions;
            _db = db;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Users request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            // Check if user already exists
            if (_db.Users.Any(u => u.UserName == request.UserName))
            {
                return BadRequest("User already exists.");
            }

            request.Password = HashPassword(request.Password);

            await _db.Users.AddAsync(request);
            await _db.SaveChangesAsync();
            return Ok("User registered successfully.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Users request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid authentication request.");
            }

            var user = _db.Users.FirstOrDefault(u => u.UserName == request.UserName);
            if (user == null || !VerifyPassword(request.Password, user.Password))
            {
                return BadRequest("Username or Password is incorrect.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Email, "default@example.com"), 
                    new Claim(ClaimTypes.Role, "User"), // Example: Add role-based claims
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return Ok(new { AccessToken = accessToken });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            var inputHash = HashPassword(inputPassword);
            return inputHash == storedPasswordHash;
        }
    }
}
