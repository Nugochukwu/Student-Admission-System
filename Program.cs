using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Student_Admission_System.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Register EF Core + SQL Server Dependency Injection
builder.Services.AddDbContext<ApplicationDbContext2>(options => options.UseSqlServer(
	builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
	options.SignIn.RequireConfirmedAccount = false) // if email confirmation
	.AddEntityFrameworkStores<ApplicationDbContext2>();


// MVC + API support
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Authorization and Authentication.
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

//Identity Ui
app.MapRazorPages();

// API routes
app.MapControllers();



app.Run();
