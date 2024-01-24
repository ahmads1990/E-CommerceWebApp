using E_CommerceWebApp.Models.Email;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Configure user settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDBContext>();

//DP
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddSingleton<ImageService>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();

builder.Services.AddScoped<ICartRepo, CartRepo>();

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<EmailServerSettings>(builder.Configuration.GetSection("EmailServerSettings"));

// Security
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(CustomPolicies.canAccessAdmin,
        policyBuilder => policyBuilder.RequireClaim(CustomClaims.IsAdmin));
});

// Third party login
// Google
builder.AddGoogleLoginProvider();


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
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

await app.AddSeedDefaultUser();
app.Run();
