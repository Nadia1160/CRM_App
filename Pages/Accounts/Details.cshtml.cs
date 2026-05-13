using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CRM_APP.Pages.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Account Account { get; set; } = default!;

        public async Task OnGetAsync(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return;
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (account != null)
            {
                Account = account;
            }
        }
    }
}