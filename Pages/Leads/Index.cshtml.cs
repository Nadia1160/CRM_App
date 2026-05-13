using System.Text;
using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CRM_APP.Pages.Leads
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Lead> Leads { get; set; } = new();
        public string? SearchTerm { get; set; }

        public IndexModel(ApplicationDbContext db) => _db = db;

        public async Task OnGetAsync(string? searchTerm)
        {
            SearchTerm = searchTerm;
            var query = _db.Leads.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(l => l.Name.Contains(searchTerm) ||
                                         l.Email.Contains(searchTerm) ||
                                         (l.Company != null && l.Company.Contains(searchTerm)));
            }
            Leads = await query.OrderByDescending(l => l.CreatedAt).ToListAsync();
        }
        public async Task<IActionResult> OnGetExport()
        {
            var leads = await _db.Leads.ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Name,Email,Company,Status,Source,CreatedAt");
            foreach (var l in leads)
            {
                csv.AppendLine($"{l.Name},{l.Email},{l.Company},{l.Status},{l.Source},{l.CreatedAt}");
            }
            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "leads.csv");
        }
        [HttpPost]
        public async Task<IActionResult> OnPostCreateLeadJson([FromBody] Lead newLead)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false, message = "Invalid data" });

            newLead.CreatedAt = DateTime.UtcNow;
            _db.Leads.Add(newLead);
            await _db.SaveChangesAsync();

            return new JsonResult(new { success = true, lead = new { newLead.Id, newLead.Name, newLead.Email, newLead.Company, newLead.Status, newLead.Source } });
        }
    }
}