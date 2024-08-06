namespace ETrade.WebUI.Models.Home;

public class HomeViewModel
{
    public IEnumerable<CategoryViewModel> Categories { get; set; }
    public IEnumerable<ProductViewModel> Products { get; set; }
    public int? SelectedCategoryId { get; set; }
}

