using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesTax.Models
{
    public class Items
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        [Column(TypeName = "decimal(4,2)")]
        public decimal Price { get; set; }
        public bool Imported { get; set; }

        public bool GSTExempt { get; set; }

        [Display(Name = "Quantity")]
        public int QuantityId { get; set; }
    }
}
