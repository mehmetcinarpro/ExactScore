using Microsoft.EntityFrameworkCore.Migrations;

namespace ExactScore.Migrations
{
    public partial class IdentityUserInPredictionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Predictions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predictions_IdentityUserId",
                table: "Predictions",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_AspNetUsers_IdentityUserId",
                table: "Predictions",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_AspNetUsers_IdentityUserId",
                table: "Predictions");

            migrationBuilder.DropIndex(
                name: "IX_Predictions_IdentityUserId",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Predictions");
        }
    }
}
