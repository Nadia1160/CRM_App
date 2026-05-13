using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CRM_APP.Pages.Customers
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public Customer Customer { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _db.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Customer == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer != null)
            {
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}