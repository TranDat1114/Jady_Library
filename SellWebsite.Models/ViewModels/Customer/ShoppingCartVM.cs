using SellWebsite.Models.Models;

namespace SellWebsite.Models.ViewModels.Customer
{
    public class ShoppingCartVM
    {
        public List<ShoppingCart> Carts { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public List<Company> Companies { get; set; }
    }
}
