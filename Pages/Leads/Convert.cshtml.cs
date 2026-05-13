using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CRM_APP.Pages.Leads
{
    public class ConvertModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public ConvertModel(ApplicationDbContext db) => _db = db;

        public Lead Lead { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Lead = await _db.Leads.FindAsync(id);
            if (Lead == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var lead = await _db.Leads.FindAsync(id);
            if (lead == null) return NotFound();

            // Create new account from lead info
            var account = new Account
            {
                Name = lead.Company ?? lead.Name,
                Email = lead.Email,
                Phone = lead.Phone,
                CreatedAt = DateTime.UtcNow
            };
            _db.Accounts.Add(account);

            // Update lead status
            lead.Status = "Converted";
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}