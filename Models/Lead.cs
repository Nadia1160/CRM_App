using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CRM_APP.Models
{
    public class Lead
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string? Source { get; set; }  // e.g., "Website", "Referral", "Cold Call"

        [Required]
        public string Status { get; set; } = "New"; // New, Contacted, Qualified, Converted, Lost

        public DateTime? ContactedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation: one lead can have many activities
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
    }
}