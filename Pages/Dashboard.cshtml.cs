using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CRM_APP.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public DashboardModel(ApplicationDbContext db) => _db = db;

        // KPIs
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalLeads { get; set; }
        public int ConvertedLeads { get; set; }
        public int QualifiedLeads { get; set; }
        public int TotalAccounts { get; set; }

        // Orders chart data (existing)
        public List<string> CustomerNames { get; set; } = new();
        public List<int> OrderCounts { get; set; } = new();

        // Lead source chart data
        public Dictionary<string, int> LeadSources { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Existing counts
            TotalCustomers = await _db.Customers.CountAsync();
            TotalOrders = await _db.Orders.CountAsync();
            var allOrders = await _db.Orders.ToListAsync();
            TotalRevenue = allOrders.Sum(o => o.TotalPrice);

            // New KPIs
            TotalLeads = await _db.Leads.CountAsync();
            ConvertedLeads = await _db.Leads.CountAsync(l => l.Status == "Converted");
            QualifiedLeads = await _db.Leads.CountAsync(l => l.Status == "Qualified");
            TotalAccounts = await _db.Accounts.CountAsync();

            // Orders per customer (existing chart)
            var customersWithOrders = await _db.Customers
                .Include(c => c.Orders)
                .Select(c => new { c.Name, OrderCount = c.Orders.Count })
                .Where(c => c.OrderCount > 0)
                .Take(5)
                .ToListAsync();
            CustomerNames = customersWithOrders.Select(c => c.Name).ToList();
            OrderCounts = customersWithOrders.Select(c => c.OrderCount).ToList();

            // Lead source chart
            LeadSources = await _db.Leads
                .GroupBy(l => l.Source ?? "Unknown")
                .Select(g => new { Source = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Source, v => v.Count);
        }
    }
}