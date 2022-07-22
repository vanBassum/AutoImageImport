using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageImporter.Migrations
{
    public partial class PictureInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Pictures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Pictures");
        }
    }
}
