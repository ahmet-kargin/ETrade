using System;
using System.Collections.Generic;

namespace ETrade.WebUI.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
