using Microsoft.EntityFrameworkCore;

using AnSumeri.API.Domain.Entities;

namespace AnSumeri.API.Infrastructure
{
    /// <summary>
    /// DB context for UntitledArticles DB
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Categories Table
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Articles Table
        /// </summary>
        public DbSet<Article> Articles { get; set; }

        // /// <summary>
        // /// .ctor
        // /// </summary>
        // public ApplicationDbContext()
        // {
        // }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer("Server=db;Database=UnitledArticlesDB;User=sa;Password=S3cur3P@ssW0rd!;Trust Server Certificate=true");
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
                e.HasKey(p => new { p.Id, p.UserId });
                e.Property(p => p.UserId).HasColumnType("nvarchar").HasMaxLength(37);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.CreatedAtTime).HasColumnType("datetime");
                e.Property(p => p.Name).HasColumnType("nvarchar").HasMaxLength(150);
                e.HasMany(c => c.Articles).WithOne(a => a.Category);
                e.HasOne(c => c.Parent)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => new { c.ParentId, c.UserId })
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Article>(e =>
            {
                e.ToTable("Articles");
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.UserId).HasColumnType("nvarchar").HasMaxLength(37);
                e.Property(p => p.CreatedAtTime).HasColumnType("datetime");
                e.Property(p => p.Content).HasColumnType("nvarchar(max)");
                e.Property(p => p.Title).HasColumnType("nvarchar").HasMaxLength(80);
                e.HasOne(a => a.Category).WithMany(c => c.Articles).HasForeignKey(c =>
                    new { c.CategoryId, c.UserId }).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
