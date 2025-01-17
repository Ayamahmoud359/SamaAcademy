using Academy.Data;
using Academy.Models;
using Academy.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Academy.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AcademyContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount =false)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Password settings
    options.Password.RequireDigit = false; // Requires at least one digit (0-9)
    options.Password.RequireLowercase = false; // Requires at least one lowercase letter (a-z)
    options.Password.RequireUppercase = false; // Requires at least one uppercase letter (A-Z)
    options.Password.RequireNonAlphanumeric = false; // Requires at least one special character
    options.Password.RequiredLength = 6; // Minimum length of the password
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use null to keep property names as is
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region Toast
builder.Services.AddRazorPages();
builder.Services.AddRazorPages().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = true,
    PreventDuplicates = true,
    CloseButton = true
});

//builder.Services.AddRazorPages().AddRazorRuntimeCompilation().AddNToastNotifyToastr(new ToastrOptions
//{
//    PositionClass = ToastPositions.TopRight,
//    ProgressBar = true,
//    TimeOut = 5000,
//    CloseButton = true,
//    PreventDuplicates = false,
//});
#endregion
var app = builder.Build();
System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Seed roles and admin user
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await DataSeeder.SeedRolesAndAdminUser(serviceProvider);
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseNToastNotify();

app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}"
);
app.MapGet("/", async context =>
{
   context.Response.Redirect("/admin");
});
app.Run();
