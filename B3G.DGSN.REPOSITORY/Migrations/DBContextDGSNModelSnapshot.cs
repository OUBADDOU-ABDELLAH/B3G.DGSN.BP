﻿// <auto-generated />
using B3G.DGSN.REPOSITORY;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace B3G.DGSN.REPOSITORY.Migrations
{
    [DbContext(typeof(DBContextDGSN))]
    partial class DBContextDGSNModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("B3G.DGSN.DOMAIN.Session", b =>
                {
                    b.Property<string>("state")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("state");

                    b.ToTable("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}