using ETrade.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers;

// CategoryController, kategori ile ilgili işlemleri yöneten bir MVC controller'dır.
public class CategoryController : Controller
{
    // CategoryService, kategori verilerini yönetmek için kullanılan bir servistir.
    private readonly CategoryService _categoryService;

    // Constructor, dependency injection kullanarak CategoryService'yi alır.
    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Kategorileri döndüren bir ActionResult metodu
    // Bu metod, kategori verilerini alır ve bir partial view ile döner.
    public async Task<IActionResult> GetCategories()
    {
        // CategoryService kullanarak kategorileri alır.
        var categories = await _categoryService.GetCategoriesAsync();

        // Kategorileri "_CategoriesPartial" adlı partial view ile döner.
        // Partial view, genellikle bir sayfanın parçası olarak yüklenen bir view'dir.
        return PartialView("_CategoriesPartial", categories);
    }
}

