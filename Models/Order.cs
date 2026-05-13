using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_APP.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string OrderNumber { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Range(1, 10000)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a customer.")]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;
    }
}