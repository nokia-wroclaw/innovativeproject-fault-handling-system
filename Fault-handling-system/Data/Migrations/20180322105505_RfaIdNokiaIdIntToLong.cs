using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fault_handling_system.Data.Migrations
{
    public partial class RfaIdNokiaIdIntToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "RfaId",
                table: "Report",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "NokiaCaseId",
                table: "Report",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RfaId",
                table: "Report",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "NokiaCaseId",
                table: "Report",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
