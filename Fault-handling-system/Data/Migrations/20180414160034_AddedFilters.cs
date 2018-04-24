using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class AddedFilters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportFilter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateIssuedFrom = table.Column<string>(nullable: true),
                    DateIssuedFromDaysAgo = table.Column<int>(nullable: true),
                    DateIssuedFromWeeksAgo = table.Column<int>(nullable: true),
                    DateIssuedTo = table.Column<string>(nullable: true),
                    DateIssuedToDaysAgo = table.Column<int>(nullable: true),
                    DateIssuedToWeeksAgo = table.Column<int>(nullable: true),
                    DateSentFrom = table.Column<string>(nullable: true),
                    DateSentFromDaysAgo = table.Column<int>(nullable: true),
                    DateSentFromWeeksAgo = table.Column<int>(nullable: true),
                    DateSentTo = table.Column<string>(nullable: true),
                    DateSentToDaysAgo = table.Column<int>(nullable: true),
                    DateSentToWeeksAgo = table.Column<int>(nullable: true),
                    EtrNumber = table.Column<string>(nullable: true),
                    EtrStatus = table.Column<string>(nullable: true),
                    EtrType = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NsnCoordinatorId = table.Column<string>(nullable: true),
                    Priority = table.Column<string>(nullable: true),
                    RfaId = table.Column<string>(nullable: true),
                    RfaName = table.Column<string>(nullable: true),
                    SubcontractorId = table.Column<string>(nullable: true),
                    TroubleType = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    Zone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFilter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportFilter_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportFilter_UserId",
                table: "ReportFilter",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportFilter");
        }
    }
}
