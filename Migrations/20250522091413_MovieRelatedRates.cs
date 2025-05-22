using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class MovieRelatedRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_applicationUserId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "applicationUserId",
                table: "Reviews",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "UserRate",
                table: "Reviews",
                newName: "OverallRate");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_applicationUserId",
                table: "Reviews",
                newName: "IX_Reviews_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.CreateTable(
                name: "MovieTheater",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "int", nullable: false),
                    TheatersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieTheater", x => new { x.MoviesId, x.TheatersId });
                    table.ForeignKey(
                        name: "FK_MovieTheater_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieTheater_Theaters_TheatersId",
                        column: x => x.TheatersId,
                        principalTable: "Theaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieTheater_TheatersId",
                table: "MovieTheater",
                column: "TheatersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "MovieTheater");

            migrationBuilder.DropColumn(
                name: "ActorsRate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DirectorRate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "GraphicsRate",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Reviews",
                newName: "applicationUserId");

            migrationBuilder.RenameColumn(
                name: "OverallRate",
                table: "Reviews",
                newName: "UserRate");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                newName: "IX_Reviews_applicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_applicationUserId",
                table: "Reviews",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
