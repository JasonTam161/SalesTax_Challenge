using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesTax.Models
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        public string ItemName { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Price { get; set; }
        public bool Imported { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        [Display(Name = "Sales Tax: ")]
        public decimal TotalTax { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        [Display(Name = "Total: ")]
        public decimal TotalPrice{ get; set; }

        public bool GSTExempt { get; set; }
    }
}
