using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatelliteTracker.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SatelliteData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Altitude = table.Column<double>(type: "float", nullable: true),
                    SatellitesInUse = table.Column<int>(type: "int", nullable: false),
                    SatelliteSystem = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SatelliteId = table.Column<int>(type: "int", nullable: false),
                    Elevation = table.Column<double>(type: "float", nullable: false),
                    Azimuth = table.Column<double>(type: "float", nullable: false),
                    SignalToNoiseRatio = table.Column<int>(type: "int", nullable: true),
                    UsedInFix = table.Column<bool>(type: "bit", nullable: false),
                    SentenceType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SatelliteData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SatelliteData_SatelliteSystem",
                table: "SatelliteData",
                column: "SatelliteSystem");

            migrationBuilder.CreateIndex(
                name: "IX_SatelliteData_Timestamp",
                table: "SatelliteData",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SatelliteData");
        }
    }
}
