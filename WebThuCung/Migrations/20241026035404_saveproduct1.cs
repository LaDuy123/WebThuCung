using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebThuCung.Migrations
{
    /// <inheritdoc />
    public partial class saveproduct1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SaveProduct",
                columns: table => new
                {
                    idProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idCustomer = table.Column<int>(type: "int", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveProduct", x => new { x.idProduct, x.idCustomer });
                    table.ForeignKey(
                        name: "FK_SaveProduct_Customer_idCustomer",
                        column: x => x.idCustomer,
                        principalTable: "Customer",
                        principalColumn: "idCustomer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaveProduct_Product_idProduct",
                        column: x => x.idProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaveProduct_idCustomer",
                table: "SaveProduct",
                column: "idCustomer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaveProduct");
        }
    }
}
