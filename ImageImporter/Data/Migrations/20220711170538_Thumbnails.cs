using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageImporter.Migrations
{
    public partial class Thumbnails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Pictures",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImporterType",
                table: "ImportResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchedWithId",
                table: "ImportResults",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RemovedFileThumb",
                table: "ImportResults",
                type: "text",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ImporterType",
                table: "ImportResults");

            migrationBuilder.DropColumn(
                name: "MatchedWithId",
                table: "ImportResults");

            migrationBuilder.DropColumn(
                name: "RemovedFileThumb",
                table: "ImportResults");
        }
    }
}
