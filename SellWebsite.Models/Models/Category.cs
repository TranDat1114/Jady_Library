
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SellWebsite.Models.Models
{
    [Table("Catagories")]
    public class Category
    {
        [Key]
        [Column("CategoryId", TypeName = "int")]
        [DisplayName("Category ID")]
        public int CategoryId { get; set; }
        [Column("CategoryName", TypeName = "varchar(128)")]
        [DisplayName("Category Name")]
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;

        [Column("CategoryDescriptionEnglish", TypeName = "varchar(2048)")]
        [DisplayName("Category Description")]
        [MaxLength(2048)]
        public string? Description { get; set; }

        [ValidateNever]
        public virtual ICollection<Product>? Products { get; set; }
    }
}
