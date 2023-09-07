using SellWebsite.Models.Models;

namespace SellWebsite.Models.ViewModels.Customer
{
    public class UpsertBookVM
    {
        public List<Product> ListProduct { get; set; }
        public ProductInCompany ProductInCompany { get; set; }

    }
}
