using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CRM_APP.Pages.Leads
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DetailsModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public Lead? Lead { get; set; }
        public List<Activity> Activities { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Lead = await _db.Leads
                .Include(l => l.Activities)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (Lead == null)
            {
                return NotFound();
            }

            Activities = Lead.Activities?
                .OrderByDescending(a => a.ActivityDate)
                .ToList() ?? new List<Activity>();

            return Page();
        }

        public async Task<IActionResult> OnPostAddActivity(Activity activity)
        {
            if (!ModelState.IsValid)
            {
                // If validation fails, reload the lead data and return to the page with errors
                Lead = await _db.Leads
                    .Include(l => l.Activities)
                    .FirstOrDefaultAsync(l => l.Id == activity.LeadId);
                if (Lead == null) return NotFound();
                Activities = Lead.Activities?
                    .OrderByDescending(a => a.ActivityDate)
                    .ToList() ?? new();
                return Page();
            }

            activity.ActivityDate = DateTime.UtcNow;
            _db.Activities.Add(activity);
            await _db.SaveChangesAsync();

            // Redirect back to the same lead details page
            return RedirectToPage(new { id = activity.LeadId ?? activity.AccountId });
        }
    }
}