﻿// <auto-generated />
using System;
using FeatureToggle.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FeatureToggle.Infrastructure.Migrations
{
    [DbContext(typeof(FeatureToggleContext))]
    partial class FeatureToggleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FeatureToggle.Domain.Entities.Feature", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DbId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NewId()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProductDbId")
                        .HasColumnType("int");

                    b.HasKey("DbId");

                    b.HasIndex("ProductDbId");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("FeatureToggle.Domain.Entities.FeatureState", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DbId"), 1L, 1);

                    b.Property<int>("Environment")
                        .HasColumnType("int");

                    b.Property<int>("FeatureDbId")
                        .HasColumnType("int");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NewId()");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("DbId");

                    b.HasIndex("FeatureDbId");

                    b.ToTable("FeatureStates");
                });

            modelBuilder.Entity("FeatureToggle.Domain.Entities.Product", b =>
                {
                    b.Property<int>("DbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DbId"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NewId()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("DbId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("FeatureToggle.Domain.Entities.Feature", b =>
                {
                    b.HasOne("FeatureToggle.Domain.Entities.Product", "Product")
                        .WithMany("Features")
                        .HasForeignKey("ProductDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("FeatureToggle.Domain.Entities.FeatureState", b =>
                {
                    b.HasOne("FeatureToggle.Domain.Entities.Feature", "Feature")
                        .WithMany("FeatureStates")
                        .HasForeignKey("FeatureDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("FeatureToggle.Domain.Entities.Feature", b =>
                {
                    b.Navigation("FeatureStates");
                });

            modelBuilder.Entity("FeatureToggle.Domain.Entities.Product", b =>
                {
                    b.Navigation("Features");
                });
#pragma warning restore 612, 618
        }
    }
}
