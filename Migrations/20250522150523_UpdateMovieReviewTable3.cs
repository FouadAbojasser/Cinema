using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovieReviewTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "MovieReviews",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
