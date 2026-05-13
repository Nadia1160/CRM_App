using System.ComponentModel.DataAnnotations;

namespace CRM_APP.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }
        public string? City { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
