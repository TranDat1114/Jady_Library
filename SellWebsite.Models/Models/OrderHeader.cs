using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SellWebsite.Models.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [ValidateNever]
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

        public DateTime OrderTime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? ReturnTime { get; set; }

        public bool IsReturn { get; set; } = false;
        //public double OrderTotal { get; set; }
        //public double Discount { get; set; }

        public string? OrderStatus { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Name { get; set; }
        public int CompanyId { get; set; }
        [ValidateNever]
        public Company Company { get; set; } = new Company();

    }
}
