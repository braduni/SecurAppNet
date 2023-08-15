using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SecurAppNet.Models;
using SecurAppNet.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecurAppNet.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthUserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }

                var existingUser = await _userService.GetByUsernameAsync(request.Username);

                if (existingUser != null) 
                {
                    return BadRequest("Username already in use. Please choose a different username.");
                }
 
                var newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                };

                await _userService.CreateAsync(newUser);

                return Ok("Registration successful. You can now log in.");

            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.GetByUsernameAsync(request.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return BadRequest("Invalid username or password. Please check your credentials and try again.");
                }

                var roles = new List<string> { "User" };

                if (user.IsAdmin)
                {
                    roles.Add("Admin");
                }

                string token = GenerateJwtToken(user, roles);

                return Ok(token);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private string GenerateJwtToken(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            foreach (var role in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["AppSettings:SecurityKey"]!));

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials
           );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;           
        }
    }
}