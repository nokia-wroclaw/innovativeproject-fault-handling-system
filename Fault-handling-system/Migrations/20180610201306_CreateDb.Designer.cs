﻿// <auto-generated />
using Fault_handling_system.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Faulthandlingsystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180610201306_CreateDb")]
    partial class CreateDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Fault_handling_system.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Fault_handling_system.Models.EtrStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Status")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("EtrStatus");
                });

            modelBuilder.Entity("Fault_handling_system.Models.EtrType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("EtrType");
                });

            modelBuilder.Entity("Fault_handling_system.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AssignedTo");

                    b.Property<DateTime?>("ClosingDate");

                    b.Property<string>("Comment");

                    b.Property<DateTime>("DateIssued");

                    b.Property<DateTime?>("DateSent");

                    b.Property<string>("EtrDescription")
                        .IsRequired();

                    b.Property<string>("EtrNumber")
                        .IsRequired();

                    b.Property<int>("EtrStatusId");

                    b.Property<DateTime?>("EtrToDes");

                    b.Property<int>("EtrTypeId");

                    b.Property<int?>("Grade");

                    b.Property<long?>("NokiaCaseId");

                    b.Property<string>("NsnCoordinatorId");

                    b.Property<string>("Priority");

                    b.Property<string>("RequestorId")
                        .IsRequired();

                    b.Property<long>("RfaId");

                    b.Property<string>("RfaName");

                    b.Property<string>("SubcontractorId");

                    b.Property<string>("TroubleType");

                    b.Property<int>("ZoneId");

                    b.HasKey("Id");

                    b.HasIndex("EtrNumber")
                        .IsUnique();

                    b.HasIndex("EtrStatusId");

                    b.HasIndex("EtrTypeId");

                    b.HasIndex("NsnCoordinatorId");

                    b.HasIndex("RequestorId");

                    b.HasIndex("SubcontractorId");

                    b.HasIndex("ZoneId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("Fault_handling_system.Models.ReportFilter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DateIssuedFrom");

                    b.Property<int?>("DateIssuedFromDaysAgo");

                    b.Property<int?>("DateIssuedFromWeeksAgo");

                    b.Property<string>("DateIssuedTo");

                    b.Property<int?>("DateIssuedToDaysAgo");

                    b.Property<int?>("DateIssuedToWeeksAgo");

                    b.Property<string>("DateSentFrom");

                    b.Property<int?>("DateSentFromDaysAgo");

                    b.Property<int?>("DateSentFromWeeksAgo");

                    b.Property<string>("DateSentTo");

                    b.Property<int?>("DateSentToDaysAgo");

                    b.Property<int?>("DateSentToWeeksAgo");

                    b.Property<string>("EtrNumber");

                    b.Property<string>("EtrStatus");

                    b.Property<string>("EtrType");

                    b.Property<string>("Grade");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NsnCoordinatorId");

                    b.Property<string>("Priority");

                    b.Property<string>("RfaId");

                    b.Property<string>("RfaName");

                    b.Property<string>("SubcontractorId");

                    b.Property<string>("TroubleType");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<string>("Zone");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ReportFilter");
                });

            modelBuilder.Entity("Fault_handling_system.Models.ScheduleFilter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Cron");

                    b.Property<string>("DayOfWeek");

                    b.Property<int>("FilterId");

                    b.Property<string>("Hour");

                    b.Property<string>("Interval")
                        .IsRequired();

                    b.Property<string>("MailingList");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FilterId");

                    b.HasIndex("UserId");

                    b.ToTable("ScheduleFilter");
                });

            modelBuilder.Entity("Fault_handling_system.Models.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ZoneName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Zone");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Fault_handling_system.Models.Report", b =>
                {
                    b.HasOne("Fault_handling_system.Models.EtrStatus", "EtrStatus")
                        .WithMany()
                        .HasForeignKey("EtrStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fault_handling_system.Models.EtrType", "EtrType")
                        .WithMany()
                        .HasForeignKey("EtrTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fault_handling_system.Models.ApplicationUser", "NsnCoordinator")
                        .WithMany()
                        .HasForeignKey("NsnCoordinatorId");

                    b.HasOne("Fault_handling_system.Models.ApplicationUser", "Requestor")
                        .WithMany()
                        .HasForeignKey("RequestorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fault_handling_system.Models.ApplicationUser", "Subcontractor")
                        .WithMany()
                        .HasForeignKey("SubcontractorId");

                    b.HasOne("Fault_handling_system.Models.Zone", "Zone")
                        .WithMany()
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fault_handling_system.Models.ReportFilter", b =>
                {
                    b.HasOne("Fault_handling_system.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Fault_handling_system.Models.ScheduleFilter", b =>
                {
                    b.HasOne("Fault_handling_system.Models.ReportFilter", "Filter")
                        .WithMany()
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fault_handling_system.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Fault_handling_system.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Fault_handling_system.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Fault_handling_system.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Fault_handling_system.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
