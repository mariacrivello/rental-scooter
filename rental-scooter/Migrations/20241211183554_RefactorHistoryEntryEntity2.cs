using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rental_scooter.Migrations
{
    /// <inheritdoc />
    public partial class RefactorHistoryEntryEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpan",
                table: "RentalHistoryEntries");

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "Scooters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DurationAsTicks",
                table: "RentalHistoryEntries",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationAsTicks",
                table: "RentalHistoryEntries");

            migrationBuilder.AlterColumn<int>(
                name: "StationId",
                table: "Scooters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpan",
                table: "RentalHistoryEntries",
                type: "time",
                nullable: true);
        }
    }
}
