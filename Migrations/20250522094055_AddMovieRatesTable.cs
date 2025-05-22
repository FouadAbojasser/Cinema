using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieRatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActorsRate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DirectorRate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "GraphicsRate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "OverallRate",
                table: "Reviews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActorsRate",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DirectorRate",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GraphicsRate",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OverallRate",
                table: "Reviews",
                type: "int",
                nullable: true);
        }
    }
}
