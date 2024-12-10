using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rental_scooter.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scooters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scooters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scooters_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RentalHistoryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScooterId = table.Column<int>(type: "int", nullable: false),
                    PickUpStationId = table.Column<int>(type: "int", nullable: false),
                    DropOffStationId = table.Column<int>(type: "int", nullable: true),
                    RentalStartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentalEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeSpan = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalHistoryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalHistoryEntries_Scooters_ScooterId",
                        column: x => x.ScooterId,
                        principalTable: "Scooters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RentalHistoryEntries_Stations_DropOffStationId",
                        column: x => x.DropOffStationId,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RentalHistoryEntries_Stations_PickUpStationId",
                        column: x => x.PickUpStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalHistoryEntries_DropOffStationId",
                table: "RentalHistoryEntries",
                column: "DropOffStationId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalHistoryEntries_PickUpStationId",
                table: "RentalHistoryEntries",
                column: "PickUpStationId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalHistoryEntries_ScooterId",
                table: "RentalHistoryEntries",
                column: "ScooterId");

            migrationBuilder.CreateIndex(
                name: "IX_Scooters_StationId",
                table: "Scooters",
                column: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalHistoryEntries");

            migrationBuilder.DropTable(
                name: "Scooters");

            migrationBuilder.DropTable(
                name: "Stations");
        }
    }
}
