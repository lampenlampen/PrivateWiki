using Microsoft.EntityFrameworkCore.Migrations;

namespace StorageProvider.Migrations
{
    public partial class ExternalPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalFileToken",
                table: "Pages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalFileToken",
                table: "Pages");
        }
    }
}
