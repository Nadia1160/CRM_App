using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CRM_APP.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Website { get; set; }
        public string? Industry { get; set; }
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation: an account can have many activities
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

        // If you also want to link lead conversions, you could add LeadId, but not required.
    }
}