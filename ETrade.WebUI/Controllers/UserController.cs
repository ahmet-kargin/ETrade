using ETrade.Application.Interfaces;
using ETrade.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers;


// UsersController, kullanıcı CRUD işlemlerini yöneten bir API controller'dır.
[ApiController] // Bu attribute, bu controller'ın bir API controller olduğunu belirtir.
[Route("api/[controller]")] // Bu attribute, API endpoint'lerinin temel yolunu belirtir (örn. /api/users).
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    // Constructor, dependency injection kullanarak IUserRepository'yi alır.
    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Tüm kullanıcıları getiren endpoint
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        // Tüm kullanıcıları repository'den al
        var users = await _userRepository.GetAllUsersAsync();
        // 200 OK durum kodu ile kullanıcıları döner
        return Ok(users);
    }

    // Belirli bir kullanıcıyı ID'ye göre getiren endpoint
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        // ID'ye göre kullanıcıyı repository'den al
        var user = await _userRepository.GetUserByIdAsync(id);
        // Kullanıcı bulunamazsa 404 Not Found döner
        if (user == null)
        {
            return NotFound();
        }
        // Kullanıcı bulunursa 200 OK durum kodu ile kullanıcıyı döner
        return Ok(user);
    }

    // Yeni bir kullanıcı oluşturan endpoint
    [HttpPost]
    public async Task<ActionResult> CreateUser(User user)
    {
        // Yeni kullanıcıyı repository'ye ekler
        await _userRepository.AddUserAsync(user);
        // Kullanıcı oluşturulduktan sonra 201 Created durum kodu ile kullanıcıyı döner
        // Ayrıca, GetUser endpoint'inin yolunu ve yeni kullanıcının ID'sini belirtir
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // Belirli bir kullanıcıyı güncelleyen endpoint
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, User user)
    {
        // ID'nin kullanıcı ID'si ile eşleşip eşleşmediğini kontrol eder
        if (id != user.Id)
        {
            return BadRequest();
        }
        // Kullanıcıyı repository'de günceller
        await _userRepository.UpdateUserAsync(user);
        // Güncelleme işlemi başarılıysa 204 No Content durum kodu döner
        return NoContent();
    }

    // Belirli bir kullanıcıyı silen endpoint
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        // Kullanıcıyı repository'den siler
        await _userRepository.DeleteUserAsync(id);
        // Silme işlemi başarılıysa 204 No Content durum kodu döner
        return NoContent();
    }
}