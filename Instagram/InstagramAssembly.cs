using Instagram.Concrete;
using Instagram.Concrete.Development;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram;

public static class InstagramAssembly
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUserManager, FakeUserManager>();
        services.AddScoped<IFollowingsManager, FakeFollowingsManager>();
        services.AddScoped<IFollowersManager, FakeFollowersManager>();
        
        services.AddScoped<ILamavadaClient, LamavadaClient>();
        
        services.AddHttpClient();
    }
}