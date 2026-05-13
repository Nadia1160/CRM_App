using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CRM_APP.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Customer> Customers { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            var query = _db.Customers.Include(c => c.Orders).AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(SearchTerm) ||
                                         (c.City != null && c.City.Contains(SearchTerm)));
            }

            Customers = await query.ToListAsync();
        }

        // PDF Export handler
        public async Task<IActionResult> OnGetExportPdf()
        {
            var customers = await _db.Customers.Include(c => c.Orders).ToListAsync();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header().Text("CRM Customer Report").SemiBold().FontSize(20);
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("Name");
                            header.Cell().Text("Email");
                            header.Cell().Text("City");
                            header.Cell().Text("Orders");
                        });
                        foreach (var c in customers)
                        {
                            table.Cell().Text(c.Name);
                            table.Cell().Text(c.Email ?? "-");
                            table.Cell().Text(c.City ?? "-");
                            table.Cell().Text(c.Orders?.Count.ToString() ?? "0");
                        }
                    });
                    page.Footer().AlignCenter().Text($"Generated on {DateTime.Now.ToShortDateString()}");
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return File(stream.ToArray(), "application/pdf", $"Customers_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}
