namespace ETrade.WebUI.Models.OrderModel;


public class OrderModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<OrderItemModel> OrderItems { get; set; } // OrderItems özellik ismiyle uyumlu
}

