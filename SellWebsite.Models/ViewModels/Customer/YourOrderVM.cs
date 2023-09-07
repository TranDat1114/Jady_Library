using SellWebsite.Models.Models;

namespace SellWebsite.Models.ViewModels.Customer
{
    public class YourOrderVM
    {
        public List<OrderHeader> ListOrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }

    }
}
