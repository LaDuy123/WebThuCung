using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebThuCung.Migrations
{
    /// <inheritdoc />
    public partial class fiximageproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageProduct_Product_ProductidProduct",
                table: "ImageProduct");

            migrationBuilder.DropIndex(
                name: "IX_ImageProduct_ProductidProduct",
                table: "ImageProduct");

            migrationBuilder.DropColumn(
                name: "ProductidProduct",
                table: "ImageProduct");

            migrationBuilder.AlterColumn<string>(
                name: "idProduct",
                table: "ImageProduct",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ImageProduct_idProduct",
                table: "ImageProduct",
                column: "idProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageProduct_Product_idProduct",
                table: "ImageProduct",
                column: "idProduct",
                principalTable: "Product",
                principalColumn: "idProduct",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageProduct_Product_idProduct",
                table: "ImageProduct");

            migrationBuilder.DropIndex(
                name: "IX_ImageProduct_idProduct",
                table: "ImageProduct");

            migrationBuilder.AlterColumn<string>(
                name: "idProduct",
                table: "ImageProduct",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ProductidProduct",
                table: "ImageProduct",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ImageProduct_ProductidProduct",
                table: "ImageProduct",
                column: "ProductidProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageProduct_Product_ProductidProduct",
                table: "ImageProduct",
                column: "ProductidProduct",
                principalTable: "Product",
                principalColumn: "idProduct",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
