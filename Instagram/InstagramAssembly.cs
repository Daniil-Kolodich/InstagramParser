using Instagram.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Instagram;

public static class InstagramAssembly
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IInstagramManager, FakeInstagramManager>();
    }
}