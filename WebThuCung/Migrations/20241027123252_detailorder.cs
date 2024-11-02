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
            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailOrder",
                table: "DetailOrder");

            migrationBuilder.AlterColumn<string>(
                name: "idProduct",
                table: "DetailOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "idOrder",
                table: "DetailOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "IdDetailOrder",
                table: "DetailOrder",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailOrder",
                table: "DetailOrder",
                column: "IdDetailOrder");

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrder_idOrder",
                table: "DetailOrder",
                column: "idOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailOrder",
                table: "DetailOrder");

            migrationBuilder.DropIndex(
                name: "IX_DetailOrder_idOrder",
                table: "DetailOrder");

            migrationBuilder.DropColumn(
                name: "IdDetailOrder",
                table: "DetailOrder");

            migrationBuilder.AlterColumn<string>(
                name: "idProduct",
                table: "DetailOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<string>(
                name: "idOrder",
                table: "DetailOrder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailOrder",
                table: "DetailOrder",
                columns: new[] { "idOrder", "idProduct" });
        }
    }
}
