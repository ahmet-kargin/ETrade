using ETrade.Application.Interfaces;
using ETrade.WebUI.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            // Ürünleri al
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);

            // Ürünleri ProductViewModel formatına dönüştür
            var productViewModels = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            }).ToList();

            // PartialView döndür
            return PartialView("_ProductList", productViewModels);
        }
    }
}
