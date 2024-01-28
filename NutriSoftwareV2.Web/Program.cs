using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriSoftwareV2.Web.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(6);
    options.LoginPath = "/Account/Login";
    options.SlidingExpiration = true;
});

builder.Services.AddDbContext<NutriSoftwareV2.Web.Identity.ApplicationDbContext>(opt =>
{
    opt.UseMySQL(builder.Configuration.GetConnectionString("IdentityConnection"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(6);
    options.Lockout.MaxFailedAccessAttempts = 5;

})
.AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Account}/{action=Login}");

app.Run();
