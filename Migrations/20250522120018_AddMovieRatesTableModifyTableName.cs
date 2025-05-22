using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieRatesTableModifyTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "movieRates");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserName",
                table: "movieRates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserName",
                table: "movieRates");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "movieRates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
