using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurAppNet.Models;
using SecurAppNet.Services.UserService;
using System.Data;

namespace SecurAppNet.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAll() 
        {
            try
            {
                var users = await _userService.GetAllAsync();

                var usersDto = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    isAdmin = user.IsAdmin      
                });

                return Ok(usersDto);

            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }  
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid) 
                {
                    return BadRequest(ModelState);
                }

                var userToUpdate = await _userService.GetByIdAsync(id);

                if (userToUpdate == null) 
                {
                    return NotFound("User not found.");
                }

                var existingUserWithUsername = await _userService.GetByUsernameAsync(request.Username);

                if (existingUserWithUsername != null && existingUserWithUsername.Id != id) 
                {
                    return BadRequest("Username already taken.");
                }

                userToUpdate.Username = request.Username;
                userToUpdate.IsAdmin = request.IsAdmin;

                await _userService.UpdateAsync(userToUpdate);

                return NoContent();

            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);

                if (user == null) 
                {
                    return NotFound();
                }

                await _userService.DeleteAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}