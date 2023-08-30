﻿// <auto-generated />
using System;
using AnSumeri.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AnSumeri.API.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230829203057_SetContentToBeMaxNVarChar")]
    partial class SetContentToBeMaxNVarChar
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AnSumeri.API.Domain.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAtTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(37)
                        .HasColumnType("nvarchar");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId", "UserId");

                    b.ToTable("Articles", (string)null);
                });

            modelBuilder.Entity("AnSumeri.API.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("UserId")
                        .HasMaxLength(37)
                        .HasColumnType("nvarchar");

                    b.Property<DateTime>("CreatedAtTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id", "UserId");

                    b.HasIndex("ParentId", "UserId");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("AnSumeri.API.Domain.Entities.Article", b =>
                {
                    b.HasOne("AnSumeri.API.Domain.Entities.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("AnSumeri.API.Domain.Entities.Category", b =>
                {
                    b.HasOne("AnSumeri.API.Domain.Entities.Category", "Parent")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentId", "UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("AnSumeri.API.Domain.Entities.Category", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("SubCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
