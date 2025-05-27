using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "MovieReviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserRoles",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.CreateIndex(
                name: "IX_MovieReviews_ApplicationUserId",
                table: "MovieReviews",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieReviews_AspNetUsers_ApplicationUserId",
                table: "MovieReviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieReviews_AspNetUsers_ApplicationUserId",
                table: "MovieReviews");

            migrationBuilder.DropIndex(
                name: "IX_MovieReviews_ApplicationUserId",
                table: "MovieReviews");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "MovieReviews");

            migrationBuilder.DropColumn(
                name: "UserRoles",
                table: "AspNetUsers");
        }
    }
}
