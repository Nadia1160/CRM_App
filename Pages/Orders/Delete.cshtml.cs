using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CRM_APP.Pages.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (Order == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var order = await _context.Orders.FindAsync(Order.Id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Customers/Index");
        }
    }
}