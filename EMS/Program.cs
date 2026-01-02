using EMS.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =======================
// SERVICES
// =======================

builder.Services.AddControllersWithViews();

// 🔹 Environment-safe SQLite connection
string connectionString;

if (builder.Environment.IsDevelopment())
{
    // LOCAL (Visual Studio)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
}
else
{
    // AZURE (Writable directory)
    var dbPath = Path.Combine(
        Environment.GetEnvironmentVariable("HOME")!,
        "site",
        "wwwroot",
        "school.db"
    );

    connectionString = $"Data Source={dbPath}";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString)
);

// SESSION
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// =======================
// PIPELINE
// =======================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

// =======================
// DB INITIALIZATION
// =======================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated(); // Creates DB & tables if missing
}

app.Run();
