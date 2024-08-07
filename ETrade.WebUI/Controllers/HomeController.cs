using ETrade.Application.Interfaces;
using ETrade.Infrastructure.Connection;
using ETrade.Services.Services;
using ETrade.WebUI.Models.Home;
using ETrade.WebUI.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly ProductService _productService;
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _context;

    // Dependency Injection kullanılarak gerekli servislerin alınması
    public HomeController(CategoryService categoryService, ProductService productService, IUserRepository userRepository)
    {
        _categoryService = categoryService;
        _productService = productService;
        _userRepository = userRepository;
    }


    // Kategoriler ve ürünler için partial view'i döndüren action method
    [HttpGet]
    public async Task<IActionResult> GetCategoryAndProductPartial(int? categoryId)
    {
        // Kategorileri getir
        var categories = await _categoryService.GetCategoriesAsync();

        // Eğer categoryId varsa, o kategoriye ait ürünleri getir, yoksa tüm ürünleri getir
        var products = categoryId.HasValue
            ? await _productService.GetProductsByCategoryAsync(categoryId.Value)
            : await _productService.GetAllProductsAsync();

        // Domain model'i view model'e dönüştür
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

        // HomeViewModel oluştur ve verileri ata
        var model = new HomeViewModel
        {
            Categories = categoryViewModels,
            Products = productViewModels,
            SelectedCategoryId = categoryId
        };

        // "_CategoryAndProductPartial" partial view'ini döndür
        return PartialView("_CategoryAndProductPartial", model);
    }

    // Ana sayfa için action method
    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId)
    {
        // Kategorileri getir
        var categories = await _categoryService.GetCategoriesAsync();

        // Eğer categoryId varsa, o kategoriye ait ürünleri getir, yoksa tüm ürünleri getir
        var products = categoryId.HasValue
            ? await _productService.GetProductsByCategoryAsync(categoryId.Value)
            : await _productService.GetAllProductsAsync();

        // Domain model'leri view model'lere dönüştür
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

        // HomeViewModel oluştur ve verileri ata
        var model = new HomeViewModel
        {
            Categories = categoryViewModels,
            Products = productViewModels,
            SelectedCategoryId = categoryId
        };

        // ViewBag üzerinden kategorileri ve seçilen kategori ID'sini view'e gönder
        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        return View(model);
    }

    // Kullanıcı profilini görüntüleyen action method
    public async Task<IActionResult> UserProfile()
    {
        // Kullanıcı ID'sini session'dan al
        var userIdString = HttpContext.Session.GetString("Id");

        // Eğer ID yoksa veya geçerli bir ID değilse, login sayfasına yönlendir
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        // Kullanıcıyı ID'ye göre bul
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        // UserViewModel oluştur ve verileri ata
        var model = new UserViewModel
        {
            UserName = user.FirstName + " " + user.LastName, // Örnek: tam ad
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address
        };

        // ViewBag üzerinden kullanıcı profilini view'e gönder
        ViewBag.UserProfile = model;
        return View(); // Burada UserViewModel türünde bir model gönderiliyor
    }



}





