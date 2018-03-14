using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class FirstReportPrototypeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EtrStatus",
                columns: table => new
                {
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtrStatus", x => x.Status);
                });

            migrationBuilder.CreateTable(
                name: "EtrType",
                columns: table => new
                {
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtrType", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "TroubleType",
                columns: table => new
                {
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TroubleType", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ZoneName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssignedTo = table.Column<string>(nullable: true),
                    ClosingDate = table.Column<DateTime>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    DateIssued = table.Column<DateTime>(nullable: false),
                    DateSent = table.Column<DateTime>(nullable: true),
                    EtrDescription = table.Column<string>(nullable: true),
                    EtrNumber = table.Column<string>(nullable: true),
                    EtrStatusStatus = table.Column<string>(nullable: true),
                    EtrToDes = table.Column<DateTime>(nullable: true),
                    EtrTypeType = table.Column<string>(nullable: true),
                    Grade = table.Column<int>(nullable: false),
                    NokiaCaseId = table.Column<int>(nullable: true),
                    NsnCoordinatorId = table.Column<string>(nullable: true),
                    Priority = table.Column<string>(nullable: true),
                    RequestorId = table.Column<string>(nullable: false),
                    RfaId = table.Column<int>(nullable: false),
                    RfaName = table.Column<string>(nullable: true),
                    SubcontractorId = table.Column<string>(nullable: true),
                    TroubleTypeType = table.Column<string>(nullable: true),
                    ZoneId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_EtrStatus_EtrStatusStatus",
                        column: x => x.EtrStatusStatus,
                        principalTable: "EtrStatus",
                        principalColumn: "Status",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_EtrType_EtrTypeType",
                        column: x => x.EtrTypeType,
                        principalTable: "EtrType",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_NsnCoordinatorId",
                        column: x => x.NsnCoordinatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_AspNetUsers_SubcontractorId",
                        column: x => x.SubcontractorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_TroubleType_TroubleTypeType",
                        column: x => x.TroubleTypeType,
                        principalTable: "TroubleType",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrStatusStatus",
                table: "Report",
                column: "EtrStatusStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrTypeType",
                table: "Report",
                column: "EtrTypeType");

            migrationBuilder.CreateIndex(
                name: "IX_Report_NsnCoordinatorId",
                table: "Report",
                column: "NsnCoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_RequestorId",
                table: "Report",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_SubcontractorId",
                table: "Report",
                column: "SubcontractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_TroubleTypeType",
                table: "Report",
                column: "TroubleTypeType");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ZoneId",
                table: "Report",
                column: "ZoneId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "EtrStatus");

            migrationBuilder.DropTable(
                name: "EtrType");

            migrationBuilder.DropTable(
                name: "TroubleType");

            migrationBuilder.DropTable(
                name: "Zone");
        }
    }
}
