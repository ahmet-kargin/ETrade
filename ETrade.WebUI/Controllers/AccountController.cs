using ETrade.Domain.Entities;
using ETrade.Infrastructure.Connection;
using ETrade.Services.Services;
using ETrade.WebUI.Models.Login;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ETrade.Application.Interfaces;

namespace ETrade.WebUI.Controllers;

// AccountController, kullanıcı hesap işlemlerini yönetir (giriş, kayıt, çıkış).
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenService _jwtTokenService;
    private readonly ICurrentUser _currentUser;


    // Dependency Injection kullanılarak gerekli servislerin alınması
    public AccountController(ApplicationDbContext context, JwtTokenService jwtTokenService, ICurrentUser currentUser)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _currentUser = currentUser;
    }

    // Giriş sayfasını görüntüleyen action method
    [HttpGet]
    public async Task<IActionResult> Login()
    {

        //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return View();
    }

    // Giriş işlemlerini yöneten action method
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
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

                // Claims oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    // İsteğe bağlı olarak diğer kullanıcı bilgilerini de ekleyebilirsiniz
                };

                // ClaimsIdentity oluştur
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // AuthenticationProperties oluştur
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Oturum süresi
                };

                // Kullanıcıyı oturum açma işlemi gerçekleştirin
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Anasayfaya yönlendir
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Geçersiz giriş denemesi durumunda hata mesajı ekle
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }
        return View(model);
    }

    // Kayıt sayfasını görüntüleyen action method
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View();
    }

    // Kayıt işlemlerini yöneten action method
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // E-posta adresinin veritabanında olup olmadığını kontrol et
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                // E-posta adresi zaten mevcut
                ModelState.AddModelError(string.Empty, "Bu e-posta adresi zaten kullanımda.");
                return View(model); // RedirectToAction yerine View döndür
            }
            else
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
        }
        return View(model);
    }

    // Çıkış işlemini yöneten action method
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // ClaimsIdentity'yi temizle
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Token'ı cookie'den kaldır
        Response.Cookies.Delete("jwt");

        // Giriş sayfasına yönlendir
        return RedirectToAction("Login", "Account");
    }
}
