using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class DebugReportModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusStatus",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrType_EtrTypeType",
                table: "Report");

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

            migrationBuilder.AddColumn<string>(
                name: "EtrStatusName",
                table: "Report",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EtrTypeName",
                table: "Report",
                nullable: false,
                defaultValue: "");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusStatus",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrType_EtrTypeType",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "EtrStatusName",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "EtrTypeName",
                table: "Report");

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
    }
}
