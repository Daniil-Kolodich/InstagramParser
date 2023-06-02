using Instagram.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram;

public static class InstagramAssembly
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IFollowersManager, ILamavadaClient>();
        services.AddScoped<IFollowingsManager, ILamavadaClient>();
        services.AddScoped<IInstagramManager, ILamavadaClient>();
    }
}