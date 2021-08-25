using Microsoft.EntityFrameworkCore.Migrations;

namespace ExactScore.Migrations
{
    public partial class TeamAndRoundUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Teams",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "Rounds",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Rounds");
        }
    }
}
