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
    .AddRoles<IdentityRole>()
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


using (var scope = app.Services.CreateScope())
{
    await SeedRolesAndAdminAsync(scope.ServiceProvider);
}

app.Run();



// ---------- Helper method ----------
async Task SeedRolesAndAdminAsync(IServiceProvider services)
{
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

    string[] roleNames = { "Admin", "Student" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    string adminEmail = "admin@school.com";
    string adminPassword = "Admin@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Admin creation error: {error.Description}");
            }
        }
    }
}

