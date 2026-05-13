using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CRM_APP.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Account Account { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Account.CreatedAt = DateTime.UtcNow;
            _context.Accounts.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}