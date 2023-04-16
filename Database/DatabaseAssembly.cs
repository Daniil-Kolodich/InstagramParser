using Database.Context;
using Database.Repositories;
using Database.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database;

public static class DatabaseAssembly
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<InstagramContext>(o =>
            o.UseMySQL("server=localhost;database=instagram;user=instagram;password=1234"));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
    }
}