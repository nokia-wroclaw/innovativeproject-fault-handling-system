using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class AdjustedReportModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusStatus",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrType_EtrTypeType",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_TroubleType_TroubleTypeType",
                table: "Report");

            migrationBuilder.DropTable(
                name: "TroubleType");

            migrationBuilder.DropIndex(
                name: "IX_Report_TroubleTypeType",
                table: "Report");

            migrationBuilder.RenameColumn(
                name: "TroubleTypeType",
                table: "Report",
                newName: "TroubleType");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "Report",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "EtrTypeType",
                table: "Report",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EtrStatusStatus",
                table: "Report",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EtrDescription",
                table: "Report",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TroubleType",
                table: "Report",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusStatus",
                table: "Report",
                column: "EtrStatusStatus",
                principalTable: "EtrStatus",
                principalColumn: "Status",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrType_EtrTypeType",
                table: "Report",
                column: "EtrTypeType",
                principalTable: "EtrType",
                principalColumn: "Type",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusStatus",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrType_EtrTypeType",
                table: "Report");

            migrationBuilder.RenameColumn(
                name: "TroubleType",
                table: "Report",
                newName: "TroubleTypeType");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "Report",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EtrTypeType",
                table: "Report",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EtrStatusStatus",
                table: "Report",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EtrDescription",
                table: "Report",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "TroubleTypeType",
                table: "Report",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Report_TroubleTypeType",
                table: "Report",
                column: "TroubleTypeType");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusStatus",
                table: "Report",
                column: "EtrStatusStatus",
                principalTable: "EtrStatus",
                principalColumn: "Status",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrType_EtrTypeType",
                table: "Report",
                column: "EtrTypeType",
                principalTable: "EtrType",
                principalColumn: "Type",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_TroubleType_TroubleTypeType",
                table: "Report",
                column: "TroubleTypeType",
                principalTable: "TroubleType",
                principalColumn: "Type",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
