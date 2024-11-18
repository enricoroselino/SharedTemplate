namespace Shared.Persistence.Extensions;

public static class DatabaseExtensions
{
    public static async Task MigrateDatabaseAsync<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        await context.Database.MigrateAsync();
    }

    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeders = scope.ServiceProvider
            .GetServices<IDataSeeder>()
            .ToList();

        if (seeders.Count == 0) return;

        var seeder = seeders.Select(x => x.SeedAsync());
        await Task.WhenAll(seeder);
    }
}