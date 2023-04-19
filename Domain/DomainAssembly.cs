using Database;
using Database.Entities;
using Domain.AuthenticationDomain;
using Domain.AuthenticationDomain.Concrete;
using Domain.InstagramAccountDomain;
using Domain.InstagramAccountDomain.Concrete;
using Domain.SubscriptionDomain;
using Domain.SubscriptionDomain.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DomainAssembly
{
    public static void ConfigureServices(IServiceCollection services)
    {
        var assembly = typeof(DomainAssembly).Assembly;

        services.AddAutoMapper(assembly);
        
        // Authentication
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRegistrationService, RegistrationService>();

        // Subscription
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        
        // InstagramAccount
        services.AddScoped<IInstagramAccountService, InstagramAccountService>();
        
        DatabaseAssembly.ConfigureServices(services);
    }
}