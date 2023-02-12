using Microsoft.EntityFrameworkCore;

using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryMapping> CategoryMappings { get; set; }

        public DbSet<Article> Articles { get; set; }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=UntitledArticlesDB;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.CreatedAtTime).HasColumnType("datetime");
                e.Property(p => p.Name).HasColumnType("nvarchar").HasMaxLength(150);
                e.HasMany(c => c.Articles).WithOne(a => a.Category);
                e.HasMany(c => c.CategoryMappings).WithOne(cm => cm.AncestorCategory)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Article>(e =>
            {
                e.ToTable("Articles");
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.CreatedAtTime).HasColumnType("datetime");
                e.Property(p => p.Content).HasColumnType("nvarchar").IsFixedLength(false);
                e.HasOne(a => a.Category).WithMany(c => c.Articles);
            });

            modelBuilder.Entity<CategoryMapping>(e =>
            {
                e.ToTable("CategoryMappings");
                e.HasKey(p => p.Id);
                e.HasOne(cm => cm.AncestorCategory).WithMany(c => c.CategoryMappings)
                .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
