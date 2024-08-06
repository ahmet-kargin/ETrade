namespace ETrade.WebUI.Models.Basket;

public class CartViewModel
{
    public IEnumerable<CartItemViewModel> CartItems { get; set; }
    public decimal TotalAmount { get; set; }
}