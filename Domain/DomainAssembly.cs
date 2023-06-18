using Database;
using Database.Entities;
using Domain.AuthenticationDomain;
using Domain.AuthenticationDomain.Concrete;
using Domain.InstagramAccountDomain;
using Domain.InstagramAccountDomain.Concrete;
using Domain.ParsingDomain;
using Domain.ParsingDomain.Concrete;
using Domain.SubscriptionDomain;
using Domain.SubscriptionDomain.Concrete;
using Domain.UsersDomain;
using Domain.UsersDomain.Concrete;
using Instagram;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DomainAssembly
{
    public static void ConfigureServices(IServiceCollection services)
    {
        var assembly = typeof(DomainAssembly).Assembly;

        // Authentication
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRegistrationService, RegistrationService>();

        // Subscription
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        
        // InstagramAccount
        services.AddScoped<IInstagramAccountService, InstagramAccountService>();
        
        // Parsing
        services.AddScoped<IParsingService, ParsingService>();
        
        // User
        services.AddScoped<IUserService, UserService>();
        
        
        DatabaseAssembly.ConfigureServices(services);
        InstagramAssembly.ConfigureServices(services);
    }
}