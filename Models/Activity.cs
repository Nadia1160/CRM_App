using System.ComponentModel.DataAnnotations;

namespace CRM_APP.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; } = "Call"; // Call, Email, Meeting, Note

        [Required, MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime ActivityDate { get; set; } = DateTime.UtcNow;

        // Foreign keys (nullable: can be linked to Lead or Account)
        public int? LeadId { get; set; }
        public Lead? Lead { get; set; }

        public int? AccountId { get; set; }
        public Account? Account { get; set; }
    }
}