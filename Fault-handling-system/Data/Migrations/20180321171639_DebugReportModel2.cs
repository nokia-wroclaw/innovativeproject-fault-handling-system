using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class DebugReportModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrStatus_EtrStatusStatus",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_EtrType_EtrTypeType",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_EtrStatusStatus",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_EtrTypeType",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EtrType",
                table: "EtrType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EtrStatus",
                table: "EtrStatus");

            migrationBuilder.DropColumn(
                name: "EtrStatusStatus",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "EtrTypeType",
                table: "Report");

            migrationBuilder.RenameColumn(
                name: "EtrTypeName",
                table: "Report",
                newName: "EtrTypeId");

            migrationBuilder.RenameColumn(
                name: "EtrStatusName",
                table: "Report",
                newName: "EtrStatusId");

            migrationBuilder.AddColumn<int>(
                name: "EtrStatusId1",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EtrTypeId1",
                table: "Report",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "EtrType",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EtrType",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "EtrStatus",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EtrStatus",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EtrType",
                table: "EtrType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EtrStatus",
                table: "EtrStatus",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_EtrType",
                table: "EtrType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EtrStatus",
                table: "EtrStatus");

            migrationBuilder.DropColumn(
                name: "EtrStatusId1",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "EtrTypeId1",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EtrType");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EtrStatus");

            migrationBuilder.RenameColumn(
                name: "EtrTypeId",
                table: "Report",
                newName: "EtrTypeName");

            migrationBuilder.RenameColumn(
                name: "EtrStatusId",
                table: "Report",
                newName: "EtrStatusName");

            migrationBuilder.AddColumn<string>(
                name: "EtrStatusStatus",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EtrTypeType",
                table: "Report",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "EtrType",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "EtrStatus",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EtrType",
                table: "EtrType",
                column: "Type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EtrStatus",
                table: "EtrStatus",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrStatusStatus",
                table: "Report",
                column: "EtrStatusStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Report_EtrTypeType",
                table: "Report",
                column: "EtrTypeType");

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
    }
}
