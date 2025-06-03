using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTheaterRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TheaterSchedules_MovieTheaters_MovieId_TheaterId",
                table: "TheaterSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TheaterSchedules_MovieId_TheaterId",
                table: "TheaterSchedules");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "MovieTheaters");

            migrationBuilder.DropColumn(
                name: "ReservedTickets",
                table: "MovieTheaters");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MovieTheaters");

            migrationBuilder.DropColumn(
                name: "TotalNumberOfTickets",
                table: "MovieTheaters");

            migrationBuilder.CreateIndex(
                name: "IX_TheaterSchedules_MovieId",
                table: "TheaterSchedules",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_TheaterSchedules_TheaterId",
                table: "TheaterSchedules",
                column: "TheaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_TheaterSchedules_Movies_MovieId",
                table: "TheaterSchedules",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TheaterSchedules_Theaters_TheaterId",
                table: "TheaterSchedules",
                column: "TheaterId",
                principalTable: "Theaters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TheaterSchedules_Movies_MovieId",
                table: "TheaterSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TheaterSchedules_Theaters_TheaterId",
                table: "TheaterSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TheaterSchedules_MovieId",
                table: "TheaterSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TheaterSchedules_TheaterId",
                table: "TheaterSchedules");

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "MovieTheaters",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservedTickets",
                table: "MovieTheaters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "MovieTheaters",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalNumberOfTickets",
                table: "MovieTheaters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TheaterSchedules_MovieId_TheaterId",
                table: "TheaterSchedules",
                columns: new[] { "MovieId", "TheaterId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TheaterSchedules_MovieTheaters_MovieId_TheaterId",
                table: "TheaterSchedules",
                columns: new[] { "MovieId", "TheaterId" },
                principalTable: "MovieTheaters",
                principalColumns: new[] { "MovieId", "TheaterId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
