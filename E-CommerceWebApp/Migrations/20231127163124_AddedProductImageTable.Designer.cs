﻿// <auto-generated />
using System;
using E_CommerceWebApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace E_CommerceWebApp.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20231127163124_AddedProductImageTable")]
    partial class AddedProductImageTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("E_CommerceWebApp.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int?>("ProductImageID")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductID");

                    b.HasIndex("ProductImageID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductID = 1,
                            Description = "Fancy",
                            Price = 40f,
                            ProductName = "Fancy Product"
                        },
                        new
                        {
                            ProductID = 2,
                            Description = "Special",
                            Price = 80f,
                            ProductName = "Special Item"
                        },
                        new
                        {
                            ProductID = 3,
                            Description = "Sale",
                            Price = 20f,
                            ProductName = "Sale Item"
                        },
                        new
                        {
                            ProductID = 4,
                            Description = "Popular",
                            Price = 100f,
                            ProductName = "Popular Item"
                        });
                });

            modelBuilder.Entity("E_CommerceWebApp.Models.ProductImage", b =>
                {
                    b.Property<int>("ProductImageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductImageID"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ProductImageID");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("E_CommerceWebApp.Models.Product", b =>
                {
                    b.HasOne("E_CommerceWebApp.Models.ProductImage", "ProductImage")
                        .WithMany()
                        .HasForeignKey("ProductImageID");

                    b.Navigation("ProductImage");
                });
#pragma warning restore 612, 618
        }
    }
}