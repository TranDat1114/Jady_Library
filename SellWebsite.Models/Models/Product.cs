using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SellWebsite.Models.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("ProductId", TypeName = "int")]
        [DisplayName("Product ID")]
        public int ProductId { get; set; }

        [Required]
        [Column("ProductTitle", TypeName = "nvarchar(128)")]
        [DisplayName("Product Title")]
        [MaxLength(128)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("ProductAuthor", TypeName = "nvarchar(128)")]
        [DisplayName("Product Author")]
        [MaxLength(128)]
        public string Author { get; set; } = string.Empty;

        [Column("ProductDescription", TypeName = "nvarchar(2048)")]
        [MaxLength(2048)]
        [DisplayName("Product Description")]
        public string? Description { get; set; }

        [Column("ProductCreatedDate", TypeName = "Date")]
        [DisplayName("Created Date")]
        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Column("ProductUpdatedDate", TypeName = "Date")]
        [DisplayName("Updated Date")]

        public DateTime UpdatedDate { get; set; }

        [Column("ProductImage", TypeName = "varchar(256)")]
        [DisplayName("Product Image")]
        [MaxLength(256)]
        public string? Image { get; set; }

        [ValidateNever]
        public virtual ICollection<Category>? Categories { get; set; }

    }
}
