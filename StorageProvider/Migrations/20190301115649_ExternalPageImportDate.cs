using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StorageProvider.Migrations
{
    public partial class ExternalPageImportDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExternalFileImportDate",
                table: "Pages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalFileImportDate",
                table: "Pages");
        }
    }
}
