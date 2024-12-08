using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DogsApp.Infrastructure.Data.Entities;

namespace DogsApp.Infrastructure.Data
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            var data = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedBreeds(data);

            return app;
        }

        public static void SeedBreeds(ApplicationDbContext data)
        {
            if (data.Breeds.Any())
            {
                return;
            }
            data.Breeds.AddRange(new[]
            {
                new Breed { Name = "Husky" },
                new Breed { Name = "Pinscher" },
                new Breed { Name = "Cocer spaniol" },
                new Breed { Name = "Dachshund" },
                new Breed { Name = "Doberman" },
                new Breed { Name = "Chihuahua" },
                new Breed { Name = "Rothweiler" },
            });
            data.SaveChanges();
        }
    }
}