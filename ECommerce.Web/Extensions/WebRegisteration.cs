using ECommerce.Domin.Contract.DataSeeding;
using ECommerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Extensions
{
    public static class WebRegisteration
    {
        public static async Task<WebApplication> MigrateDbAsync(this WebApplication app)
        {

            await using var scope = app.Services.CreateAsyncScope();
            var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            var pendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();


            if (pendingMigrations.Any())
                await dbContextService.Database.MigrateAsync();

            return app;
        }

        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dataIniService = scope.ServiceProvider.GetRequiredService<IDataini>();
            await dataIniService.InitilizeAsync();
            return app;
        }
    }
}
