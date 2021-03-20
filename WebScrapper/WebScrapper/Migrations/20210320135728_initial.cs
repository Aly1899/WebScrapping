using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebScrapper.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdPrices",
                columns: table => new
                {
                    AdPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RealEstateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FetchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdPrices", x => x.AdPriceId);
                });

            migrationBuilder.CreateTable(
                name: "FetchDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryDate = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FetchDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealEstates",
                columns: table => new
                {
                    RealEstateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PricePerSqm = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlotSize = table.Column<int>(type: "int", nullable: true),
                    LeaseRights = table.Column<bool>(type: "bit", nullable: false),
                    Balcony = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealEstates", x => x.RealEstateId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdPrices");

            migrationBuilder.DropTable(
                name: "FetchDates");

            migrationBuilder.DropTable(
                name: "RealEstates");
        }
    }
}
