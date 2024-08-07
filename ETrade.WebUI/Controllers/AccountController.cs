using ETrade.Domain.Entities;
using ETrade.Infrastructure.Connection;
using ETrade.Services.Services;
using ETrade.WebUI.Models.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETrade.WebUI.Controllers;

// AccountController, kullanıcı hesap işlemlerini yönetir (giriş, kayıt, çıkış).
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenService _jwtTokenService;

    // Dependency Injection kullanılarak gerekli servislerin alınması
    public AccountController(ApplicationDbContext context, JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    // Giriş sayfasını görüntüleyen action method
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Giriş işlemlerini yöneten action method
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Kullanıcıyı email ile veritabanından bul
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                // JWT token oluştur
                var token = _jwtTokenService.GenerateToken(user);
                // Token'ı cookie'ye ekle
                Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(30) });

                // Kullanıcı bilgilerini session'a ekle
                HttpContext.Session.SetString("Id", user.Id.ToString());
                HttpContext.Session.SetString("Email", user.Email);

                // Anasayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }

            // Geçersiz giriş denemesi durumunda hata mesajı ekle
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    // Kayıt sayfasını görüntüleyen action method
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // Kayıt işlemlerini yöneten action method
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Yeni kullanıcı oluştur
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.PhoneNumber,
                Address = model.Address,
                IsActive = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password) // Şifreyi hashle
            };

            // Kullanıcıyı veritabanına ekle
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Anasayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    // Çıkış işlemini yöneten action method
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // Token'ı cookie'den kaldır
        Response.Cookies.Delete("jwt");
        // Giriş sayfasına yönlendir
        return RedirectToAction("Login", "Account");
    }
}
