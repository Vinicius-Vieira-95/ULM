﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UlmApi.Infra.Data;

namespace UlmApi.Infra.Data.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20220103145627_RemoveUserTable")]
    partial class RemoveUserTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("UlmApi.Domain.Entities.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("APPLICATION");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.License", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ApplicationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("AquisitionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Archived")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Justification")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Label")
                        .HasColumnType("text");

                    b.Property<double?>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<int>("SolutionId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("SolutionId");

                    b.ToTable("LICENSE");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PRODUCT");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.RequestLicense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ApplicationId")
                        .HasColumnType("integer");

                    b.Property<string>("Justification")
                        .HasColumnType("text");

                    b.Property<string>("JustificationForDeny")
                        .HasColumnType("text");

                    b.Property<int?>("LicenseId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("RequesterId")
                        .HasColumnType("text");

                    b.Property<string>("RequesterName")
                        .HasColumnType("text");

                    b.Property<int>("SolutionId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UsageTime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.HasIndex("LicenseId")
                        .IsUnique();

                    b.HasIndex("ProductId");

                    b.HasIndex("SolutionId");

                    b.ToTable("REQUEST_LICENSE");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.Solution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OwnerName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("SOLUTION");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.License", b =>
                {
                    b.HasOne("UlmApi.Domain.Entities.Application", "Application")
                        .WithMany("Licenses")
                        .HasForeignKey("ApplicationId");

                    b.HasOne("UlmApi.Domain.Entities.Solution", "Solution")
                        .WithMany("Licenses")
                        .HasForeignKey("SolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Solution");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.RequestLicense", b =>
                {
                    b.HasOne("UlmApi.Domain.Entities.Application", "Application")
                        .WithMany("RequestLicenses")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UlmApi.Domain.Entities.License", "License")
                        .WithOne("Request")
                        .HasForeignKey("UlmApi.Domain.Entities.RequestLicense", "LicenseId");

                    b.HasOne("UlmApi.Domain.Entities.Product", "Product")
                        .WithMany("RequestLicenses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UlmApi.Domain.Entities.Solution", "Solution")
                        .WithMany("RequestLicenses")
                        .HasForeignKey("SolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("License");

                    b.Navigation("Product");

                    b.Navigation("Solution");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.Solution", b =>
                {
                    b.HasOne("UlmApi.Domain.Entities.Product", "Product")
                        .WithMany("Solutions")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.Application", b =>
                {
                    b.Navigation("Licenses");

                    b.Navigation("RequestLicenses");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.License", b =>
                {
                    b.Navigation("Request");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.Product", b =>
                {
                    b.Navigation("RequestLicenses");

                    b.Navigation("Solutions");
                });

            modelBuilder.Entity("UlmApi.Domain.Entities.Solution", b =>
                {
                    b.Navigation("Licenses");

                    b.Navigation("RequestLicenses");
                });
#pragma warning restore 612, 618
        }
    }
}
