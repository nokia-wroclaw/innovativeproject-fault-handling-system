using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class FixForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusId1",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrType_EtrTypeId1",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_EtrStatusId1",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_EtrTypeId1",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "EtrStatusId1",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "EtrTypeId1",
                table: "Report");

            migrationBuilder.AlterColumn<int>(
                name: "EtrTypeId",
                table: "Report",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "EtrStatusId",
                table: "Report",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrStatusId",
                table: "Report",
                column: "EtrStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrTypeId",
                table: "Report",
                column: "EtrTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusId",
                table: "Report",
                column: "EtrStatusId",
                principalTable: "EtrStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrType_EtrTypeId",
                table: "Report",
                column: "EtrTypeId",
                principalTable: "EtrType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrType_EtrTypeId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_EtrStatusId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_EtrTypeId",
                table: "Report");

            migrationBuilder.AlterColumn<string>(
                name: "EtrTypeId",
                table: "Report",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "EtrStatusId",
                table: "Report",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "EtrStatusId1",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EtrTypeId1",
                table: "Report",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrStatusId1",
                table: "Report",
                column: "EtrStatusId1");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrTypeId1",
                table: "Report",
                column: "EtrTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusId1",
                table: "Report",
                column: "EtrStatusId1",
                principalTable: "EtrStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_EtrType_EtrTypeId1",
                table: "Report",
                column: "EtrTypeId1",
                principalTable: "EtrType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
