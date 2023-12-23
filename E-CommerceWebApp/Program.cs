using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using E_CommerceWebApp.Services;
using E_CommerceWebApp.Models.Email;
using System.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Security.Claims;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1.Data;

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

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    // scope for birthday
    googleOptions.Scope.Add("https://www.googleapis.com/auth/user.birthday.read");
    googleOptions.Scope.Add("https://www.googleapis.com/auth/user.gender.read");
    googleOptions.Scope.Add("https://www.googleapis.com/auth/user.phonenumbers.read");
    googleOptions.SaveTokens = true;

    googleOptions.Events.OnCreatingTicket = ctx =>
    {
        List<AuthenticationToken> tokens = ctx.Properties.GetTokens().ToList();

        // start requset using google people service to get more info about the user (birthdate, gender)
        // create people service with GoogleCredentials
        var cred = GoogleCredential.FromAccessToken(tokens.First().Value);
        var peopleServiceService = new PeopleServiceService(
            new BaseClientService.Initializer() { HttpClientInitializer = cred });

        // use people/me to access data for the current authenticated user 
        var personRequest = peopleServiceService.People.Get("people/me");
        // add requested fields
        personRequest.PersonFields = "birthdays,genders";
        // send request
        Person person = personRequest.Execute();

        // extracte values from response
        var dateResponse = person.Birthdays.FirstOrDefault()?.Date;
        DateTime? birthdateValue = null;
        if (dateResponse.Day != null && dateResponse.Month != null && dateResponse.Year != null)
            birthdateValue = new DateTime(dateResponse?.Year ?? 1, dateResponse?.Month ?? 1, dateResponse?.Day ?? 1);

        var genderValue = person.Genders?.FirstOrDefault()?.FormattedValue;

        // get user identity to add the claims too
        var claimsIdentity = ctx.Principal?.Identities.First();
        // create claims
        claimsIdentity?.AddClaim(new Claim(ClaimTypes.Gender, genderValue ?? ""));
        claimsIdentity?.AddClaim(new Claim(ClaimTypes.DateOfBirth, birthdateValue?.ToString() ?? "", ClaimValueTypes.DateTime));

        tokens.Add(new AuthenticationToken()
        {
            Name = "TicketCreated",
            Value = DateTime.UtcNow.ToString()
        });
        ctx.Properties.StoreTokens(tokens);
        return Task.CompletedTask;
    };
});

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
