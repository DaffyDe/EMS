using EMS.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services

builder.Services.AddControllersWithViews();

// Configure EF Core with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=school.db"));


// Enable Session

builder.Services.AddDistributedMemoryCache(); // In-memory cache for session

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;                // Prevent JS access
    options.Cookie.IsEssential = true;             // Required for GDPR
});

var app = builder.Build();


// Configure Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();       // <-- MUST come before UseAuthorization
app.UseAuthorization();

// Default Route

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // start at login

app.Run();
