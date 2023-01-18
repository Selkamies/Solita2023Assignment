using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solita2023Assignment.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Station",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PublicStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    NameFI = table.Column<string>(type: "TEXT", nullable: true),
                    NameSV = table.Column<string>(type: "TEXT", nullable: true),
                    AddressFI = table.Column<string>(type: "TEXT", nullable: true),
                    AddressSV = table.Column<string>(type: "TEXT", nullable: true),
                    CityFI = table.Column<string>(type: "TEXT", nullable: true),
                    CitySV = table.Column<string>(type: "TEXT", nullable: true),
                    Operator = table.Column<string>(type: "TEXT", nullable: true),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    XCoordinate = table.Column<float>(type: "REAL", nullable: false),
                    YCoordinate = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Journey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReturnTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DepartureStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReturnStationId = table.Column<int>(type: "INTEGER", nullable: false),
                    DistanceMeters = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationSeconds = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Journey_Station_DepartureStationId",
                        column: x => x.DepartureStationId,
                        principalTable: "Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Journey_Station_ReturnStationId",
                        column: x => x.ReturnStationId,
                        principalTable: "Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journey_DepartureStationId",
                table: "Journey",
                column: "DepartureStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Journey_ReturnStationId",
                table: "Journey",
                column: "ReturnStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Journey");

            migrationBuilder.DropTable(
                name: "Station");
        }
    }
}
