using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebThuCung.Migrations
{
    /// <inheritdoc />
    public partial class imageproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shape");

            migrationBuilder.RenameColumn(
                name: "namCategory",
                table: "Category",
                newName: "nameCategory");

            migrationBuilder.RenameColumn(
                name: "namBranch",
                table: "Branch",
                newName: "nameBranch");

            migrationBuilder.CreateTable(
                name: "ImageProduct",
                columns: table => new
                {
                    idImageProduct = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductidProduct = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageProduct", x => x.idImageProduct);
                    table.ForeignKey(
                        name: "FK_ImageProduct_Product_ProductidProduct",
                        column: x => x.ProductidProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageProduct_ProductidProduct",
                table: "ImageProduct",
                column: "ProductidProduct");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageProduct");

            migrationBuilder.RenameColumn(
                name: "nameCategory",
                table: "Category",
                newName: "namCategory");

            migrationBuilder.RenameColumn(
                name: "nameBranch",
                table: "Branch",
                newName: "namBranch");

            migrationBuilder.CreateTable(
                name: "Shape",
                columns: table => new
                {
                    idShape = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductidProduct = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idProduct = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shape", x => x.idShape);
                    table.ForeignKey(
                        name: "FK_Shape_Product_ProductidProduct",
                        column: x => x.ProductidProduct,
                        principalTable: "Product",
                        principalColumn: "idProduct");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shape_ProductidProduct",
                table: "Shape",
                column: "ProductidProduct");
        }
    }
}
