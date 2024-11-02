using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebThuCung.Migrations
{
    /// <inheritdoc />
    public partial class transaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    idTransaction = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameCustomer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    shippingAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    idCustomer = table.Column<int>(type: "int", nullable: false),
                    idOrder = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.idTransaction);
                    table.ForeignKey(
                        name: "FK_Transaction_Customer_idCustomer",
                        column: x => x.idCustomer,
                        principalTable: "Customer",
                        principalColumn: "idCustomer");

                    table.ForeignKey(
                        name: "FK_Transaction_Order_idOrder",
                        column: x => x.idOrder,
                        principalTable: "Order",
                        principalColumn: "idOrder");
                        
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_idCustomer",
                table: "Transaction",
                column: "idCustomer");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_idOrder",
                table: "Transaction",
                column: "idOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
