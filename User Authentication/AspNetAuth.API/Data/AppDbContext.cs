using AspNetAuth.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetAuth.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}