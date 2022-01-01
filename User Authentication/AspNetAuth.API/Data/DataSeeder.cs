using System;
using System.Threading.Tasks;
using AspNetAuth.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetAuth.API.Data
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        private async Task SeedDefaultAdminUser()
        {
            var adminUserExist = await _context.Users.AnyAsync(x => x.Role == Role.Admin);

            if (!adminUserExist)
            {
                var adminUser = new User
                {
                    DisplayName = "Admin",
                    Email = "admin@domain.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Role = Role.Admin
                };

                _context.Add(adminUser);
                await _context.SaveChangesAsync();
            }
        }

        public static void Run(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var seeder = new DataSeeder(appDbContext);

            seeder.SeedDefaultAdminUser().Wait();
        }
    }
}