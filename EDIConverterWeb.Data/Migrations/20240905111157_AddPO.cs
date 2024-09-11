using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDIConverterWeb.Data.Migrations
{
    public partial class AddPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsOrdered_PurchaseOrder_PurchaseOrderId",
                table: "ItemsOrdered");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderAcknowledgements_PurchaseOrder_PurchaseOrderId",
                table: "PurchaseOrderAcknowledgements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrder",
                table: "PurchaseOrder");

            migrationBuilder.RenameTable(
                name: "PurchaseOrder",
                newName: "PurchaseOrders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrders",
                table: "PurchaseOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsOrdered_PurchaseOrders_PurchaseOrderId",
                table: "ItemsOrdered",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderAcknowledgements_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderAcknowledgements",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsOrdered_PurchaseOrders_PurchaseOrderId",
                table: "ItemsOrdered");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderAcknowledgements_PurchaseOrders_PurchaseOrderId",
                table: "PurchaseOrderAcknowledgements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseOrders",
                table: "PurchaseOrders");

            migrationBuilder.RenameTable(
                name: "PurchaseOrders",
                newName: "PurchaseOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseOrder",
                table: "PurchaseOrder",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsOrdered_PurchaseOrder_PurchaseOrderId",
                table: "ItemsOrdered",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderAcknowledgements_PurchaseOrder_PurchaseOrderId",
                table: "PurchaseOrderAcknowledgements",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
