using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class EditTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShowTimes");

            migrationBuilder.CreateTable(
                name: "TheaterSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ShowTimeFrom = table.Column<TimeOnly>(type: "time", nullable: false),
                    ShowTimeTo = table.Column<TimeOnly>(type: "time", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    TheaterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheaterSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TheaterSchedules_MovieTheaters_MovieId_TheaterId",
                        columns: x => new { x.MovieId, x.TheaterId },
                        principalTable: "MovieTheaters",
                        principalColumns: new[] { "MovieId", "TheaterId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TheaterSchedules_MovieId_TheaterId",
                table: "TheaterSchedules",
                columns: new[] { "MovieId", "TheaterId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TheaterSchedules");

            migrationBuilder.CreateTable(
                name: "ShowTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    TheaterId = table.Column<int>(type: "int", nullable: false),
                    ShowDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ShowTimeFrom = table.Column<TimeOnly>(type: "time", nullable: false),
                    ShowTimeTo = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShowTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShowTimes_MovieTheaters_MovieId_TheaterId",
                        columns: x => new { x.MovieId, x.TheaterId },
                        principalTable: "MovieTheaters",
                        principalColumns: new[] { "MovieId", "TheaterId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShowTimes_MovieId_TheaterId",
                table: "ShowTimes",
                columns: new[] { "MovieId", "TheaterId" });
        }
    }
}
