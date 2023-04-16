using Database;
using Domain.AuthenticationDomain;
using Domain.AuthenticationDomain.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DomainAssembly
{
    public static void ConfigureServices(IServiceCollection services)
    {
        var assembly = typeof(DomainAssembly).Assembly;

        services.AddAutoMapper(assembly);
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRegistrationService, RegistrationService>();

        DatabaseAssembly.ConfigureServices(services);
    }
}