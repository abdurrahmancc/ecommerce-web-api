

using Ecommerce_web_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_web_api.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options): base(options){}
        public DbSet<Category>Categories{get; set;}
    }
}
