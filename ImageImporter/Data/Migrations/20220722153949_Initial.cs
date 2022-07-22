using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace ImageImporter.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Started = table.Column<DateTime>(type: "datetime", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    File = table.Column<string>(type: "text", nullable: true),
                    Hash = table.Column<long>(type: "bigint", nullable: true),
                    Thumbnail = table.Column<string>(type: "text", nullable: true),
                    Quality = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ActionItemId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    JobResultId = table.Column<int>(type: "int", nullable: true),
                    PictureId = table.Column<int>(type: "int", nullable: true),
                    PictureRemovedItem_PictureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionItems_ActionItems_ActionItemId",
                        column: x => x.ActionItemId,
                        principalTable: "ActionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionItems_JobResults_JobResultId",
                        column: x => x.JobResultId,
                        principalTable: "JobResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionItems_Pictures_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionItems_Pictures_PictureRemovedItem_PictureId",
                        column: x => x.PictureRemovedItem_PictureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_ActionItemId",
                table: "ActionItems",
                column: "ActionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_JobResultId",
                table: "ActionItems",
                column: "JobResultId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_PictureId",
                table: "ActionItems",
                column: "PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionItems_PictureRemovedItem_PictureId",
                table: "ActionItems",
                column: "PictureRemovedItem_PictureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionItems");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "JobResults");

            migrationBuilder.DropTable(
                name: "Pictures");
        }
    }
}
