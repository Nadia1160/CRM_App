using CRM_APP.Data;
using CRM_APP.Models;
using Microsoft.EntityFrameworkCore;
using QuestPDF;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// **** FIX #1: PDF Generation Configuration Failure ****
// Without this line, QuestPDF throws an exception.
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Ensure database is created (for demo, we'll create automatically)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated(); // creates the SQLite database and tables
                                        // Seed data (optional)
    if (!dbContext.Customers.Any())
    {
        var customer = new Customer { Name = "Acme Corp", Email = "contact@acme.com", City = "New York" };
        dbContext.Customers.Add(customer);
        dbContext.Orders.Add(new Order
        {
            OrderNumber = "ORD-001",
            Quantity = 5,
            UnitPrice = 100,
            TotalPrice = 500,
            Customer = customer
        });
        dbContext.SaveChanges();
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();