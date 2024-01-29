using GestaoPresencasMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GestaoPresencasMVC.Data;
using GestaoPresencasMVC.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddViewComponentsAsServices();
builder.Services.AddRazorPages();


builder.Services.AddDbContext<TentativaDb4Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("constring")));
builder.Services.AddDbContext<gpContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("gpContextConnection")));

builder.Services.AddDefaultIdentity<gpUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<gpContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Disable other password requirements
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;

    // Set the minimum length if needed
    options.Password.RequiredLength = 1; // Set your desired minimum length
});


builder.Services.AddControllers().AddJsonOptions(options => { 
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

//builder.Services.AddAuthentication("YourAuthenticationScheme")
//    .AddCookie("YourAuthenticationScheme", options =>
//    {
//        // Cookie options
//    });

builder.Services.AddHttpClient();

//builder.Services.AddScoped<SecurityStampValidator<gpUser>, CustomSecurityStampValidator>();


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

// Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // ... other endpoints if needed
});

app.MapRazorPages();

app.Run();
