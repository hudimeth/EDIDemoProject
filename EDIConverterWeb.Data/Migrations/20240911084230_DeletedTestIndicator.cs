using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDIConverterWeb.Data.Migrations
{
    public partial class DeletedTestIndicator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestIndicator",
                table: "PurchaseOrderAcknowledgements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestIndicator",
                table: "PurchaseOrderAcknowledgements",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }
    }
}
