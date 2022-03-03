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



            return app;
        }
    }
}
