using ETrade.Application.Interfaces;
using ETrade.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(User user)
    {
        await _userRepository.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }
        await _userRepository.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userRepository.DeleteUserAsync(id);
        return NoContent();
    }
}