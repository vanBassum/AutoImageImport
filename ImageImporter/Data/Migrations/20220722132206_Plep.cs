using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageImporter.Migrations
{
    public partial class Plep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Pictures",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Quality",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "JobResults",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "PictureImportItem_PictureId",
                table: "ActionItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PictureRemovedItem_PictureId",
                table: "ActionItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_PictureImportItem_PictureId",
                table: "ActionItems",
                column: "PictureImportItem_PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_PictureRemovedItem_PictureId",
                table: "ActionItems",
                column: "PictureRemovedItem_PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionItems_Pictures_PictureImportItem_PictureId",
                table: "ActionItems",
                column: "PictureImportItem_PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionItems_Pictures_PictureRemovedItem_PictureId",
                table: "ActionItems",
                column: "PictureRemovedItem_PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionItems_Pictures_PictureImportItem_PictureId",
                table: "ActionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionItems_Pictures_PictureRemovedItem_PictureId",
                table: "ActionItems");

            migrationBuilder.DropIndex(
                name: "IX_ActionItems_PictureImportItem_PictureId",
                table: "ActionItems");

            migrationBuilder.DropIndex(
                name: "IX_ActionItems_PictureRemovedItem_PictureId",
                table: "ActionItems");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Quality",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "JobResults");

            migrationBuilder.DropColumn(
                name: "PictureImportItem_PictureId",
                table: "ActionItems");

            migrationBuilder.DropColumn(
                name: "PictureRemovedItem_PictureId",
                table: "ActionItems");
        }
    }
}
