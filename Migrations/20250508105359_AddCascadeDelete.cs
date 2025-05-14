using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
