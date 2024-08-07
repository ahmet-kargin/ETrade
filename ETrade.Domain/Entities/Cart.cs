namespace Framework.ETrade.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
}

