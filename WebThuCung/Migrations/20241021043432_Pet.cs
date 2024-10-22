using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebThuCung.Migrations
{
    /// <inheritdoc />
    public partial class Pet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PetidPet",
                table: "Product",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "idPet",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    idPet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    namePet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.idPet);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_PetidPet",
                table: "Product",
                column: "PetidPet");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Pet_PetidPet",
                table: "Product",
                column: "PetidPet",
                principalTable: "Pet",
                principalColumn: "idPet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Pet_PetidPet",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_Product_PetidPet",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PetidPet",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "idPet",
                table: "Product");
        }
    }
}
