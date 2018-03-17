using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class ReportEtrNumberNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EtrNumber",
                table: "Report",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EtrNumber",
                table: "Report",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
