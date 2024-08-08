namespace ETrade.WebUI.Models.OrderModel;

public class OrderItemModel
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal => Price * Quantity; // Hesaplanmış özellik
    public string ProductName { get; set; } // Ürün adını ekleyin
}
