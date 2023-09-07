using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SellWebsite.Models.Models
{
    public class ProductInCompany
    {
        [Key]
        public int ProductInCompanyId { get; set; }
        public int ProductId { get; set; }
        [ValidateNever]
        public Product Product { get; set; }
        public int CompanyId { get; set; }

        [ValidateNever]
        public Company Company { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
