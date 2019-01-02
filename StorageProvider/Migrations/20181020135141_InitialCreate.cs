using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StorageProvider.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"Pages",
				table => new
				{
					Id = table.Column<string>(nullable: false),
					Content = table.Column<string>(nullable: true),
					CreationTime = table.Column<DateTimeOffset>(nullable: false),
					ChangeTime = table.Column<DateTimeOffset>(nullable: false),
					ValidFrom = table.Column<DateTimeOffset>(nullable: false),
					ValidTo = table.Column<DateTimeOffset>(nullable: false),
					IsFavorite = table.Column<bool>(nullable: false),
					IsLocked = table.Column<bool>(nullable: false)
				},
				constraints: table => { table.PrimaryKey("PK_Pages", x => x.Id); });

			migrationBuilder.CreateTable(
				"Tags",
				table => new
				{
					Name = table.Column<string>(nullable: false),
					ContentPageId = table.Column<string>(nullable: true),
					TagName = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Tags", x => x.Name);
					table.ForeignKey(
						"FK_Tags_Pages_ContentPageId",
						x => x.ContentPageId,
						"Pages",
						"Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						"FK_Tags_Tags_TagName",
						x => x.TagName,
						"Tags",
						"Name",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				"IX_Tags_ContentPageId",
				"Tags",
				"ContentPageId");

			migrationBuilder.CreateIndex(
				"IX_Tags_TagName",
				"Tags",
				"TagName");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"Tags");

			migrationBuilder.DropTable(
				"Pages");
		}
	}
}