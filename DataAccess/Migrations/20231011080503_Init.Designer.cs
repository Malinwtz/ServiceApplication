﻿// <auto-generated />
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231011080503_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("DataAccess.Models.Entities.AppSettings", b =>
                {
                    b.Property<string>("ConnectionString")
                        .HasColumnType("TEXT");

                    b.HasKey("ConnectionString");

                    b.ToTable("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
