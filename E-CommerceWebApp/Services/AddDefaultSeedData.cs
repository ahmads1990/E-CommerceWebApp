using System.Security.Claims;

namespace E_CommerceWebApp.Services
{
    public static class AddDefaultSeedData
    {
        public static async Task AddSeedDefaultUser(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Check if the default user already exists
                var defaultAdminUser = await userManager.FindByNameAsync("DefaultAdmin");

                if (defaultAdminUser == null)
                {
                    // Create a new user with the default username and password
                    defaultAdminUser = new ApplicationUser
                    {
                        UserName = "admin@managment.com",
                        NormalizedUserName = "ADMIN@MANAGMENT.com",
                        Email = "admin@managment.com",
                        NormalizedEmail = "ADMIN@MANAGMENT.com",
                        EmailConfirmed = true,
                        PhoneNumber = "1234567890",
                        FirstName = "Admin",
                        LastName = "Admin"
                    };
                    string adminPassword = "AdminPassword99*";
                    var result = await userManager.CreateAsync(defaultAdminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        var claimResult = await userManager.AddClaimAsync(defaultAdminUser, new Claim(CustomClaims.IsAdmin, "admin"));
                        
                        if (claimResult.Succeeded)                       
                            Console.WriteLine("Claim 'IsAdmin' added to the default user.");
                        
                        else
                            Console.WriteLine("Error adding claim to the default user.");
                    }
                    else
                        Console.WriteLine("Error adding default user");
                }
            }
        }
    }
}