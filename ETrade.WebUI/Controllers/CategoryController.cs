using ETrade.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers;

public class CategoryController : Controller
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Kategorileri döndüren bir ActionResult metodu
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();
        return PartialView("_CategoriesPartial", categories);
    }
}
