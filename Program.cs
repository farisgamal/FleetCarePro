using Microsoft.AspNetCore.Identity;
using MVCDemo.Context;
using MVCDemo.Data;
using MVCDemo.Middleware;
using MVCDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Our Context doesn't take a ConnectionString from appsettings because OnConfiguring already hardcodes it
builder.Services.AddDbContext<ApplicationDbContext>();

// Module 1: Identity + Role Hierarchy
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Module 6: Global Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Module 6: Custom Middleware -> must run after UseRouting and before UseAuthentication so it can lock down the whole site
app.UseMaintenanceMode();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Module 1: Seed default roles + test users (Admin/FleetManager/Driver)
using (var scope = app.Services.CreateScope())
{
    await SeedData.SeedRolesAndUsersAsync(scope.ServiceProvider);
}

app.Run();
