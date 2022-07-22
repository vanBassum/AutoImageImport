using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageImporter.Migrations
{
    public partial class JobResultsDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "JobResults",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Started",
                table: "JobResults",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "JobResults");

            migrationBuilder.DropColumn(
                name: "Started",
                table: "JobResults");
        }
    }
}
