﻿// <auto-generated />
using System;
using ImageImporter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageImporter.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220722172626_PictureInfo")]
    partial class PictureInfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.15");

            modelBuilder.Entity("ImageImporter.Models.Db.ActionItems.ActionItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ActionItemId")
                        .HasColumnType("int");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("JobResultId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActionItemId");

                    b.HasIndex("JobResultId");

                    b.ToTable("ActionItems");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ActionItem");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.JobResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("Started")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("JobResults");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("File")
                        .HasColumnType("text");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<long?>("Hash")
                        .HasColumnType("bigint");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Quality")
                        .HasColumnType("int");

                    b.Property<string>("Thumbnail")
                        .HasColumnType("text");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(85)
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(85)
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("RoleId")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("Name")
                        .HasMaxLength(85)
                        .HasColumnType("varchar(85)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.ActionItems.PictureFoundItem", b =>
                {
                    b.HasBaseType("ImageImporter.Models.Db.ActionItems.ActionItem");

                    b.Property<int?>("PictureId")
                        .HasColumnType("int");

                    b.HasIndex("PictureId");

                    b.HasDiscriminator().HasValue("PictureFoundItem");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.ActionItems.PictureRemovedItem", b =>
                {
                    b.HasBaseType("ImageImporter.Models.Db.ActionItems.ActionItem");

                    b.Property<int?>("PictureId")
                        .HasColumnType("int")
                        .HasColumnName("PictureRemovedItem_PictureId");

                    b.HasIndex("PictureId");

                    b.HasDiscriminator().HasValue("PictureRemovedItem");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.ActionItems.ActionItem", b =>
                {
                    b.HasOne("ImageImporter.Models.Db.ActionItems.ActionItem", null)
                        .WithMany("ActionsLog")
                        .HasForeignKey("ActionItemId");

                    b.HasOne("ImageImporter.Models.Db.JobResult", null)
                        .WithMany("ActionsLog")
                        .HasForeignKey("JobResultId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ImageImporter.Models.Db.ActionItems.PictureFoundItem", b =>
                {
                    b.HasOne("ImageImporter.Models.Db.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.ActionItems.PictureRemovedItem", b =>
                {
                    b.HasOne("ImageImporter.Models.Db.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.ActionItems.ActionItem", b =>
                {
                    b.Navigation("ActionsLog");
                });

            modelBuilder.Entity("ImageImporter.Models.Db.JobResult", b =>
                {
                    b.Navigation("ActionsLog");
                });
#pragma warning restore 612, 618
        }
    }
}
