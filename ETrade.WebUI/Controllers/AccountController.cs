using ETrade.Domain.Entities;
using ETrade.Infrastructure.Connection;
using ETrade.Services.Services;
using ETrade.WebUI.Models.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETrade.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenService _jwtTokenService;

    public AccountController(ApplicationDbContext context, JwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    [HttpGet]
    public IActionResult Login()
    {
       
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                var token = _jwtTokenService.GenerateToken(user);
                Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = true, Expires = DateTime.UtcNow.AddMinutes(30) });

                // Store user info in session
                HttpContext.Session.SetString("Id", user.Id.ToString());
                HttpContext.Session.SetString("Email", user.Email);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.PhoneNumber,
                Address = model.Address,
                IsActive = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        // Token'ı cookie'den kaldırın
        Response.Cookies.Delete("jwt");
        return RedirectToAction("Login", "Account");
    }
}
