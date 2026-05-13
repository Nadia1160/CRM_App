using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CRM_APP.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order Order { get; set; } = new();

        [BindProperty]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a customer.")]
        public int SelectedCustomerId { get; set; }

        // This property will hold the list of customers for the dropdown
        public List<Customer> CustomersList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            CustomersList = await _context.Customers.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Reload customers list for the dropdown in case of errors
            CustomersList = await _context.Customers.ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Map the selected customer ID to the Order
            Order.CustomerId = SelectedCustomerId;
            Order.TotalPrice = Order.Quantity * Order.UnitPrice;
            Order.OrderDate = DateTime.UtcNow;

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Order created successfully!";
            return RedirectToPage("/Customers/Index");
        }
    }
}