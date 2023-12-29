using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace E_CommerceWebApp.Services
{
    public static class AddLoginProviderServices
    {
        public static void AddGoogleLoginProvider(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
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

                        // start request using google people service to get more info about the user (birthdate, gender)
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

                        // extracted values from response
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
        }
    }
}
