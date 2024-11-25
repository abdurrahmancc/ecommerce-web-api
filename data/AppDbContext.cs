

using Ecommerce_web_api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_web_api.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FilesUploadModel> FilesUploads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Blog>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.Blog)
                .HasForeignKey(e => e.BlogId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
