using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UntitledArticles.API.Domain.Entities;

namespace UntitledSelfArticles.API.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<CategoryMapping> CategoryMappings { get; set; }

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    : base(options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=UntitledArticlesDB;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories")
                .HasKey(p => p.Id);

                e.Property(p => p.Name)
                .IsRequired();

                e.HasMany<Article>(a => a.Articles)
                .WithOne(c => c.Category);

                e.HasMany<CategoryMapping>(a => a.CategoryMappings)
                .WithOne(p => p.AncestorCategory);
            });

            modelBuilder.Entity<Article>(e =>
            {
                e.ToTable("Articles")
                .HasKey(p => p.Id);

                e.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(60);

                e.Property(p => p.Content).IsRequired();

                e.HasOne<Category>(p => p.Category)
                .WithMany(p => p.Articles);
            });

            modelBuilder.Entity<CategoryMapping>(e =>
            {
                e.ToTable("CategoryMappings");

                e.HasOne<Category>(p => p.AncestorCategory)
                .WithMany(p => p.CategoryMappings)
                .HasForeignKey(p => p.AncestoryCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

                //e.HasOne<Category>(p => p.Category)
                //.WithMany(p => p.CategoryMappings)
                //.HasForeignKey(p => p.CategoryId)
                //.OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
