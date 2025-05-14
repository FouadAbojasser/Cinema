using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Series_SeriesId",
                table: "Image");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Series_SeriesId",
                table: "Image",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_Series_SeriesId",
                table: "Image");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Movies_MovieId",
                table: "Image",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Series_SeriesId",
                table: "Image",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
