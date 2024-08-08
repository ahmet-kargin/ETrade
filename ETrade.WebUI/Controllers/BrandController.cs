using ETrade.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers;


public class BrandController : Controller
{
    private readonly IBrandRepository _brandRepository;

    public BrandController(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<IActionResult> Index()
    {
        var brands = await _brandRepository.GetAll();
        return View(brands);
    }

}
