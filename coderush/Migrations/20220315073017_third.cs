using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace coderush.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "SalesOrder");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "SalesOrder");

            migrationBuilder.DropColumn(
                name: "SalesTypeId",
                table: "SalesOrder");

            migrationBuilder.DropColumn(
                name: "BatchID",
                table: "PurchaseOrderLine");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "PurchaseOrderLine");

            migrationBuilder.DropColumn(
                name: "ManufareDate",
                table: "PurchaseOrderLine");

            migrationBuilder.AddColumn<int>(
                name: "goodsRecievedNoteLineId",
                table: "GoodsReceivedNote",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GoodsRecievedNoteLine",
                columns: table => new
                {
                    GoodsRecievedNoteLineId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    BatchID = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DiscountAmount = table.Column<double>(nullable: false),
                    DiscountPercentage = table.Column<double>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    GoodsReceivedNoteId = table.Column<int>(nullable: false),
                    ManufareDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    SubTotal = table.Column<double>(nullable: false),
                    TaxAmount = table.Column<double>(nullable: false),
                    TaxPercentage = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsRecievedNoteLine", x => x.GoodsRecievedNoteLineId);
                    table.ForeignKey(
                        name: "FK_GoodsRecievedNoteLine_GoodsReceivedNote_GoodsReceivedNoteId",
                        column: x => x.GoodsReceivedNoteId,
                        principalTable: "GoodsReceivedNote",
                        principalColumn: "GoodsReceivedNoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsRecievedNoteLine_GoodsReceivedNoteId",
                table: "GoodsRecievedNoteLine",
                column: "GoodsReceivedNoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsRecievedNoteLine");

            migrationBuilder.DropColumn(
                name: "goodsRecievedNoteLineId",
                table: "GoodsReceivedNote");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "SalesOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "SalesOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SalesTypeId",
                table: "SalesOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BatchID",
                table: "PurchaseOrderLine",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "PurchaseOrderLine",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufareDate",
                table: "PurchaseOrderLine",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
