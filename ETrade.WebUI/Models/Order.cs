using System;
using System.Collections.Generic;

namespace ETrade.WebUI.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
