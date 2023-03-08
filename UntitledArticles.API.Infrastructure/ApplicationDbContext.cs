﻿using Microsoft.EntityFrameworkCore;

using UntitledArticles.API.Domain.Entities;

namespace UntitledArticles.API.Infrastructure
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

        /// <summary>
        /// .ctor
        /// </summary>
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        ///// <inheritdoc />
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=untitled_articles_mssql;Database=UnitledArticlesDB;User=sa;Password=Str0ngPa$$w0rd;Trust Server Certificate=true");
        //}

        /// <inheritdoc />
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
                e.HasOne(c => c.Parent)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
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
        }
    }
}
