using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRM_APP.Pages.Leads
{
    public class ImportModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ImportModel(ApplicationDbContext db) => _db = db;

        [BindProperty]
        public IFormFile? file { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a CSV file.");
                return Page();
            }

            var leads = new List<Lead>();
            using var reader = new StreamReader(file.OpenReadStream());
            // Skip header if exists – assume first line is header
            await reader.ReadLineAsync(); // skip header
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    var lead = new Lead
                    {
                        Name = parts[0].Trim(),
                        Email = parts[1].Trim(),
                        Company = parts.Length > 2 ? parts[2].Trim() : "",
                        Status = parts.Length > 3 ? parts[3].Trim() : "New",
                        Source = parts.Length > 4 ? parts[4].Trim() : "Import",
                        CreatedAt = DateTime.UtcNow
                    };
                    leads.Add(lead);
                }
            }
            await _db.Leads.AddRangeAsync(leads);
            await _db.SaveChangesAsync();
            TempData["Message"] = $"{leads.Count} leads imported successfully.";
            return RedirectToPage("./Index");
        }
    }
}