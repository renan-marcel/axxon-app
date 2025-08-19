using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProdAbs.Infrastructure.Data;
public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;
        var environment = sp.GetRequiredService<IHostEnvironment>();

        if (environment.IsProduction()) return;

        var appDbContextFactory = sp.GetRequiredService<IDbContextFactory<AppDbContext>>();
        await using var appDbContext = await appDbContextFactory.CreateDbContextAsync();
        var connectionString = appDbContext.Database.GetConnectionString();
        if ((await appDbContext.Database.GetPendingMigrationsAsync()).Any())
            await appDbContext.Database.MigrateAsync();
    }
}