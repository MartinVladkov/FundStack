using FundStack.Data;
using Microsoft.EntityFrameworkCore;

namespace FundStack.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<FundStackDbContext>();

            data.Database.Migrate();

            SeedTypes(data);

            return app;
        }

        private static void SeedTypes(FundStackDbContext data)
        {
            if (data.Types.Any())
            {
                return;
            }

            data.Types.AddRange(new[]
            {
                new Data.Models.Type { Name = "Crypto" },
                new Data.Models.Type { Name = "Stock"},
                new Data.Models.Type { Name = "ETF"},
            });

            data.SaveChanges();
        }
    }
}
