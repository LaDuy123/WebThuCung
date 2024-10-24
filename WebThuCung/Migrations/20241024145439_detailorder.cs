using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebThuCung.Migrations
{
    /// <inheritdoc />
    public partial class detailorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nameColor",
                table: "DetailOrder");

            migrationBuilder.DropColumn(
                name: "nameSize",
                table: "DetailOrder");

            migrationBuilder.AddColumn<string>(
                name: "idColor",
                table: "DetailOrder",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "idSize",
                table: "DetailOrder",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrder_idColor",
                table: "DetailOrder",
                column: "idColor");

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrder_idSize",
                table: "DetailOrder",
                column: "idSize");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailOrder_Color_idColor",
                table: "DetailOrder",
                column: "idColor",
                principalTable: "Color",
                principalColumn: "idColor",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailOrder_Size_idSize",
                table: "DetailOrder",
                column: "idSize",
                principalTable: "Size",
                principalColumn: "idSize",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailOrder_Color_idColor",
                table: "DetailOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailOrder_Size_idSize",
                table: "DetailOrder");

            migrationBuilder.DropIndex(
                name: "IX_DetailOrder_idColor",
                table: "DetailOrder");

            migrationBuilder.DropIndex(
                name: "IX_DetailOrder_idSize",
                table: "DetailOrder");

            migrationBuilder.DropColumn(
                name: "idColor",
                table: "DetailOrder");

            migrationBuilder.DropColumn(
                name: "idSize",
                table: "DetailOrder");

            migrationBuilder.AddColumn<string>(
                name: "nameColor",
                table: "DetailOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "nameSize",
                table: "DetailOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
