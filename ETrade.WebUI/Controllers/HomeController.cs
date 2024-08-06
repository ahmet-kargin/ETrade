using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Connection;
using ETrade.Services.Services;
using ETrade.WebUI.Models;
using ETrade.WebUI.Models.Home;
using ETrade.WebUI.Models.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ETrade.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;

    public HomeController(CategoryService categoryService, ProductService productService, IUserRepository userRepository)
    {
        _categoryService = categoryService;
        _productService = productService;
        _userRepository = userRepository;
    }



    [HttpGet]
    public async Task<IActionResult> GetCategoryAndProductPartial(int? categoryId)
    {
        var categories = await _categoryService.GetCategoriesAsync();
        var products = categoryId.HasValue
            ? await _productService.GetProductsByCategoryAsync(categoryId.Value)
            : await _productService.GetAllProductsAsync();

        // Domain model'i view model'e dönü?tür
        var categoryViewModels = categories.Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name
        });

        var productViewModels = products.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockCode = p.StockCode
        });

        var model = new HomeViewModel
        {
            Categories = categoryViewModels,
            Products = productViewModels,
            SelectedCategoryId = categoryId
        };

        return PartialView("_CategoryAndProductPartial", model);
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId)
    {
        var categories = await _categoryService.GetCategoriesAsync();
        var products = categoryId.HasValue
            ? await _productService.GetProductsByCategoryAsync(categoryId.Value)
            : await _productService.GetAllProductsAsync();

        var categoryViewModels = categories.Select(c => new CategoryViewModel
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();

        var productViewModels = products.Select(p => new ProductViewModel
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            StockCode = p.StockCode
        }).ToList();

        var model = new HomeViewModel
        {
            Categories = categoryViewModels,
            Products = productViewModels,
            SelectedCategoryId = categoryId
        };
        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        return View(model);
    }
    public async Task<IActionResult> UserProfile()
    {
        // Kullan?c? ID'sini session'dan al
        var userIdString = HttpContext.Session.GetString("Id");

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Kullan?c?y? ID'ye göre bul
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        // ViewModel olu?tur
        var model = new UserViewModel
        {
            UserName = user.FirstName + " " + user.LastName, // Örnek: tam ad
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address
            // Di?er kullan?c? bilgilerini ekleyin
        };
        ViewBag.UserProfile = model;
        return View(); // Burada UserViewModel türünde bir model gönderiliyor
    }



}





