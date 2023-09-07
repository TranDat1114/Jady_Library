using SellWebsite.Models.Models;

namespace SellWebsite.Models.ViewModels.Customer
{
    public class DetailCompanyVM
    {
        public List<ProductInCompany> ListProductInCompany { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }
}
