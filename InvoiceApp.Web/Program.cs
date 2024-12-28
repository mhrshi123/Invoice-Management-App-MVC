using Microsoft.EntityFrameworkCore;
using InvoiceApp.DataAccess.Entities;
using InvoiceApp.DataAccess.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("InvoiceAppDb");
builder.Services.AddDbContext<InvoiceAppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IInvoiceManagerService, InvoiceManagerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
