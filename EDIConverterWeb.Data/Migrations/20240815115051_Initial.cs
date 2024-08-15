using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDIConverterWeb.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrderAcknowledgements",
                columns: table => new
                {
                    ReferenceNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000000001, 100001"),
                    InterchangeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcknowledgementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledShipDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestIndicator = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderAcknowledgements", x => x.ReferenceNumber);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemsOrdered",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineNumber = table.Column<int>(type: "int", nullable: false),
                    QuantityOrdered = table.Column<int>(type: "int", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ItemNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderAcknowledgementId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsOrdered", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsOrdered_PurchaseOrderAcknowledgements_PurchaseOrderAcknowledgementId",
                        column: x => x.PurchaseOrderAcknowledgementId,
                        principalTable: "PurchaseOrderAcknowledgements",
                        principalColumn: "ReferenceNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOrdered_PurchaseOrderAcknowledgementId",
                table: "ItemsOrdered",
                column: "PurchaseOrderAcknowledgementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsOrdered");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "PurchaseOrderAcknowledgements");
        }
    }
}
