using ETrade.WebUI.Models.Home;

namespace ETrade.WebUI.Models.Basket;

public class CartItemViewModel
{
    public int ProductId { get; set; } // Eklenen özellik
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}


